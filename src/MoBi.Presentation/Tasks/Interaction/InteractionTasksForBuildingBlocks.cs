using System;
using System.Collections.Generic;
using MoBi.Core.Commands;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForBuildingBlocks
   {
      void LoadBuildingBlocksToModule(Module module);
   }

   public class InteractionTasksForBuildingBlocks : InteractionTasksForChildren<Module, IBuildingBlock>, IInteractionTasksForBuildingBlocks
   {
      public InteractionTasksForBuildingBlocks(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<IBuildingBlock> editTask) :
         base(
            interactionTaskContext,
            editTask)
      {
      }

      public IMoBiCommand GetAddBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks)
      {
         return new AddMultipleBuildingBlocksToModuleCommand(existingModule, listOfNewBuildingBlocks);
      }

      public void LoadBuildingBlocksToModule(Module module)
      {
         using (var presenter = _interactionTaskContext.ApplicationController.Start<ISelectBuildingBlockTypePresenter>())
         {
            //probably returning a list makes more sense
            var buildingBlockType = presenter.GetBuildingBlockType(module);

            if (buildingBlockType == BuildingBlockType.None)
               return;

            var filename = AskForPKMLFileToOpen();

            if (filename.IsNullOrEmpty())
               return;

            var items = loadBuildingBlock(buildingBlockType, filename);

            if (items.Count < 1)
               return;

            _interactionTaskContext.Context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, items)
               .Run(_interactionTaskContext.Context));
         }

         //var items = LoadItems(filename);
         // one solution would be to pass the interactionTask by interface directly to the presenter
         //and just let the mapper directly do the work for us
         //actually the mapper I think could only need _interactionTaskContext.InteractionTask
         //so if it resolves just the _interactionTaskContext from the IoC, it should not need anything more.
         //var items = InteractionTask.LoadItems<ObserverBuildingBlock>(filename).ToList();
         //then: pass the interaction task + restraints for the drop down
         //check if there can be more than one item in the file. actually there should not
         //except of course of the StartValues...so we can check or not
      }

      private List<IBuildingBlock> loadBuildingBlock(BuildingBlockType buildingBlockType, string filename)
      {
         var items = new List<IBuildingBlock>();

         switch (buildingBlockType)
         {
            case BuildingBlockType.Reaction:
               items.AddRange(InteractionTask.LoadItems<ReactionBuildingBlock>(filename));
               break;
            case BuildingBlockType.EventGroup:
               items.AddRange(InteractionTask.LoadItems<EventGroupBuildingBlock>(filename));
               break;
            case BuildingBlockType.SpatialStructure:
               items.AddRange(InteractionTask.LoadItems<SpatialStructure>(filename));
               break;
            case BuildingBlockType.PassiveTransport:
               items.AddRange(InteractionTask.LoadItems<PassiveTransportBuildingBlock>(filename));
               break;
            case BuildingBlockType.Molecule:
               items.AddRange(InteractionTask.LoadItems<MoleculeBuildingBlock>(filename));
               break;
            case BuildingBlockType.Observer:
               items.AddRange(InteractionTask.LoadItems<ObserverBuildingBlock>(filename));
               break;
            case BuildingBlockType.MoleculeStartValues:
               items.AddRange(InteractionTask.LoadItems<MoleculeStartValuesBuildingBlock>(filename));
               break;
            case BuildingBlockType.ParameterStartValues:
               items.AddRange(InteractionTask.LoadItems<ParameterStartValuesBuildingBlock>(filename));
               break;
         }

         return items;
      }

      //not sure what to do with those here underneath

      public override IMoBiCommand GetRemoveCommand(IBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         throw new NotImplementedException();
      }

      public override IMoBiCommand GetAddCommand(IBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         throw new NotImplementedException();
      }
   }
}