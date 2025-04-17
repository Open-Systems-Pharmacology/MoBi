using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForChildren<TParent, TChild>
      where TParent : class
      where TChild : class
   {
      IMoBiCommand Remove(TChild objectToRemove, TParent parent, IBuildingBlock buildingBlock, bool silent = false);
      IMoBiCommand AddNew(TParent parent, IBuildingBlock buildingBlockToAddTo);
      IMoBiCommand AddExisting(TParent parent, IBuildingBlock buildingBlockWithFormulaCache);
      IMoBiCommand AddExistingTemplate(TParent parent, IBuildingBlock buildingBlockWithFormulaCache);
      IMoBiCommand AddTo(TChild childToAdd, TParent parent, IBuildingBlock buildingBlockWithFormulaCache);
      IMoBiCommand AddTo(IReadOnlyCollection<TChild> itemsToAdd, TParent parent, IBuildingBlock buildingBlockWithFormulaCache = null);
      IMoBiCommand AddFromFileTo(string filename, TParent parent, IBuildingBlock buildingBlockWithFormulaCache = null);
      IMoBiCommand GetRemoveCommand(TChild objectToRemove, TParent parent, IBuildingBlock buildingBlock);

      TChild CreateNewEntity(TParent parent);
      string AskForPKMLFileToOpen();
      IReadOnlyList<TChild> LoadFromPKML();
   }

   public abstract class InteractionTasksForChildren<TParent, TChild> : InteractionTasksForChildren<TParent, TChild, IEditTaskFor<TChild>>
      where TChild : class, IObjectBase
      where TParent : class, IObjectBase
   {
      protected InteractionTasksForChildren(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TChild> editTask) : base(interactionTaskContext, editTask)
      {
      }
   }

   public abstract class InteractionTasksForChildren<TParent, TChild, TEditTask> : IInteractionTasksForChildren<TParent, TChild>
      where TChild : class, IObjectBase
      where TParent : class, IObjectBase
      where TEditTask : IEditTaskFor<TChild>
   {
      protected readonly IInteractionTaskContext _interactionTaskContext;
      protected readonly TEditTask _editTask;

      protected InteractionTasksForChildren(IInteractionTaskContext interactionTaskContext, TEditTask editTask)
      {
         _interactionTaskContext = interactionTaskContext;
         _editTask = editTask;
      }

      protected IBuildingBlockRepository BuildingBlockRepository => _interactionTaskContext.BuildingBlockRepository;

      protected IMoBiContext Context => _interactionTaskContext.Context;

      protected IMoBiApplicationController ApplicationController => _interactionTaskContext.ApplicationController;

      protected IInteractionTask InteractionTask => _interactionTaskContext.InteractionTask;

      protected IDialogCreator DialogCreator => _interactionTaskContext.DialogCreator;

      protected virtual string ObjectName => _editTask.ObjectName;

      public virtual IMoBiCommand AddNew(TParent parent, IBuildingBlock buildingBlockToAddTo)
      {
         var macroCommand = new MoBiMacroCommand
         {
            ObjectType = ObjectName,
            CommandType = AppConstants.Commands.AddCommand,
         };

         var newEntity = CreateNewEntity(parent);
         var addCommand = GetSilentAddCommand(newEntity, parent, buildingBlockToAddTo);
         macroCommand.AddCommand(addCommand.RunCommand(Context));
         var parentContainer = parent as IEnumerable<IObjectBase> ?? Enumerable.Empty<IObjectBase>();

         if (!_editTask.EditEntityModal(newEntity, parentContainer, macroCommand, buildingBlockToAddTo))
            return CancelCommand(macroCommand);

         //allow specific methods to do something with the new entity before it is returned to the caller
         PerformPostAddActions(newEntity, parent, buildingBlockToAddTo);

         //Once the entity was created, select or edit the entity if required
         _editTask.Edit(newEntity);

         //icon needs to be set after name was defined
         newEntity.Icon = InteractionTask.IconFor(newEntity);
         throwAddedEvent(newEntity, parent);

         //Set Command Description after new object is named
         SetAddCommandDescription(newEntity, parent, addCommand, macroCommand, buildingBlockToAddTo);

         return macroCommand;
      }

      protected virtual void PerformPostAddActions(TChild newEntity, TParent parent, IBuildingBlock buildingBlockToAddTo)
      {
      }

      /// <summary>
      ///    Sets the command description after the new object is named.
      /// </summary>
      /// <param name="child">The new entity that is added </param>
      /// <param name="parent">The parent object where new entity is added</param>
      /// <param name="addCommand">The add command where the description needs to be updated</param>
      /// <param name="macroCommand">The macro command where the description needs to be updated</param>
      /// <param name="buildingBlock"></param>
      protected virtual void SetAddCommandDescription(TChild child, TParent parent, IMoBiCommand addCommand, MoBiMacroCommand macroCommand, IBuildingBlock buildingBlock)
      {
         addCommand.Description = AppConstants.Commands.AddToDescription(addCommand.ObjectType, child.Name,
            parent.Name, _interactionTaskContext.GetTypeFor(buildingBlock), buildingBlock.Name);
         macroCommand.Description = addCommand.Description;
      }

      public IMoBiCommand GetSilentAddCommand(TChild newEntity, TParent parent, IBuildingBlock buildingBlock)
      {
         var command = GetAddCommand(newEntity, parent, buildingBlock);
         var silentCommand = command as ISilentCommand;
         if (silentCommand != null)
            silentCommand.Silent = true;

         return command;
      }

      public virtual TChild CreateNewEntity(TParent parent)
      {
         return Context.Create<TChild>();
      }

      /// <summary>
      ///    Cancels the commands and returns an empty command
      /// </summary>
      protected IMoBiCommand CancelCommand(IMoBiMacroCommand command)
      {
         return _interactionTaskContext.CancelCommand(command);
      }

      public virtual IMoBiCommand AddExisting(TParent parent, IBuildingBlock buildingBlockWithFormulaCache)
      {
         var filename = AskForPKMLFileToOpen();
         return AddFromFileTo(filename, parent, buildingBlockWithFormulaCache);
      }

      public string AskForPKMLFileToOpen()
      {
         return InteractionTask.AskForFileToOpen(AppConstants.Dialog.Load(_editTask.ObjectName), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
      }

      public IReadOnlyList<TChild> LoadFromPKML()
      {
         var filename = AskForPKMLFileToOpen();
         return (string.IsNullOrEmpty(filename) ? Enumerable.Empty<TChild>() : LoadItems(filename)).ToList();
      }

      public IMoBiCommand AddExistingTemplate(TParent parent, IBuildingBlock buildingBlockWithFormulaCache)
      {
         _interactionTaskContext.UpdateTemplateDirectories();
         var filename = InteractionTask.AskForFileToOpen(AppConstants.Dialog.LoadFromTemplate(_editTask.ObjectName), Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.TEMPLATE);
         return AddFromFileTo(filename, parent, buildingBlockWithFormulaCache);
      }

      public IMoBiCommand AddFromFileTo(string filename, TParent parent, IBuildingBlock buildingBlockWithFormulaCache = null)
      {
         if (filename.IsNullOrEmpty())
            return new MoBiEmptyCommand();

         return AddTo(LoadItems(filename), parent, buildingBlockWithFormulaCache);
      }

      public virtual IReadOnlyCollection<TChild> LoadItems(string filename)
      {
         return InteractionTask.LoadItems<TChild>(filename);
      }

      public IMoBiCommand AddTo(IReadOnlyCollection<TChild> itemsToAdd, TParent parent, IBuildingBlock buildingBlockWithFormulaCache = null)
      {
         if (itemsToAdd == null || !itemsToAdd.Any())
            return new MoBiEmptyCommand();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = _editTask.ObjectName,
            Description = AppConstants.Commands.AddMany(_editTask.ObjectName)
         };

         Context.PublishEvent(new BulkUpdateStartedEvent());
         foreach (var existingItem in itemsToAdd)
         {
            var command = AddTo(existingItem, parent, buildingBlockWithFormulaCache);
            if (command.IsEmpty())
               return CancelCommand(macroCommand);

            macroCommand.Add(command);
         }

         Context.PublishEvent(new BulkUpdateFinishedEvent());

         return macroCommand;
      }

      public virtual IMoBiCommand Remove(TChild objectToRemove, TParent parent, IBuildingBlock buildingBlock, bool silent = false)
      {
         var parentName = parent == null || string.IsNullOrEmpty(parent.Name) ? AppConstants.Project : parent.Name;

         if (!silent && DialogCreator.MessageBoxYesNo(AppConstants.Dialog.Remove(InteractionTask.TypeFor(objectToRemove), objectToRemove.Name, parentName)) != ViewResult.Yes)
            return new MoBiEmptyCommand();

         var command = RunRemoveCommand(objectToRemove, parent, buildingBlock);
         Context.PublishEvent(new RemovedEvent(objectToRemove, parent));
         return command;
      }

      protected virtual IMoBiCommand RunRemoveCommand(TChild objectToRemove, TParent parent, IBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(objectToRemove, parent, buildingBlock).RunCommand(Context);
      }

      public abstract IMoBiCommand GetRemoveCommand(TChild objectToRemove, TParent parent, IBuildingBlock buildingBlock);

      public virtual IMoBiCommand AddTo(TChild childToAdd, TParent parent, IBuildingBlock buildingBlockWithFormulaCache)
      {
         var nameIsValid = CorrectName(childToAdd, parent);
         if (!nameIsValid)
            return new MoBiEmptyCommand();

         var macroCommand = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = _editTask.ObjectName,
            Description = AppConstants.Commands.AddToProjectDescription(_editTask.ObjectName, childToAdd.Name)
         };

         //add silently and raise event at the end
         macroCommand.Add(GetSilentAddCommand(childToAdd, parent, buildingBlockWithFormulaCache).RunCommand(Context));

         if (!adjustFormula(childToAdd, buildingBlockWithFormulaCache, macroCommand))
            return CancelCommand(macroCommand);

         throwAddedEvent(childToAdd, parent);

         return macroCommand;
      }

      private bool adjustFormula(TChild childToAdd, IBuildingBlock buildingBlockWithFormulaCache, IMoBiMacroCommand macroCommand)
      {
         //no formula to check for building blocks, simulations, or modules
         if (childToAdd.IsAnImplementationOf<IBuildingBlock>() ||
             childToAdd.IsAnImplementationOf<IMoBiSimulation>() ||
             childToAdd.IsAnImplementationOf<Module>())
            return true;

         return InteractionTask.AdjustFormula(childToAdd, buildingBlockWithFormulaCache, macroCommand);
      }

      private void throwAddedEvent(TChild newObjectBase, IObjectBase parent)
      {
         var newEntity = newObjectBase as IEntity;
         if (newEntity != null && newEntity.ParentContainer != null)
            parent = newEntity.ParentContainer;

         Context.PublishEvent(new AddedEvent<TChild>(newObjectBase, parent));
      }

      public virtual bool CorrectName(TChild child, TParent parent)
      {
         var parentContainer = parent as IEnumerable<IObjectBase> ?? Enumerable.Empty<IObjectBase>();
         var forbiddenNames = _editTask.GetForbiddenNames(child, parentContainer);
         return InteractionTask.CorrectName(child, forbiddenNames);
      }

      /// <summary>
      ///    Should return the command that will add <paramref name="itemToAdd" /> to <paramref name="parent" /> when run.
      /// </summary>
      public abstract IMoBiCommand GetAddCommand(TChild itemToAdd, TParent parent, IBuildingBlock buildingBlock);
   }
}