using System.Collections.Generic;
using Antlr.Runtime.Misc;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Exceptions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForModuleBuildingBlocks
   {
      void LoadBuildingBlocksToModule(Module module);
      void LoadBuildingBlocksFromTemplateToModule(Module module);
      void RemoveModule(Module module);
   }

   public class InteractionTasksForModuleBuildingBlocks : InteractionTasksForChildren<Module, IBuildingBlock>,
      IInteractionTasksForModuleBuildingBlocks
   {
      public InteractionTasksForModuleBuildingBlocks(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<IBuildingBlock> editTask) :
         base(interactionTaskContext, editTask)
      {
      }

      public IMoBiCommand GetAddBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks)
      {
         return new AddMultipleBuildingBlocksToModuleCommand(existingModule, listOfNewBuildingBlocks);
      }

      public void LoadBuildingBlocksToModule(Module module)
      {
         loadBuildingBlocksToModuleBase(module, AskForPKMLFileToOpen);
      }
      public void LoadBuildingBlocksFromTemplateToModule(Module module)
      {
         loadBuildingBlocksToModuleBase(module, openTemplateFile);
      }

      public void RemoveModule(Module module)
      {
         _interactionTaskContext.Context.AddToHistory(new RemoveModuleCommand(module)
            .Run(_interactionTaskContext.Context));
      }

      private void loadBuildingBlocksToModuleBase(Module module, Func<string> getFilename)
      {
         using (var presenter = _interactionTaskContext.ApplicationController.Start<ISelectBuildingBlockTypePresenter>())
         {
            var buildingBlockType = presenter.GetBuildingBlockType(module);

            if (buildingBlockType == BuildingBlockType.None)
               return;

            var filename = getFilename();

            if (filename.IsNullOrEmpty())
               return;

            var items = loadBuildingBlock(buildingBlockType, filename);

            if (items.Count < 1)
               return;

            _interactionTaskContext.Context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, items)
               .Run(_interactionTaskContext.Context));
         }
      }

      private string openTemplateFile()
      {
         _interactionTaskContext.UpdateTemplateDirectories();
         return InteractionTask.AskForFileToOpen(AppConstants.Dialog.LoadFromTemplate(_editTask.ObjectName),
            Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.TEMPLATE);
      }
      public override IMoBiCommand GetRemoveCommand(IBuildingBlock objectToRemove, Module module, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<IBuildingBlock>(objectToRemove, module);
      }

      public override IMoBiCommand GetAddCommand(IBuildingBlock itemToAdd, Module module, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<IBuildingBlock>(itemToAdd, module);
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
            //for the cases underneath, we could have multiple buildingBlocks being loaded
            case BuildingBlockType.MoleculeStartValues:
               items.AddRange(InteractionTask.LoadItems<MoleculeStartValuesBuildingBlock>(filename));
               return items;
            case BuildingBlockType.ParameterStartValues:
               items.AddRange(InteractionTask.LoadItems<ParameterStartValuesBuildingBlock>(filename));
               return items;
         }

         //if there are multiple BBs where only one is allowed, abort
         if (items.Count > 1)
            throw new MoBiException(AppConstants.Exceptions.MoreThanOneBuildingBlocks(buildingBlockType.ToString()));

         return items;
      }
   }
}