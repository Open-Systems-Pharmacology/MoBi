using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public class SerializationTask : ISerializationTask
   {
      private readonly IXmlSerializationService _xmlSerializationService;
      private readonly IContextPersistor _contextPersistor;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IDialogCreator _dialogCreator;
      private readonly IXmlContentSelector _xmlContentSelector;
      private readonly IProjectConverterLogger _projectConverterLogger;
      private readonly IMoBiContext _context;
      private readonly IPostSerializationStepsMaker _postSerializationSteps;
      private readonly IHeavyWorkManager _heavyWorkManager;

      public SerializationTask(IXmlSerializationService xmlSerializationService, IContextPersistor contextPersistor,
         IObjectTypeResolver objectTypeResolver, IDialogCreator dialogCreator,
         IXmlContentSelector xmlContentSelector, IProjectConverterLogger projectConverterLogger,
         IMoBiContext context, IPostSerializationStepsMaker postSerializationSteps, IHeavyWorkManager heavyWorkManager)
      {
         _xmlSerializationService = xmlSerializationService;
         _contextPersistor = contextPersistor;
         _objectTypeResolver = objectTypeResolver;
         _dialogCreator = dialogCreator;
         _xmlContentSelector = xmlContentSelector;
         _projectConverterLogger = projectConverterLogger;

         _context = context;
         _postSerializationSteps = postSerializationSteps;
         _heavyWorkManager = heavyWorkManager;
      }

      public void LoadProject(string fileName)
      {
         if (!tryLockFile(fileName))
            return;

         try
         {
            _heavyWorkManager.Start(() => _contextPersistor.Load(_context, fileName), AppConstants.Captions.LoadingProject);
         }
         catch (ConversionException e)
         {
            _dialogCreator.MessageBoxInfo(e.Message);
         }
      }

      private bool tryLockFile(string projectFile)
      {
         try
         {
            _context.LockFile(projectFile);
            return true;
         }
         catch (CannotLockFileException e)
         {
            var ans = _dialogCreator.MessageBoxYesNo(AppConstants.Exceptions.ProjectWillBeOpenedAsReadOnly(e.Message), AppConstants.Captions.OpenAnyway, AppConstants.Captions.CancelButton);
            var shouldOpenInReadOnly = (ans == ViewResult.Yes);
            if (!shouldOpenInReadOnly) return false;
            _context.ProjectIsReadOnly = true;
            return true;
         }
      }

      public void SaveProject()
      {
         var project = _context.CurrentProject;
         if (project == null) return;

         //try to lock the file if it exisits or is not lock already
         _context.LockFile(project.FilePath);

         _contextPersistor.Save(_context);

         //once the project was saved, we should be able to access the file we just saved
         _context.AccessFile(project.FilePath);

         //we just save the project. It is not readonly per construction
         _context.ProjectIsReadOnly = false;
      }

      public void CloseProject()
      {
         _contextPersistor.CloseProject(_context);
      }

      public void NewProject()
      {
         _contextPersistor.NewProject(_context);
      }

      public void LoadJournal(string journalPath, string projectFullPath = null, bool showJournal = false)
      {
         _contextPersistor.LoadJournal(_context, journalPath, projectFullPath, showJournal);
      }

      public void SaveModelPart<T>(T entityToSerialize, string filename)
      {
         var xelPart = _xmlSerializationService.SerializeModelPart(entityToSerialize);
         xelPart.Save(filename);
      }

      public T Load<T>(string fileName, bool resetIds = false)
      {
         return LoadMany<T>(fileName, resetIds).FirstOrDefault();
      }

      public IEnumerable<T> LoadMany<T>(string fileName, bool resetIds = false)
      {
         _projectConverterLogger.Clear();

         XElement xelRoot = XElement.Load(fileName);
         if (string.IsNullOrEmpty(fileName))
            return Enumerable.Empty<T>();

         var possibleElementsToDeserialize = retrieveElementsToDeserializeFromFile<T>(xelRoot, fileName).ToList();

         var selection = possibleElementsToDeserialize.Count > 1
            ? selectToDeserialize(possibleElementsToDeserialize, _objectTypeResolver.TypeFor<T>())
            : possibleElementsToDeserialize;

         if (!selection.Any())
            return Enumerable.Empty<T>();

         var deserializedObjects = new List<T>();
         int version = _xmlSerializationService.VersionFrom(xelRoot);
         var currentProject = _context.CurrentProject;
         selection.Each(element => deserializedObjects.Add(_xmlSerializationService.Deserialize<T>(element, currentProject, version)));

         var notificationMessages = _projectConverterLogger.AllMessages().ToList();
         if (notificationMessages.Any())
            _context.PublishEvent(new ShowNotificationsEvent(notificationMessages));

         _postSerializationSteps.PerformPostDeserializationFor(deserializedObjects, version, resetIds);

         return deserializedObjects;
      }

      private IEnumerable<XElement> retrieveElementsToDeserializeFromFile<T>(XElement xelRoot, string fileName)
      {
         var elementName = _xmlSerializationService.ElementNameFor(typeof (T));

         //models only required when importing simulations. Otherwise, too many containers are loaded
         if (!loadingSimulation<T>())
            xelRoot = removeModelFrom(xelRoot);

         var possibleElementsToDeserialize = retrieveElementsToDeserializeFromRootNode(xelRoot, elementName).ToList();
         if (possibleElementsToDeserialize.Any())
            return possibleElementsToDeserialize;

         throw new NotMatchingSerializationFileException(fileName, _objectTypeResolver.TypeFor<T>(), xelRoot.Name.LocalName);
      }

      private IEnumerable<XElement> retrieveElementsToDeserializeFromRootNode(XElement xelRoot, string expectedElementName)
      {
         var possibleElementsToDeserialize = xelRoot.DescendantsAndSelf(expectedElementName).ToList();
         if (possibleElementsToDeserialize.Any())
            return possibleElementsToDeserialize;

         //no element found. Check to see if we can match elements using a previously saved name or a build configuration
         return xelRoot.DescendantsAndSelf(getElementNameFromBuildConfiguration(expectedElementName));
      }

  
      private bool loadingSimulation<T>()
      {
         return typeof (T).IsAnImplementationOf<IModelCoreSimulation>() ||
                typeof (T).IsAnImplementationOf<SimulationTransfer>();
      }

      private XElement removeModelFrom(XElement xelRoot)
      {
         var models = xelRoot.DescendantsAndSelf(AppConstants.XmlNames.Model).ToList();
         models.Each(model => model.Remove());
         return xelRoot;
      }

      private string getElementNameFromBuildConfiguration(string expectedElementName)
      {
         if (expectedElementName.Equals(AppConstants.XmlNames.MoleculeBuildingBlock)) return AppConstants.XmlNames.Molecules;
         if (expectedElementName.Equals(AppConstants.XmlNames.ObserverBuildingBlock)) return AppConstants.XmlNames.Observers;
         if (expectedElementName.Equals(AppConstants.XmlNames.EventGroupBuildingBlock)) return AppConstants.XmlNames.EventGroups;
         if (expectedElementName.Equals(AppConstants.XmlNames.MoleculeStartValuesBuildingBlock)) return AppConstants.XmlNames.MoleculeStartValues;
         if (expectedElementName.Equals(AppConstants.XmlNames.ParameterStartValuesBuildingBlock)) return AppConstants.XmlNames.ParameterStartValues;
         if (expectedElementName.Equals(AppConstants.XmlNames.MoBiReactionBuildingBlock)) return AppConstants.XmlNames.Reactions;
         if (expectedElementName.Equals(AppConstants.XmlNames.ReactionBuildingBlock)) return AppConstants.XmlNames.Reactions;
         if (expectedElementName.Equals(AppConstants.XmlNames.MoBiSpatialStructure)) return AppConstants.XmlNames.SpatialStructure;
         if (expectedElementName.Equals(AppConstants.XmlNames.PassiveTransportBuildingBlock)) return AppConstants.XmlNames.PassiveTransports;
         if (expectedElementName.Equals(AppConstants.XmlNames.ModelCoreSimulation)) return AppConstants.XmlNames.MoBiSimulation;

         //this line is required if the file we are loading contains an old ModelCoreSimulation file
         if (expectedElementName.Equals(AppConstants.XmlNames.MoBiSimulation)) return AppConstants.XmlNames.ModelCoreSimulation;
         if (expectedElementName.Equals(AppConstants.XmlNames.CurveChart)) return AppConstants.XmlNames.Chart;

         return expectedElementName;
      }

      private IReadOnlyList<XElement> selectToDeserialize(IEnumerable<XElement> possibleElements, string searchedEntityType)
      {
         return _xmlContentSelector.SelectFrom(possibleElements, searchedEntityType).ToList();
      }
   }
}