using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForBuildingBlock
   {
      IMoBiCommand Merge(IBuildingBlock buildingBlockToMerge, IBuildingBlock targetBuildingBlock);
      void EditBuildingBlock(IBuildingBlock buildingBlock);
      IMoBiCommand AddToProject(IBuildingBlock buildingBlockToAdd);
   }

   public interface IInteractionTasksForBuildingBlock<T> : IInteractionTasksForChildren<IMoBiProject, T>,
      IInteractionTasksForBuildingBlock where T : class, IObjectBase
   {
      IMoBiCommand AddNew();
      IMoBiCommand Clone(T buildingBlockToClone);
      IMoBiCommand Merge(T buildingBlockToMerge, T targetBuildingBlock);
      IMoBiCommand AddToProject(T buildingBlockToAdd);
   }

   public abstract class InteractionTasksForBuildingBlock<TBuildingBlock> :
      InteractionTasksForChildren<IMoBiProject, TBuildingBlock, IEditTasksForBuildingBlock<TBuildingBlock>>,
      IInteractionTasksForBuildingBlock<TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock
   {
      protected InteractionTasksForBuildingBlock(IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public IMoBiCommand AddNew()
      {
         return AddNew(Context.CurrentProject, null);
      }

      protected MoBiMacroCommand CreateMergeMacroCommand(TBuildingBlock targetBuildingBlock)
      {
         var moBiMacroCommand = new BulkUpdateMacroCommand
         {
            CommandType = AppConstants.Commands.MergeCommand,
            Description = AppConstants.Commands.MergeBuildingBlocks,
            ObjectType = _interactionTaskContext.GetTypeFor(targetBuildingBlock)
         };
         return moBiMacroCommand;
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

      public abstract IMoBiCommand Merge(TBuildingBlock buildingBlockToMerge, TBuildingBlock targetBuildingBlock);

      public IMoBiCommand AddToProject(IBuildingBlock buildingBlockToAdd)
      {
         return AddToProject(buildingBlockToAdd as TBuildingBlock);
      }

      public IMoBiCommand AddToProject(TBuildingBlock buildingBlockToAdd)
      {
         return AddToProject(buildingBlockToAdd, Context.CurrentProject, null);
      }

      public IMoBiCommand Merge(IBuildingBlock buildingBlockToMerge, IBuildingBlock targetBuildingBlock)
      {
         return Merge(buildingBlockToMerge.DowncastTo<TBuildingBlock>(),
            targetBuildingBlock.DowncastTo<TBuildingBlock>());
      }

      public override IMoBiCommand Remove(TBuildingBlock buildingBlockToRemove, IMoBiProject project,
         IBuildingBlock buildingBlock, bool silent)
      {
         var referringSimulations = project.SimulationsCreatedUsing(buildingBlockToRemove);
         if (referringSimulations.Any())
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlockToRemove.Name,
               referringSimulations.Select(simulation => simulation.Name)));

         return base.Remove(buildingBlockToRemove, project, buildingBlock, silent);
      }

      public override IMoBiCommand GetRemoveCommand(TBuildingBlock objectToRemove, IMoBiProject parent,
         IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockCommand<TBuildingBlock>(objectToRemove);
      }

      public override IMoBiCommand GetAddCommand(TBuildingBlock itemToAdd, IMoBiProject parent,
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

      protected override void SetAddCommandDescription(TBuildingBlock newEntity, IMoBiProject parent, IMoBiCommand addCommand, MoBiMacroCommand macroCommand, IBuildingBlock buildingBlock)
      {
         addCommand.Description = AppConstants.Commands.AddToProjectDescription(addCommand.ObjectType, newEntity.Name);
         macroCommand.Description = addCommand.Description;
      }
   }
}