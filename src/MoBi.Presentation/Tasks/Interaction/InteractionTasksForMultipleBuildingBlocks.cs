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
         private readonly IInteractionTasksForChildren<Module, EventGroupBuildingBlock> _interactionTasksForEventGroupBuildingBlock;
         private readonly IInteractionTasksForChildren<Module, InitialConditionsBuildingBlock> _interactionTasksForInitialConditionBuildingBlock;
         private readonly IInteractionTasksForChildren<Module, MoleculeBuildingBlock> _interactionTasksForMoleculeBuildingBlock;
         private readonly IInteractionTasksForChildren<Module, PassiveTransportBuildingBlock> _interactionTasksForPassiveTransportBuildingBlock;
         private readonly IInteractionTasksForChildren<Module, ParameterValuesBuildingBlock> _interactionTasksForParameterValues;
         private readonly IInteractionTasksForChildren<Module, MoBiReactionBuildingBlock> _interactionTasksForMobiReactionBuildingBlock;
         private readonly IInteractionTasksForChildren<Module, MoBiSpatialStructure> _interactionTasksForMobiSpatialStructureBuildingBlock;
         private readonly IInteractionTasksForChildren<Module, ObserverBuildingBlock> _interactionTasksForObserverBuildingBlock;
         private readonly IInteractionTaskContext _interactionTaskContext;

         public InteractionTasksForMultipleBuildingBlocks(
            IInteractionTasksForChildren<Module, EventGroupBuildingBlock> interactionTasksForEventGroupBuildingBlock,
            IInteractionTasksForChildren<Module, InitialConditionsBuildingBlock> interactionTasksForInitialConditionBuildingBlock,
            IInteractionTasksForChildren<Module, MoleculeBuildingBlock> interactionTasksForMoleculeBuildingBlock,
            IInteractionTasksForChildren<Module, PassiveTransportBuildingBlock> interactionTasksForPassiveTransportBuildingBlock,
            IInteractionTasksForChildren<Module, ParameterValuesBuildingBlock> interactionTasksForParameterValues,
            IInteractionTasksForChildren<Module, MoBiReactionBuildingBlock> interactionTasksForMobiReactionBuildingBlock,
            IInteractionTasksForChildren<Module, MoBiSpatialStructure> interactionTasksForMobiSpatialStructureBuildingBlock,
            IInteractionTasksForChildren<Module, ObserverBuildingBlock> interactionTasksForObserverBuildingBlock,
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

               default:
                  throw new InvalidOperationException($"No interaction task found for building block type: {buildingBlockToRemove.GetType()}");
            }
         }
      }
   }
}