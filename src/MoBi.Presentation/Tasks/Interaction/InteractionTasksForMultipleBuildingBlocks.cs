using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
         private readonly IInteractionTasksForChildren<Module, EventGroupBuildingBlock> _interactionTasksForApplicationMoleculeBuilder;
         private readonly IInteractionTasksForChildren<Module, InitialConditionsBuildingBlock> _interactionTasksForInitialCondition;
         private readonly IInteractionTasksForChildren<Module, MoleculeBuildingBlock> _interactionTasksForMolecule;
         private readonly IInteractionTasksForChildren<Module, PassiveTransportBuildingBlock> _interactionTasksForPassiveTransport;
         private readonly IInteractionTasksForChildren<Module, ParameterValuesBuildingBlock> _interactionTasksForParameterValues;
         private readonly IInteractionTasksForChildren<Module, MoBiReactionBuildingBlock> _interactionTasksForMobiReaction;
         private readonly IInteractionTasksForChildren<Module, MoBiSpatialStructure> _interactionTasksForMobiSpatialStructure;
         private readonly IInteractionTasksForChildren<Module, ObserverBuildingBlock> _interactionTasksForObserver;
         private readonly IInteractionTaskContext _interactionTaskContext;

         public InteractionTasksForMultipleBuildingBlocks(
            IInteractionTasksForChildren<Module, EventGroupBuildingBlock> interactionTasksForApplicationMoleculeBuilder,
            IInteractionTasksForChildren<Module, InitialConditionsBuildingBlock> interactionTasksForInitialCondition,
            IInteractionTasksForChildren<Module, MoleculeBuildingBlock> interactionTasksForMolecule,
            IInteractionTasksForChildren<Module, PassiveTransportBuildingBlock> interactionTasksForPassiveTransport,
            IInteractionTasksForChildren<Module, ParameterValuesBuildingBlock> interactionTasksForParameterValues,
            IInteractionTasksForChildren<Module, MoBiReactionBuildingBlock> interactionTasksForMobiReaction,
            IInteractionTasksForChildren<Module, MoBiSpatialStructure> interactionTasksForMobiSpatialStructure,
            IInteractionTasksForChildren<Module, ObserverBuildingBlock> interactionTasksForObserver,
            IInteractionTaskContext interactionTaskContext)
         {
            _interactionTasksForApplicationMoleculeBuilder = interactionTasksForApplicationMoleculeBuilder;
            _interactionTasksForInitialCondition = interactionTasksForInitialCondition;
            _interactionTasksForMolecule = interactionTasksForMolecule;
            _interactionTasksForPassiveTransport = interactionTasksForPassiveTransport;
            _interactionTasksForParameterValues = interactionTasksForParameterValues;
            _interactionTasksForMobiReaction = interactionTasksForMobiReaction;
            _interactionTasksForMobiSpatialStructure = interactionTasksForMobiSpatialStructure;
            _interactionTasksForObserver = interactionTasksForObserver;
            _interactionTaskContext = interactionTaskContext;
         }

         public void RemoveBuildingBlocks(IReadOnlyList<IBuildingBlock> buildingBlocksToRemove)
         {
            if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveMultipleBuildingBlocks) != ViewResult.Yes)
               return;

            var referringSimulationsAndBuildingBlocks = new List<Tuple<IReadOnlyList<IMoBiSimulation>, IBuildingBlock>>();

            foreach (var buildingBlockToRemove in buildingBlocksToRemove)
            {
               var referringSimulations = _interactionTaskContext.Context.CurrentProject.SimulationsUsing(buildingBlockToRemove);

               if (referringSimulations.Any())
                  referringSimulationsAndBuildingBlocks.Add(new Tuple<IReadOnlyList<IMoBiSimulation>, IBuildingBlock>(referringSimulations, buildingBlockToRemove));
               else
               {
                  removeBuildingBlock(buildingBlockToRemove);
               }
            }

            if (referringSimulationsAndBuildingBlocks.Any())
            {
               var messageBuilder = new StringBuilder();

               messageBuilder.AppendLine(AppConstants.Dialog.BuildingBlocksUsedInSimulation);
               messageBuilder.AppendLine();

               foreach (var item in referringSimulationsAndBuildingBlocks)
               {
                  var buildingBlock = item.Item2;
                  var simulations = item.Item1;

                  messageBuilder.AppendLine($"\t- {AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlock.Name, simulations.AllNames())}");
               }

               _interactionTaskContext.DialogCreator.MessageBoxInfo(messageBuilder.ToString());
            }
         }

         private void removeBuildingBlock(IBuildingBlock buildingBlockToRemove)
         {
            switch (buildingBlockToRemove)
            {
               case EventGroupBuildingBlock eventGroupBuildingBlock:
                  _interactionTasksForApplicationMoleculeBuilder.Remove(eventGroupBuildingBlock, eventGroupBuildingBlock.Module, eventGroupBuildingBlock, true);
                  break;

               case InitialConditionsBuildingBlock initialConditionsBuildingBlock:
                  _interactionTasksForInitialCondition.Remove(initialConditionsBuildingBlock, initialConditionsBuildingBlock.Module, initialConditionsBuildingBlock, true);
                  break;

               case MoleculeBuildingBlock moleculeBuildingBlock:
                  _interactionTasksForMolecule.Remove(moleculeBuildingBlock, moleculeBuildingBlock.Module, moleculeBuildingBlock, true);
                  break;

               case PassiveTransportBuildingBlock passiveTransportBuildingBlock:
                  _interactionTasksForPassiveTransport.Remove(passiveTransportBuildingBlock, passiveTransportBuildingBlock.Module, passiveTransportBuildingBlock, true);
                  break;

               case ParameterValuesBuildingBlock parameterValuesBuildingBlock:
                  _interactionTasksForParameterValues.Remove(parameterValuesBuildingBlock, parameterValuesBuildingBlock.Module, parameterValuesBuildingBlock, true);
                  break;

               case MoBiReactionBuildingBlock mobiReactionBuildingBlock:
                  _interactionTasksForMobiReaction.Remove(mobiReactionBuildingBlock, mobiReactionBuildingBlock.Module, mobiReactionBuildingBlock, true);
                  break;

               case MoBiSpatialStructure mobiSpatialStructure:
                  _interactionTasksForMobiSpatialStructure.Remove(mobiSpatialStructure, mobiSpatialStructure.Module, mobiSpatialStructure, true);
                  break;

               case ObserverBuildingBlock observerBuildingBlock:
                  _interactionTasksForObserver.Remove(observerBuildingBlock, observerBuildingBlock.Module, observerBuildingBlock, true);
                  break;

               default:
                  throw new InvalidOperationException($"No interaction task found for building block type: {buildingBlockToRemove.GetType()}");
            }
         }
      }
   }
}