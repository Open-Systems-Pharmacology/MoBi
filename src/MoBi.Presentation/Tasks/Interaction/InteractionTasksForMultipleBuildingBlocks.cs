using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   namespace MoBi.Presentation.Tasks.Interaction
   {
      public interface IInteractionTasksForMultipleBuildingBlocks
      {
         void RemoveBuildingBlocks(IReadOnlyList<IBuildingBlock> buildingBlocks);
      }

      public class InteractionTasksForMultipleBuildingBlocks : IInteractionTasksForMultipleBuildingBlocks
      {
         private readonly IInteractionTasksForEventBuildingBlock _interactionTasksForEventGroupBuildingBlock;
         private readonly IInitialConditionsTask<InitialConditionsBuildingBlock> _interactionTasksForInitialConditionBuildingBlock;
         private readonly IParameterValuesTask _interactionTasksForParameterValues;
         private readonly IInteractionTasksForMoleculeBuildingBlock _interactionTasksForMoleculeBuildingBlock;
         private readonly IInteractionTasksForPassiveTransportBuildingBlock _interactionTasksForPassiveTransportBuildingBlock;
         private readonly IInteractionTasksForReactionBuildingBlock _interactionTasksForMobiReactionBuildingBlock;
         private readonly IInteractionTasksForSpatialStructure _interactionTasksForMobiSpatialStructureBuildingBlock;
         private readonly IInteractionTasksForObserverBuildingBlock _interactionTasksForObserverBuildingBlock;
         private readonly IInteractionTasksForIndividualBuildingBlock _interactionTasksForIndividualBuildingBlock;
         private readonly IInteractionTasksForExpressionProfileBuildingBlock _interactionTasksForExpressionProfileBuildingBlock;
         private readonly IInteractionTaskContext _interactionTaskContext;

         public InteractionTasksForMultipleBuildingBlocks(
            IInteractionTasksForEventBuildingBlock interactionTasksForEventGroupBuildingBlock,
            IInitialConditionsTask<InitialConditionsBuildingBlock> interactionTasksForInitialConditionBuildingBlock,
            IInteractionTasksForMoleculeBuildingBlock interactionTasksForMoleculeBuildingBlock,
            IInteractionTasksForPassiveTransportBuildingBlock interactionTasksForPassiveTransportBuildingBlock,
            IParameterValuesTask interactionTasksForParameterValues,
            IInteractionTasksForReactionBuildingBlock interactionTasksForMobiReactionBuildingBlock,
            IInteractionTasksForSpatialStructure interactionTasksForMobiSpatialStructureBuildingBlock,
            IInteractionTasksForObserverBuildingBlock interactionTasksForObserverBuildingBlock,
            IInteractionTasksForIndividualBuildingBlock interactionTasksForIndividualBuildingBlock,
            IInteractionTasksForExpressionProfileBuildingBlock interactionTasksForExpressionProfileBuildingBlock,
            IInteractionTaskContext interactionTaskContext)
         {
            _interactionTasksForEventGroupBuildingBlock = interactionTasksForEventGroupBuildingBlock;
            _interactionTasksForInitialConditionBuildingBlock = interactionTasksForInitialConditionBuildingBlock;
            _interactionTasksForMoleculeBuildingBlock = interactionTasksForMoleculeBuildingBlock;
            _interactionTasksForPassiveTransportBuildingBlock = interactionTasksForPassiveTransportBuildingBlock;
            _interactionTasksForParameterValues = interactionTasksForParameterValues;
            _interactionTasksForMobiReactionBuildingBlock = interactionTasksForMobiReactionBuildingBlock;
            _interactionTasksForMobiSpatialStructureBuildingBlock = interactionTasksForMobiSpatialStructureBuildingBlock;
            _interactionTasksForObserverBuildingBlock = interactionTasksForObserverBuildingBlock;
            _interactionTasksForIndividualBuildingBlock = interactionTasksForIndividualBuildingBlock;
            _interactionTasksForExpressionProfileBuildingBlock = interactionTasksForExpressionProfileBuildingBlock;
            _interactionTaskContext = interactionTaskContext;
         }

         public void RemoveBuildingBlocks(IReadOnlyList<IBuildingBlock> buildingBlocksToRemove)
         {
            if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveMultipleBuildingBlocks) != ViewResult.Yes)
               return;

            var referringSimulationsAndBuildingBlocks = new List<Tuple<IReadOnlyList<string>, string>>();
            var allCommands = new List<IMoBiCommand>();

            foreach (var buildingBlockToRemove in buildingBlocksToRemove)
            {
               var referringSimulations = _interactionTaskContext.Context.CurrentProject.SimulationsUsing(buildingBlockToRemove);

               if (referringSimulations.Any())
                  referringSimulationsAndBuildingBlocks.Add(new Tuple<IReadOnlyList<string>, string>(referringSimulations.AllNames(), buildingBlockToRemove.Name));
               else
               {
                  allCommands.Add(removeBuildingBlock(buildingBlockToRemove));
               }
            }

            if (allCommands.Any())
               executeMacroCommand(allCommands);

            if (referringSimulationsAndBuildingBlocks.Any())
            {
               var messageBuilder = AppConstants.ListOfBuildingBlocksNotRemoved(referringSimulationsAndBuildingBlocks);
               _interactionTaskContext.DialogCreator.MessageBoxInfo(messageBuilder.ToString());
            }
         }

         private void executeMacroCommand(List<IMoBiCommand> allCommands)
         {
            var macroCommand = new MoBiMacroCommand()
            {
               CommandType = AppConstants.Commands.DeleteCommand,
               ObjectType = ObjectTypes.BuildingBlock,
               Description = AppConstants.Commands.RemoveMultipleBuildingBlocks
            };
            allCommands.Each(x => x.Visible = false);
            macroCommand.AddRange(allCommands);
            macroCommand.Execute(_interactionTaskContext.Context);
            _interactionTaskContext.Context.AddToHistory(macroCommand);
         }

         private IMoBiCommand removeBuildingBlock(IBuildingBlock buildingBlockToRemove)
         {
            switch (buildingBlockToRemove)
            {
               case EventGroupBuildingBlock eventGroupBuildingBlock:
                  return _interactionTasksForEventGroupBuildingBlock.GetRemoveCommand(eventGroupBuildingBlock, eventGroupBuildingBlock.Module, eventGroupBuildingBlock);

               case InitialConditionsBuildingBlock initialConditionsBuildingBlock:
                  return _interactionTasksForInitialConditionBuildingBlock.GetRemoveCommand(initialConditionsBuildingBlock, initialConditionsBuildingBlock.Module, initialConditionsBuildingBlock);

               case MoleculeBuildingBlock moleculeBuildingBlock:
                  return _interactionTasksForMoleculeBuildingBlock.GetRemoveCommand(moleculeBuildingBlock, moleculeBuildingBlock.Module, moleculeBuildingBlock);

               case PassiveTransportBuildingBlock passiveTransportBuildingBlock:
                  return _interactionTasksForPassiveTransportBuildingBlock.GetRemoveCommand(passiveTransportBuildingBlock, passiveTransportBuildingBlock.Module, passiveTransportBuildingBlock);

               case ParameterValuesBuildingBlock parameterValuesBuildingBlock:
                  return _interactionTasksForParameterValues.GetRemoveCommand(parameterValuesBuildingBlock, parameterValuesBuildingBlock.Module, parameterValuesBuildingBlock);

               case MoBiReactionBuildingBlock mobiReactionBuildingBlock:
                  return _interactionTasksForMobiReactionBuildingBlock.GetRemoveCommand(mobiReactionBuildingBlock, mobiReactionBuildingBlock.Module, mobiReactionBuildingBlock);

               case MoBiSpatialStructure mobiSpatialStructure:
                  return _interactionTasksForMobiSpatialStructureBuildingBlock.GetRemoveCommand(mobiSpatialStructure, mobiSpatialStructure.Module, mobiSpatialStructure);

               case ObserverBuildingBlock observerBuildingBlock:
                  return _interactionTasksForObserverBuildingBlock.GetRemoveCommand(observerBuildingBlock, observerBuildingBlock.Module, observerBuildingBlock);

               case IndividualBuildingBlock individualBuildingBlock:
                  return _interactionTasksForIndividualBuildingBlock.GetRemoveCommand(individualBuildingBlock, null, individualBuildingBlock);

               case ExpressionProfileBuildingBlock expressionProfileBuildingBlock:
                  return _interactionTasksForExpressionProfileBuildingBlock.GetRemoveCommand(expressionProfileBuildingBlock, null, expressionProfileBuildingBlock);

               default:
                  throw new InvalidOperationException($"No interaction task found for building block type: {buildingBlockToRemove.GetType()}");
            }
         }
      }
   }
}