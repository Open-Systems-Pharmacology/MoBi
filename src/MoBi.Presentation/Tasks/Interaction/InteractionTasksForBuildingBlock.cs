using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
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
   }

   public interface IInteractionTasksForProjectBuildingBlock : IInteractionTasksForBuildingBlock
   {
      IMoBiCommand AddToProject(IBuildingBlock buildingBlock);
   }

   public interface IInteractionTasksForBuildingBlock<TParent, T> : IInteractionTasksForChildren<TParent, T>,
      IInteractionTasksForBuildingBlock where T : class, IObjectBase where TParent : class
   {
      
   }

   public interface IInteractionTasksForProjectBuildingBlock<T> : IInteractionTasksForBuildingBlock<MoBiProject, T> where T : class, IObjectBase
   {
      IMoBiCommand Clone(T buildingBlockToClone);
      IMoBiCommand AddToProject(T buildingBlockToAdd);
      void ExportBuildingBlockSnapshot(T buildingBlock);
      T LoadFromSnapshot(string snapshot);
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

      public override IMoBiCommand Remove(TBuildingBlock buildingBlockToRemove, TParent parent,
         IBuildingBlock buildingBlock, bool silent = false)
      {
         var referringSimulations = Context.CurrentProject.SimulationsUsing(buildingBlockToRemove);
         if (referringSimulations.Any())
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlockToRemove.Name, referringSimulations.AllNames()));

         return base.Remove(buildingBlockToRemove, parent, buildingBlock, silent);
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