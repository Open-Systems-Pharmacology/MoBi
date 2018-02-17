using System;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Visitor;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Journal;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation.Tasks
{
   public class ReloadRelatedItemTask : OSPSuite.Core.Journal.ReloadRelatedItemTask,
      IVisitor<IBuildingBlock>,
      IVisitor<DataRepository>,
      IVisitor<IMoBiSimulation>,
      IVisitor<ParameterIdentification>,
      IVisitor<SensitivityAnalysis>
   {
      private readonly IRelatedItemSerializer _relatedItemSerializer;
      private readonly IMoBiContext _context;
      private readonly IBuildingBlockTaskRetriever _taskRetriever;
      private readonly IPKSimExportTask _pkSimExportTask;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private readonly ISimulationLoader _simulationLoader;
      private readonly IObservedDataTask _observedDataTask;
      private readonly IObjectIdResetter _objectIdResetter;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;

      public ReloadRelatedItemTask(
         IApplicationConfiguration applicationConfiguration, 
         IContentLoader contentLoader,
         IDialogCreator dialogCreator, 
         IRelatedItemSerializer relatedItemSerializer,
         IMoBiContext context, 
         ICloneManagerForBuildingBlock cloneManagerForBuildingBlock,
         IBuildingBlockTaskRetriever taskRetriever, 
         IPKSimExportTask pkSimExportTask, 
         ISimulationLoader simulationLoader,
         IObservedDataTask observedDataTask, 
         IObjectIdResetter objectIdResetter, 
         IParameterIdentificationTask parameterIdentificationTask, 
         ISensitivityAnalysisTask sensitivityAnalysisTask
         ) : base(applicationConfiguration, contentLoader, dialogCreator)
      {
         _relatedItemSerializer = relatedItemSerializer;
         _context = context;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _taskRetriever = taskRetriever;
         _pkSimExportTask = pkSimExportTask;
         _simulationLoader = simulationLoader;
         _observedDataTask = observedDataTask;
         _objectIdResetter = objectIdResetter;
         _parameterIdentificationTask = parameterIdentificationTask;
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
      }

      protected override void StartSisterApplicationWithContentFile(string contentFile)
      {
         _pkSimExportTask.StartWithContentFile(contentFile);
      }

      protected override void LoadOwnContent(RelatedItem relatedItem)
      {
         try
         {
            var relatedItemObject = _relatedItemSerializer.Deserialize(relatedItem);
            this.Visit(relatedItemObject);
         }
         catch (NotUniqueIdException)
         {
            //Probably trying to load an object that was already loaded. Show a message to the user
            throw new OSPSuiteException(AppConstants.Exceptions.CannotLoadRelatedItemAsObjectAlreadyExistInProject(relatedItem.ItemType, relatedItem.Name));
         }
      }

      protected override bool RelatedItemCanBeLaunchedBySisterApplication(RelatedItem relatedItem)
      {
         return relatedItem.Origin == Origins.PKSim;
      }

      public void Visit(IBuildingBlock buildingBlock)
      {
         var task = _taskRetriever.TaskFor(buildingBlock);
         // Clone to get new ID's
         var clone = _cloneManagerForBuildingBlock.CloneBuildingBlock(buildingBlock);
         addCommand(task.AddToProject(clone));
      }

      public void Visit(IMoBiSimulation simulation)
      {
         var command = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectTypes.Simulation,
            Description = AppConstants.Commands.AddToProjectDescription(ObjectTypes.Simulation, simulation.Name)
         };
         command.Add(_simulationLoader.AddSimulationToProject(simulation));
         addCommand(command);
      }

      private void addCommand(IMoBiCommand command)
      {
         _context.AddToHistory(command);
      }

      public void Visit(DataRepository observedData)
      {
         _observedDataTask.AddObservedDataToProject(observedData);
      }

      public void Visit(ParameterIdentification parameterIdentification)
      {
         _objectIdResetter.ResetIdFor(parameterIdentification);
         _parameterIdentificationTask.AddToProject(parameterIdentification);
      }

      public void Visit(SensitivityAnalysis sensitivityAnalysis)
      {
         _objectIdResetter.ResetIdFor(sensitivityAnalysis);
         _sensitivityAnalysisTask.AddToProject(sensitivityAnalysis);
      }
   }
}