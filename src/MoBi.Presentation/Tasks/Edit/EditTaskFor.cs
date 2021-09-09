using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using System;
using OSPSuite.Core.Serialization;
using IContainer = OSPSuite.Utility.Container.IContainer;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain.Services;
using MoBi.Core.Serialization.Xml.Services;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskFor<T>
   {
      void Edit(T objectToEdit);
      void Save(T entityToSerialize);

      T Clone(T entityToClone);

      /// <summary>
      ///    Gets the forbidden names for the given object, from the local list the objects name is removed.
      ///    This is done to ensure that an already added objects name is not considered as forbidden, for the object itself.
      /// </summary>
      /// <returns> The forbidden names.</returns>
      IEnumerable<string> GetForbiddenNamesWithoutSelf(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent);

      /// <summary>
      ///    Gets the forbidden names here the active objects name maybe forbidden.
      /// </summary>
      /// <returns> The forbidden names.</returns>
      IEnumerable<string> GetForbiddenNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent);

      void Rename<TEntity>(TEntity entity, IBuildingBlock buildingBlock) where TEntity : T, IEntity;
      void Rename(T objectToRename, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock);
      string ObjectName { get; }
      bool EditEntityModal(T newEntity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock);
      string IconFor(IObjectBase objectBase);
   }

   public abstract class EditTaskFor<T> : IEditTaskFor<T> where T : class, IObjectBase
   {
      protected readonly IMoBiContext _context;
      protected readonly IMoBiApplicationController _applicationController;
      protected readonly IInteractionTask _interactionTask;
      protected readonly IInteractionTaskContext _interactionTaskContext;
      private readonly IMoBiXmlSerializerRepository _xmlSerializerRepository;
      private readonly IContainer _container;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManagerForModel;
      public string ObjectName { get; private set; }

      protected EditTaskFor(
         IInteractionTaskContext interactionTaskContext,
         IMoBiXmlSerializerRepository xmlSerializerRepository = null, 
         IContainer container = null,
         IDimensionFactory dimensionFactory = null,
         IObjectBaseFactory objectBaseFactory = null,
         ICloneManagerForModel cloneManagerForModel = null)
      {
         _interactionTaskContext = interactionTaskContext;
         _applicationController = interactionTaskContext.ApplicationController;
         _interactionTask = interactionTaskContext.InteractionTask;
         _context = interactionTaskContext.Context;
         ObjectName = _interactionTaskContext.GetTypeFor<T>();
         _xmlSerializerRepository = xmlSerializerRepository;
         _container = container;
         _dimensionFactory = dimensionFactory;
         _objectBaseFactory = objectBaseFactory;
         _cloneManagerForModel = cloneManagerForModel;
      }

      public virtual void Edit(T objectToEdit)
      {
         _context.PublishEvent(new EntitySelectedEvent(objectToEdit, this));
      }

      protected TEntity Clone<TEntity>(TEntity entityToClone) where TEntity : class
      {
         using (var serializationContext = SerializationTransaction.Create(_container,
            _dimensionFactory,
            _objectBaseFactory,
            new WithIdRepository(),
            _cloneManagerForModel))
         {
            var serializer = _xmlSerializerRepository.SerializerFor(entityToClone);
            var element = serializer.Serialize(entityToClone, serializationContext);
            return serializer.Deserialize<TEntity>(element, serializationContext);
         }
      }

      public virtual T Clone(T entityToClone)
      {
         var newEntity = Clone<T>(entityToClone);
         newEntity.Id = Guid.NewGuid().ToString();
         return newEntity;
      }

      public virtual void Rename<TEntity>(TEntity entity, IBuildingBlock buildingBlock) where TEntity : T, IEntity
      {
         Rename(entity, entity.ParentContainer, buildingBlock);
      }

      public virtual void Rename(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock)
      {
         var forbiddenNames = GetForbiddenNames(objectBase, existingObjectsInParent);
         _context.AddToHistory(_interactionTask.Rename(objectBase, forbiddenNames, buildingBlock));
      }

      public IEnumerable<string> GetForbiddenNamesWithoutSelf(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var forbiddenNames = GetForbiddenNames(objectBase, existingObjectsInParent).ToList();
         forbiddenNames.Remove(objectBase.Name);
         return forbiddenNames;
      }

      public IEnumerable<string> GetForbiddenNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         var unallowedNames = GetUnallowedNames(objectBase, existingObjectsInParent).ToList();
         return unallowedNames.Union(_interactionTask.ForbiddenNamesFor(objectBase));
      }

      public string IconFor(IObjectBase objectBase)
      {
         return _interactionTask.IconFor(objectBase);
      }

      protected virtual IEnumerable<string> GetUnallowedNames(T objectBase, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return existingObjectsInParent.AllNames();
      }

      public virtual void Save(T entityToSerialize)
      {
         _interactionTask.Save(entityToSerialize);
      }

      public virtual bool EditEntityModal(T entity, IEnumerable<IObjectBase> existingObjectsInParent, ICommandCollector commandCollector, IBuildingBlock buildingBlock)
      {
         using (var modalPresenter = GetCreateViewFor(entity, commandCollector))
         {
            InitializeSubPresenter(modalPresenter.SubPresenter, buildingBlock, entity);
            ((ICreatePresenter<T>)modalPresenter.SubPresenter).Edit(entity, existingObjectsInParent);
            return modalPresenter.Show();
         }
      }

      protected virtual IModalPresenter GetCreateViewFor(T entity, ICommandCollector command)
      {
         return _applicationController.GetCreateViewFor(entity, command);
      }

      /// <summary>
      ///    Initializes the sub presenter. Should be overridden for special initializations
      /// </summary>
      protected virtual void InitializeSubPresenter(IPresenter subPresenter, IBuildingBlock buildingBlock, T entity)
      {
         var presenterWithFormulaCache = subPresenter as IPresenterWithFormulaCache;
         if (presenterWithFormulaCache != null)
         {
            presenterWithFormulaCache.BuildingBlock = buildingBlock;
         }
      }
   }
}