using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForBuildingBlock
   {
      void EditBuildingBlock(IBuildingBlock buildingBlock);
      IMoBiCommand AddToProject(IBuildingBlock buildingBlockToAdd);
   }

   public interface IInteractionTasksForBuildingBlock<TParent, T> : IInteractionTasksForChildren<TParent, T>,
      IInteractionTasksForBuildingBlock where T : class, IObjectBase where TParent : class
   {
      IMoBiCommand AddNew();
      IMoBiCommand Clone(T buildingBlockToClone);
      IMoBiCommand AddToProject(T buildingBlockToAdd);
   }

   public abstract class InteractionTasksForBuildingBlock<TParent, TBuildingBlock> :
      InteractionTasksForChildren<TParent, TBuildingBlock, IEditTasksForBuildingBlock<TBuildingBlock>>,
      IInteractionTasksForBuildingBlock<TParent, TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock where TParent : class, IObjectBase
   {
      protected InteractionTasksForBuildingBlock(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public IMoBiCommand AddNew()
      {
         throw new NotImplementedException();
         // return AddNew(Context.CurrentProject, null);
      }

      /// <summary>
      ///    Clones the specified object with the name entered by user.
      /// </summary>
      /// <param name="buildingBlockToClone">The object to clone.</param>
      public IMoBiCommand Clone(TBuildingBlock buildingBlockToClone)
      {
         var name = GetNewNameForClone(buildingBlockToClone);

         if (string.IsNullOrEmpty(name))
            return new MoBiEmptyCommand();

         var clone = InteractionTask.Clone(buildingBlockToClone).WithName(name);

         return AddToProject(clone);
      }

      protected virtual string GetNewNameForClone(TBuildingBlock buildingBlockToClone)
      {
         var name = DialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(AppConstants.CloneName(buildingBlockToClone)),
            AppConstants.Captions.NewName,
            AppConstants.CloneName(buildingBlockToClone), _editTask.GetForbiddenNames(buildingBlockToClone, Context.CurrentProject.All<TBuildingBlock>()));
         return name;
      }

      public IMoBiCommand AddToProject(IBuildingBlock buildingBlockToAdd)
      {
         return AddToProject(buildingBlockToAdd as TBuildingBlock);
      }

      public IMoBiCommand AddToProject(TBuildingBlock buildingBlockToAdd)
      {
         throw new NotImplementedException();
         // return AddToProject(buildingBlockToAdd, Context.CurrentProject, null);
      }

      public override IMoBiCommand Remove(TBuildingBlock buildingBlockToRemove, TParent project,
         IBuildingBlock buildingBlock, bool silent)
      {
         var referringSimulations = Context.CurrentProject.SimulationsCreatedUsing(buildingBlockToRemove);
         if (referringSimulations.Any())
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlockToRemove.Name,
               referringSimulations.Select(simulation => simulation.Name)));

         return base.Remove(buildingBlockToRemove, project, buildingBlock, silent);
      }

      public override IMoBiCommand GetRemoveCommand(TBuildingBlock objectToRemove, TParent parent,
         IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockCommand<TBuildingBlock>(objectToRemove);
      }

      public override IMoBiCommand GetAddCommand(TBuildingBlock itemToAdd, TParent parent,
         IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockCommand<TBuildingBlock>(itemToAdd);
      }

      protected void AddCommand(IMoBiCommand moBiCommand)
      {
         Context.AddToHistory(moBiCommand);
      }

      public void EditBuildingBlock(IBuildingBlock buildingBlock)
      {
         _editTask.DowncastTo<IEditTasksForBuildingBlock<TBuildingBlock>>().EditBuildingBlock(buildingBlock);
      }

      protected override void SetAddCommandDescription(TBuildingBlock newEntity, TParent parent, IMoBiCommand addCommand, MoBiMacroCommand macroCommand, IBuildingBlock buildingBlock)
      {
         addCommand.Description = AppConstants.Commands.AddToProjectDescription(addCommand.ObjectType, newEntity.Name);
         macroCommand.Description = addCommand.Description;
      }
   }
}