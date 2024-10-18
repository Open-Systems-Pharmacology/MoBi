using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

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

            foreach (var buildingBlockToRemove in buildingBlocksToRemove)
            {
               var referringSimulations = _interactionTaskContext.Context.CurrentProject.SimulationsUsing(buildingBlockToRemove);

               if (referringSimulations.Any())
                  referringSimulationsAndBuildingBlocks.Add(new Tuple<IReadOnlyList<string>, string>(referringSimulations.AllNames(), buildingBlockToRemove.Name));
               else
               {
                  removeBuildingBlock(buildingBlockToRemove);
               }
            }

            if (referringSimulationsAndBuildingBlocks.Any())
            {
               var messageBuilder = AppConstants.ListOfBuildingBlocksNotRemoved(referringSimulationsAndBuildingBlocks);
               _interactionTaskContext.DialogCreator.MessageBoxInfo(messageBuilder.ToString());
            }
         }

         private void removeBuildingBlock(IBuildingBlock buildingBlockToRemove)
         {
            switch (buildingBlockToRemove)
            {
               case EventGroupBuildingBlock eventGroupBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForEventGroupBuildingBlock.Remove(eventGroupBuildingBlock, eventGroupBuildingBlock.Module, eventGroupBuildingBlock, silent: true));
                  break;

               case InitialConditionsBuildingBlock initialConditionsBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForInitialConditionBuildingBlock.Remove(initialConditionsBuildingBlock, initialConditionsBuildingBlock.Module, initialConditionsBuildingBlock, silent: true));
                  break;

               case MoleculeBuildingBlock moleculeBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForMoleculeBuildingBlock.Remove(moleculeBuildingBlock, moleculeBuildingBlock.Module, moleculeBuildingBlock, silent: true));
                  break;

               case PassiveTransportBuildingBlock passiveTransportBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForPassiveTransportBuildingBlock.Remove(passiveTransportBuildingBlock, passiveTransportBuildingBlock.Module, passiveTransportBuildingBlock, silent: true));
                  break;

               case ParameterValuesBuildingBlock parameterValuesBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForParameterValues.Remove(parameterValuesBuildingBlock, parameterValuesBuildingBlock.Module, parameterValuesBuildingBlock, silent: true));
                  break;

               case MoBiReactionBuildingBlock mobiReactionBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForMobiReactionBuildingBlock.Remove(mobiReactionBuildingBlock, mobiReactionBuildingBlock.Module, mobiReactionBuildingBlock, silent: true));
                  break;

               case MoBiSpatialStructure mobiSpatialStructure:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForMobiSpatialStructureBuildingBlock.Remove(mobiSpatialStructure, mobiSpatialStructure.Module, mobiSpatialStructure, silent: true));
                  break;

               case ObserverBuildingBlock observerBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForObserverBuildingBlock.Remove(observerBuildingBlock, observerBuildingBlock.Module, observerBuildingBlock, silent: true));
                  break;

               case IndividualBuildingBlock individualBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForIndividualBuildingBlock.Remove(individualBuildingBlock, null, individualBuildingBlock, silent: true));
                  break;

               case ExpressionProfileBuildingBlock expressionProfileBuildingBlock:
                  _interactionTaskContext.Context.AddToHistory(_interactionTasksForExpressionProfileBuildingBlock.Remove(expressionProfileBuildingBlock, null, expressionProfileBuildingBlock, silent: true));
                  break;

               default:
                  throw new InvalidOperationException($"No interaction task found for building block type: {buildingBlockToRemove.GetType()}");
            }
         }
      }
   }
}