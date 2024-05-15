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
      void AddCloneToProject(Module moduleToClone);
      void AddNewInitialConditionsBuildingBlock(Module module);
      void AddNewParameterValuesBuildingBlock(Module module);
      void MakeExtendModule(Module module);
      void MakeOverwriteModule(Module module);
   }

   public class InteractionTasksForModule : InteractionTasksForChildren<MoBiProject, Module>, IInteractionTasksForModule
   {
      private IMoBiContext context => _interactionTaskContext.Context;

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

            context.AddToHistory(GetAddCommand(module, context.CurrentProject, null)
               .Run(context));
         }
      }

      public void AddBuildingBlocksToModule(Module module)
      {
         addBuildingBlocksToModule(module, presenter => presenter.AddBuildingBlocksToModule(module));
      }

      private void addBuildingBlocksToModule(Module module, Func<IAddBuildingBlocksToModulePresenter, IReadOnlyList<IBuildingBlock>> buildingBlockCreator)
      {
         using (var presenter = _interactionTaskContext.ApplicationController.Start<IAddBuildingBlocksToModulePresenter>())
         {
            var listOfNewBuildingBlocks = buildingBlockCreator(presenter);

            if (!listOfNewBuildingBlocks.Any())
               return;

            context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, listOfNewBuildingBlocks)
               .Run(context));
         }
      }

      public void AddNewParameterValuesBuildingBlock(Module module)
      {
         addBuildingBlocksToModule(module, presenter => presenter.AddParameterValuesToModule(module));
      }

      public void MakeExtendModule(Module module)
      {
         context.AddToHistory(new SetDefaultMergeBehavior(module, MergeBehavior.Extend).Run(context));
      }

      public void MakeOverwriteModule(Module module)
      {
         context.AddToHistory(new SetDefaultMergeBehavior(module, MergeBehavior.Overwrite).Run(context));
      }

      public void AddNewInitialConditionsBuildingBlock(Module module)
      {
         addBuildingBlocksToModule(module, presenter => presenter.AddInitialConditionsToModule(module));
      }

      public void LoadBuildingBlocksToModule(Module module)
      {
         loadBuildingBlocksToModule(module, AskForPKMLFileToOpen);
      }

      public void LoadBuildingBlocksFromTemplateToModule(Module module)
      {
         loadBuildingBlocksToModule(module, openTemplateFile);
      }

      public override IMoBiCommand Remove(Module module, MoBiProject parent, IBuildingBlock buildingBlock, bool silent = false)
      {
         var referringSimulations = Context.CurrentProject.SimulationsUsing(module);
         if (referringSimulations.Any())
            throw new MoBiException(AppConstants.CannotRemoveModuleFromProject(module.Name, referringSimulations.AllNames()));

         return base.Remove(module, parent, buildingBlock, silent);
      }

      public void AddCloneToProject(Module moduleToClone)
      {
         var clonedModule = context.Clone(moduleToClone);

         using (var presenter = _interactionTaskContext.ApplicationController.Start<ICloneBuildingBlocksToModulePresenter>())
         {
            if (presenter.SelectClonedBuildingBlocks(clonedModule) == false)
               return;
         }

         context.AddToHistory(GetAddCommand(clonedModule, context.CurrentProject, null)
            .Run(context));
      }

      private void loadBuildingBlocksToModule(Module module, Func<string> getFilename)
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

            if (!items.Any())
               return;

            context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, items)
               .Run(context));
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
            case BuildingBlockType.InitialConditions:
               items.AddRange(InteractionTask.LoadItems<InitialConditionsBuildingBlock>(filename));
               return items;
            case BuildingBlockType.ParameterValues:
               items.AddRange(InteractionTask.LoadItems<ParameterValuesBuildingBlock>(filename));
               return items;
         }

         //if there are multiple BBs where only one is allowed, abort
         if (items.Count > 1)
            throw new MoBiException(AppConstants.Exceptions.MoreThanOneBuildingBlocks(buildingBlockType.ToString()));

         return items;
      }
   }
}