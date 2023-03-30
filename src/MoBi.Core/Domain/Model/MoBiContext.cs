using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.Core;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Serialization.Journal;
using OSPSuite.Infrastructure.Serialization.ORM.History;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.FileLocker;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiContext : IOSPSuiteExecutionContext<IMoBiProject>, IWorkspace
   {
      IMoBiDimensionFactory DimensionFactory { get; }
      IObjectBaseFactory ObjectBaseFactory { get; }
      IWithIdRepository ObjectRepository { get; }
      IContainer Container { get; }

      T Create<T>(string id) where T : class, IObjectBase;
      T Create<T>() where T : class, IObjectBase;

      IMoBiHistoryManager HistoryManager { get; set; }
      IObjectPathFactory ObjectPathFactory { get; }
      void NewProject();
      void LoadFrom(IMoBiProject project);

      /// <summary>
      ///    Converts the given <paramref name="value" /> to a representation that should be saved in commands. The returned
      ///    value is typically used when restoring the execution data to execute the undo command. To restore the value, use
      ///    <see cref="DeserializeValueTo" />
      /// </summary>
      string SerializeValue(object value);

      /// <summary>
      ///    Converts the given <paramref name="valueAsString" /> to the corresponding object of type
      ///    <paramref name="propertyType" />. The <paramref name="valueAsString" />
      ///    is typically created using the <see cref="SerializeValue" />
      /// </summary>
      object DeserializeValueTo(Type propertyType, string valueAsString);

      void UnregisterSimulation(IMoBiSimulation simulation);
   }

   public class MoBiContext : Workspace<IMoBiProject>, IMoBiContext
   {
      private readonly IXmlSerializationService _serializationService;
      private readonly IHistoryManagerFactory _historyManagerFactory;
      private readonly IRegisterTask _registerTask;
      private readonly IUnregisterTask _unregisterTask;
      private readonly IClipboardManager _clipboardManager;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly ILazyLoadTask _lazyLoadTask;
      private readonly IJournalSession _journalSession;

      public IContainer Container { get; }
      public IMoBiHistoryManager HistoryManager { get; set; }
      public IMoBiDimensionFactory DimensionFactory { get; }
      public IObjectBaseFactory ObjectBaseFactory { get; }
      public IObjectPathFactory ObjectPathFactory { get; }
      public IEventPublisher EventPublisher { get; }
      public IWithIdRepository ObjectRepository { get; }

      public MoBiContext(IObjectBaseFactory objectBaseFactory, IMoBiDimensionFactory dimensionFactory, IEventPublisher eventPublisher,
         IXmlSerializationService serializationService, IObjectPathFactory objectPathFactory, IWithIdRepository objectBaseRepository,
         IHistoryManagerFactory historyManagerFactory, IRegisterTask registerTask, IUnregisterTask unregisterTask,
         IClipboardManager clipboardManager, IContainer container, IObjectTypeResolver objectTypeResolver,
         ICloneManagerForBuildingBlock cloneManager, IJournalSession journalSession, IFileLocker fileLocker, ILazyLoadTask lazyLoadTask) : base(eventPublisher, fileLocker)
      {
         ObjectBaseFactory = objectBaseFactory;
         ObjectRepository = objectBaseRepository;
         EventPublisher = eventPublisher;
         DimensionFactory = dimensionFactory;
         ObjectPathFactory = objectPathFactory;
         _serializationService = serializationService;
         Container = container;
         _objectTypeResolver = objectTypeResolver;
         _cloneManager = cloneManager;
         _lazyLoadTask = lazyLoadTask;
         _historyManagerFactory = historyManagerFactory;
         _registerTask = registerTask;
         _unregisterTask = unregisterTask;
         _clipboardManager = clipboardManager;
         _journalSession = journalSession;
      }

      public IMoBiProject CurrentProject
      {
         get => _project;
         set => _project = value;
      }

      public T Create<T>() where T : class, IObjectBase
      {
         return ObjectBaseFactory.Create<T>();
      }

      public T Get<T>(string id) where T : class, IWithId
      {
         return Get(id) as T;
      }

      public IWithId Get(string id)
      {
         return ObjectRepository.Get(id);
      }

      public T Create<T>(string id) where T : class, IObjectBase
      {
         return id != null ? ObjectBaseFactory.Create<T>(id) : ObjectBaseFactory.Create<T>();
      }

      public void AddToHistory(ICommand command)
      {
         HistoryManager?.AddCommand(command);
      }

      public void ProjectChanged()
      {
         if (CurrentProject == null) return;
         CurrentProject.HasChanged = true;
      }

      public T Clone<T>(T objectToClone) where T : class, IObjectBase
      {
         return _cloneManager.Clone(objectToClone);
      }

      public void Load(IObjectBase objectBase)
      {
         _lazyLoadTask.Load(objectBase as ILazyLoadable);
      }

      public IProject Project => CurrentProject;

      public void NewProject()
      {
         Clear();
         CurrentProject = ObjectBaseFactory.Create<IMoBiProject>();
         HistoryManager = _historyManagerFactory.Create() as IMoBiHistoryManager;
         LoadFrom(CurrentProject);
         AddToHistory(new CreateProjectCommand().Run(this));
      }

      public override void Clear()
      {
         HistoryManager = null;
         ObjectRepository.Clear();
         _clipboardManager.Clear();
         _journalSession.Close();
         base.Clear();
      }

      public byte[] Serialize<T>(T toSerialize)
      {
         return _serializationService.SerializeAsBytes(toSerialize);
      }

      public T Deserialize<T>(byte[] serializationStream)
      {
         return _serializationService.Deserialize<T>(serializationStream, CurrentProject);
      }

      public void PublishEvent<T>(T eventToPublish)
      {
         EventPublisher.PublishEvent(eventToPublish);
      }

      public void Register(IWithId objectBase)
      {
         _registerTask.RegisterAllIn(objectBase);
      }

      public void Unregister(IWithId objectBase)
      {
         _unregisterTask.UnregisterAllIn(objectBase);
      }

      public T Resolve<T>()
      {
         return Container.Resolve<T>();
      }

      public void LoadFrom(IMoBiProject project)
      {
         CurrentProject = project;
         _registerTask.Register(project);
      }

      public string SerializeValue(object value)
      {
         if (value == null)
            return String.Empty;

         if (value.IsAnImplementationOf<IObjectBase>())
            return value.DowncastTo<IObjectBase>().Id;

         if (value.IsAnImplementationOf<IDimension>())
            return value.DowncastTo<IDimension>().Name;

         if (value.IsAnImplementationOf<ObjectPath>())
            return value.DowncastTo<ObjectPath>().PathAsString;

         if (value.IsAnImplementationOf<Unit>())
            return value.DowncastTo<Unit>().Name;

         return value.ConvertedTo<String>();
      }

      public object DeserializeValueTo(Type propertyType, string valueAsString)
      {
         if (valueAsString.IsNullOrEmpty())
         {
            if (propertyType.IsAnImplementationOf<string>())
               return string.Empty;

            if (!propertyType.IsValueType)
               return null;
         }

         if (propertyType.IsAnImplementationOf<IObjectBase>())
            return ObjectRepository.Get(valueAsString);

         if (propertyType.IsAnImplementationOf<IDimension>())
            return DimensionFactory.Dimension(valueAsString);

         if (propertyType.IsAnImplementationOf<ObjectPath>())
            return ObjectPathFactory.CreateObjectPathFrom(valueAsString.ToPathArray());

         if (propertyType.IsAnImplementationOf<Unit>())
            return DimensionFactory.DimensionForUnit(valueAsString).UnitOrDefault(valueAsString);

         if (propertyType.IsAnImplementationOf<Enum>())
            return Enum.Parse(propertyType, valueAsString);

         return Convert.ChangeType(valueAsString, propertyType);
      }

      public void UnregisterSimulation(IMoBiSimulation simulation)
      {
         Unregister(simulation);
         unregisterCachedFormulaInModel(simulation.Model);
      }

      private void unregisterCachedFormulaInModel(IModel model)
      {
         //Cacheable Formulas are not automatically unregistered we need to do this in an extra step
         unregisterAllCachableFormulasIn(model.Root);
         unregisterAllCachableFormulasIn(model.Neighborhoods);
      }

      private void unregisterAllCachableFormulasIn(OSPSuite.Core.Domain.IContainer container)
      {
         container.GetAllChildren<IUsingFormula>(x => x.Formula.IsCachable()).Each(x => Unregister(x.Formula));
      }

      public string TypeFor<T>(T obj) where T : class
      {
         return _objectTypeResolver.TypeFor(obj);
      }
   }
}