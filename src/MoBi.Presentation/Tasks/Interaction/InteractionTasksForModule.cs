using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Core.Extensions;
using MoBi.Core.Helper;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
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
      void MoveBuildingBlock(IBuildingBlock buildingBlockToMove, Module destinationModule);
      void CopyBuildingBlock(IBuildingBlock buildingBlockToCopy, Module destinationModule);
      bool Remove(IReadOnlyList<Module> modulesToRemove);
   }

   public class InteractionTasksForModule : InteractionTasksForChildren<MoBiProject, Module>, IInteractionTasksForModule
   {
      private readonly IParameterValuesTask _parameterValuesTask;
      private readonly IInitialConditionsTask<InitialConditionsBuildingBlock> _initialConditionsTask;
      private IMoBiContext context => _interactionTaskContext.Context;

      public InteractionTasksForModule(
         IInteractionTaskContext interactionTaskContext, 
         IEditTaskForModule editTask,
         IParameterValuesTask parameterValuesTask,
         IInitialConditionsTask<InitialConditionsBuildingBlock> initialConditionsTask) : base(interactionTaskContext, editTask)
      {
         _parameterValuesTask = parameterValuesTask;
         _initialConditionsTask = initialConditionsTask;
      }

      public override IMoBiCommand GetRemoveCommand(Module objectToRemove, MoBiProject parent, IBuildingBlock buildingBlock) => new RemoveModuleCommand(objectToRemove);

      public override IMoBiCommand GetAddCommand(Module itemToAdd, MoBiProject parent, IBuildingBlock buildingBlock) => new AddModuleCommand(itemToAdd);

      public IMoBiCommand GetAddBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks) 
         => new AddMultipleBuildingBlocksToModuleCommand(existingModule, listOfNewBuildingBlocks);

      /// <summary>
      /// Corrects the name of building blocks if they are parameter values or initial conditions
      /// Other types of building blocks can not have multiple instances in the same module, so the names do not need to be checked
      /// </summary>
      /// <returns>True if all building blocks have been uniquely named, if rename was not done, returns false</returns>
      private bool correctBuildingBlockNames(IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks, Module existingModule)
      {
         foreach (var x in listOfNewBuildingBlocks)
         {
            switch (x)
            {
               case ParameterValuesBuildingBlock parameterValuesBuildingBlock when _parameterValuesTask.CorrectName(parameterValuesBuildingBlock, existingModule) == false:
               case InitialConditionsBuildingBlock initialConditionsBuildingBlock when _initialConditionsTask.CorrectName(initialConditionsBuildingBlock, existingModule) == false:
                  return false;
            }
         };

         return true;
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
               .RunCommand(context));
         }
      }

      public void AddBuildingBlocksToModule(Module module) => addBuildingBlocksToModule(module, presenter => presenter.AddBuildingBlocksToModule(module));

      private void addBuildingBlocksToModule(Module module, Func<IAddBuildingBlocksToModulePresenter, IReadOnlyList<IBuildingBlock>> buildingBlockCreator)
      {
         using (var presenter = _interactionTaskContext.ApplicationController.Start<IAddBuildingBlocksToModulePresenter>())
         {
            var listOfNewBuildingBlocks = buildingBlockCreator(presenter);

            if (!listOfNewBuildingBlocks.Any())
               return;

            context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, listOfNewBuildingBlocks)
               .RunCommand(context));
         }
      }

      public void AddNewParameterValuesBuildingBlock(Module module) => addBuildingBlocksToModule(module, presenter => presenter.AddParameterValuesToModule(module));

      public void MakeExtendModule(Module module) => context.AddToHistory(new SetMergeBehaviorCommand(module, MergeBehavior.Extend).RunCommand(context));

      public void MakeOverwriteModule(Module module) => context.AddToHistory(new SetMergeBehaviorCommand(module, MergeBehavior.Overwrite).RunCommand(context));

      public void MoveBuildingBlock(IBuildingBlock buildingBlockToMove, Module destinationModule)
      {
         if (buildingBlockToMove.Module == null)
            return;

         context.AddToHistory(new MoveBuildingBlockToModuleCommand(buildingBlockToMove, destinationModule)
            .RunCommand(context));
      }

      public void CopyBuildingBlock(IBuildingBlock buildingBlockToCopy, Module destinationModule)
      {
         if (buildingBlockToCopy.Module == null)
            return;

         var buildingBlock = context.Clone(buildingBlockToCopy);

         context.AddToHistory(new CopyBuildingBlockToModuleCommand<IBuildingBlock>(buildingBlock, destinationModule)
            .RunCommand(context));
      }

      public void AddNewInitialConditionsBuildingBlock(Module module) => addBuildingBlocksToModule(module, presenter => presenter.AddInitialConditionsToModule(module));

      public void LoadBuildingBlocksToModule(Module module) => loadBuildingBlocksToModule(module, AskForPKMLFileToOpen);

      public void LoadBuildingBlocksFromTemplateToModule(Module module) => loadBuildingBlocksToModule(module, openTemplateFile);

      private IReadOnlyList<IMoBiSimulation> simulationsUsing(Module module) => Context.CurrentProject.SimulationsUsing(module).ToList();

      public override IMoBiCommand Remove(Module module, MoBiProject parent, IBuildingBlock buildingBlock, bool silent = false)
      {
         var referringSimulations = simulationsUsing(module);
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
            .RunCommand(context));
      }

      public bool Remove(IReadOnlyList<Module> modulesToRemove)
      {
         if (_interactionTaskContext.DialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveAllModules(modulesToRemove.AllNames())) != ViewResult.Yes)
            return false;

         var macroCommand = new MoBiMacroCommand
         {
            Description = AppConstants.Commands.RemoveMultipleModules,
            ObjectType = new ObjectTypeResolver().TypeFor<Module>(),
            CommandType = AppConstants.Commands.DeleteCommand
         };

         var modulesRemoved = modulesToRemove.Where(x => !simulationsUsing(x).Any()).ToList();
         modulesRemoved.Each(x => macroCommand.Add(GetRemoveCommand(x, null, null)));

         var modulesSkipped = modulesToRemove.Except(modulesRemoved).ToList();
         if (modulesSkipped.Any())
            showCouldNotRemoveMessage(modulesSkipped.ToList());

         context.AddToHistory(macroCommand.RunCommand(context));

         return macroCommand.IsEmpty || macroCommand.All().All(x => x.IsEmpty());
      }

      private void showCouldNotRemoveMessage(IReadOnlyList<Module> modulesNotRemoved)
      {

         _interactionTaskContext.DialogCreator.MessageBoxInfo(AppConstants.Captions.CouldNotRemoveModules(modulesNotRemoved.AllNames().ToList()));
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

            var listOfNewBuildingBlocks = loadBuildingBlocks(buildingBlockType, filename);

            if (!listOfNewBuildingBlocks.Any())
               return;

            if (!correctBuildingBlockNames(listOfNewBuildingBlocks, module))
               return;

            context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, listOfNewBuildingBlocks)
               .RunCommand(context));
         }
      }

      private string openTemplateFile()
      {
         _interactionTaskContext.UpdateTemplateDirectories();
         return InteractionTask.AskForFileToOpen(AppConstants.Dialog.LoadFromTemplate(_editTask.ObjectName),
            Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.TEMPLATE);
      }

      private List<IBuildingBlock> loadBuildingBlocks(BuildingBlockType buildingBlockType, string filename)
      {
         var items = new List<IBuildingBlock>();

         switch (buildingBlockType)
         {
            case BuildingBlockType.Reactions:
               items.AddRange(InteractionTask.LoadItems<MoBiReactionBuildingBlock>(filename));
               break;
            case BuildingBlockType.Events:
               items.AddRange(InteractionTask.LoadItems<EventGroupBuildingBlock>(filename));
               break;
            case BuildingBlockType.SpatialStructure:
               items.AddRange(InteractionTask.LoadItems<MoBiSpatialStructure>(filename));
               break;
            case BuildingBlockType.PassiveTransports:
               items.AddRange(InteractionTask.LoadItems<PassiveTransportBuildingBlock>(filename));
               break;
            case BuildingBlockType.Molecules:
               items.AddRange(InteractionTask.LoadItems<MoleculeBuildingBlock>(filename));
               break;
            case BuildingBlockType.Observers:
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