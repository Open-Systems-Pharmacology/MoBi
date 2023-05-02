using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
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
   public interface IInteractionTasksForModule : IInteractionTasksForChildren<MoBiProject, Module>
   {
      void CreateNewModuleWithBuildingBlocks();
      void AddBuildingBlocksToModule(Module module);
      void LoadBuildingBlocksToModule(Module module);
      void LoadBuildingBlocksFromTemplateToModule(Module module);
      void RemoveModule(Module module);
      void AddCloneToProject(Module moduleToClone);
   }

   public class InteractionTasksForModule : InteractionTasksForChildren<MoBiProject, Module>, IInteractionTasksForModule
   {
      public InteractionTasksForModule(IInteractionTaskContext interactionTaskContext, IEditTaskForModule editTask) : base(interactionTaskContext,
         editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(Module objectToRemove, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new RemoveModuleCommand(objectToRemove);
      }

      public override IMoBiCommand GetAddCommand(Module itemToAdd, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new AddModuleCommand(itemToAdd);
      }

      public IMoBiCommand GetAddBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks)
      {
         return new AddMultipleBuildingBlocksToModuleCommand(existingModule, listOfNewBuildingBlocks);
      }

      protected override void SetAddCommandDescription(Module child, MoBiProject parent, IMoBiCommand addCommand, MoBiMacroCommand macroCommand,
         IBuildingBlock buildingBlock)
      {
         addCommand.Description = AppConstants.Commands.AddToProjectDescription(addCommand.ObjectType, child.Name);
         macroCommand.Description = addCommand.Description;
      }

      public void CreateNewModuleWithBuildingBlocks()
      {
         using (var presenter = _interactionTaskContext.ApplicationController.Start<ICreateModulePresenter>())
         {
            var module = presenter.CreateModule();

            if (module == null)
               return;

            _interactionTaskContext.Context.AddToHistory(GetAddCommand(module, _interactionTaskContext.Context.CurrentProject, null)
               .Run(_interactionTaskContext.Context));
         }
      }

      public void AddBuildingBlocksToModule(Module module)
      {
         using (var presenter = _interactionTaskContext.ApplicationController.Start<IAddBuildingBlocksToModulePresenter>())
         {
            var listOfNewBuildingBlocks = presenter.AddBuildingBlocksToModule(module);

            if (!listOfNewBuildingBlocks.Any())
               return;

            _interactionTaskContext.Context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, listOfNewBuildingBlocks)
               .Run(_interactionTaskContext.Context));
         }
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

      public void AddCloneToProject(Module moduleToClone)
      {
         var clonedModule = _interactionTaskContext.Context.Clone(moduleToClone);

         using (var presenter = _interactionTaskContext.ApplicationController.Start<ICloneBuildingBlocksToModulePresenter>())
         {
            if (presenter.SelectClonedBuildingBlocks(clonedModule) == false)
               return;
         }
         
         _interactionTaskContext.Context.AddToHistory(GetAddCommand(clonedModule, _interactionTaskContext.Context.CurrentProject, null)
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