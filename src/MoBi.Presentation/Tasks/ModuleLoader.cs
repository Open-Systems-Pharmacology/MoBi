using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
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

      public ModuleLoader(IInteractionTasksForModule moduleTask, IInteractionTasksForExpressionProfileBuildingBlock expressionTask, IInteractionTasksForIndividualBuildingBlock individualTask, IInteractionTaskContext interactionTaskContext)
      {
         _moduleTask = moduleTask;
         _expressionTask = expressionTask;
         _individualTask = individualTask;
         _interactionTaskContext = interactionTaskContext;
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
         var configuration = _interactionTaskContext.InteractionTask.LoadSimulationTransfer(filename).Simulation.Configuration;

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = new ObjectTypeResolver().TypeFor<BuildingBlock>(),
            Description = AppConstants.Commands.AddedMultipleBuildingBlocksFromFile
         };

         var modules = configuration.ModuleConfigurations.Select(x => x.Module).ToList();
         var individuals = configuration.Individual == null ? new List<IndividualBuildingBlock>() : new List<IndividualBuildingBlock> { configuration.Individual };
         var expressions = configuration.ExpressionProfiles;

         macroCommand.Add(_moduleTask.AddTo(modules, project));

         if (!addAdditionalBuildingBlocksConfirmed(individuals.Count, expressions.Count))
            return macroCommand;

         macroCommand.Add(_individualTask.AddTo(individuals, project));
         macroCommand.Add(_expressionTask.AddTo(expressions, project));

         return macroCommand;
      }

      private bool addAdditionalBuildingBlocksConfirmed(int numberOfIndividuals, int numberOfExpressions)
      {
         // Do not prompt if there are no additional building blocks to add
         if (numberOfIndividuals == 0 && numberOfExpressions == 0)
            return false;

         return ViewResult.Yes == _interactionTaskContext.DialogCreator.MessageBoxYesNo(AppConstants.Captions.AlsoImportIndividualsAndExpressions(numberOfIndividuals, numberOfExpressions));
      }
   }
}