using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public interface IModuleLoader
   {
      IMoBiCommand LoadModule(MoBiProject project);
   }

   public class ModuleLoader : IModuleLoader
   {
      private readonly IInteractionTasksForModule _moduleTask;
      private readonly IInteractionTasksForExpressionProfileBuildingBlock _expressionTask;
      private readonly IInteractionTasksForIndividualBuildingBlock _individualTask;
      private readonly IInteractionTaskContext _interactionTaskContext;
      private readonly ICloneManagerForSimulation _cloneManager;

      public ModuleLoader(IInteractionTasksForModule moduleTask, 
         IInteractionTasksForExpressionProfileBuildingBlock expressionTask, 
         IInteractionTasksForIndividualBuildingBlock individualTask, 
         IInteractionTaskContext interactionTaskContext,
         ICloneManagerForSimulation cloneManager)
      {
         _moduleTask = moduleTask;
         _expressionTask = expressionTask;
         _individualTask = individualTask;
         _interactionTaskContext = interactionTaskContext;
         _cloneManager = cloneManager;
      }

      public IMoBiCommand LoadModule(MoBiProject project)
      {
         var filename = _moduleTask.AskForPKMLFileToOpen();

         if (filename.IsNullOrEmpty())
            return new MoBiEmptyCommand();

         try
         {
            // If the user is adding modules from a simulation transfer, optionally add other building blocks
            return addFromSimulationTransfer(project, filename);
         }
         catch
         {
            // If the user is not loading from a simulation transfer, only add modules
            return _moduleTask.AddFromFileTo(filename, project);
         }
      }

      private IMoBiCommand addFromSimulationTransfer(MoBiProject project, string filename)
      {
         var simulation = _interactionTaskContext.InteractionTask.LoadTransfer<SimulationTransfer>(filename).Simulation;
         // Clone the simulation configuration so that any loaded objects will have unique Ids.
         var configuration = _cloneManager.CloneSimulationConfiguration(simulation.Configuration);

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = new ObjectTypeResolver().TypeFor<BuildingBlock>(),
            Description = AppConstants.Commands.AddedMultipleBuildingBlocksFromFile
         };

         var modules = configuration.ModuleConfigurations.Select(x => x.Module).ToList();
         var individual = configuration.Individual;
         var expressions = configuration.ExpressionProfiles;

         var commandToAdd = _moduleTask.AddTo(modules, project);
         macroCommand.Add(commandToAdd);

         if (commandToAdd.IsEmpty() || !addAdditionalBuildingBlocksConfirmed(individual, expressions))
            return macroCommand;

         macroCommand.Add(_individualTask.AddTo(new[] { individual }, project));
         macroCommand.Add(_expressionTask.AddTo(expressions, project));

         return macroCommand;
      }

      private bool addAdditionalBuildingBlocksConfirmed(IndividualBuildingBlock individual, IReadOnlyList<ExpressionProfileBuildingBlock> expressions)
      {
         // Do not prompt if there are no additional building blocks to add
         if (individual == null && expressions.Count == 0)
            return false;

         return ViewResult.Yes == _interactionTaskContext.DialogCreator.MessageBoxYesNo(AppConstants.Captions.AlsoImportIndividualsAndExpressions(individual?.Name, expressions.AllNames()));
      }
   }
}