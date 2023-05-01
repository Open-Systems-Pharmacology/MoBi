using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForProjectPathAndValueEntityBuildingBlocks<TBuildingBlock, TParameter> : InteractionTasksForPathAndValueEntity<MoBiProject, TBuildingBlock, TParameter> where TParameter : PathAndValueEntity where TBuildingBlock : class, IBuildingBlock<TParameter>
   {
      protected InteractionTasksForProjectPathAndValueEntityBuildingBlocks(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask, IMoBiFormulaTask moBiFormulaTask) : base(interactionTaskContext, editTask, moBiFormulaTask)
      {
      }

      protected virtual string GetNewNameForClone(TBuildingBlock buildingBlockToClone)
      {
         var name = DialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(AppConstants.CloneName(buildingBlockToClone)),
            AppConstants.Captions.NewName,
            AppConstants.CloneName(buildingBlockToClone), _editTask.GetForbiddenNames(buildingBlockToClone, Context.CurrentProject.All<IndividualBuildingBlock>()));
         return name;
      }
      
      public IMoBiCommand Clone(TBuildingBlock buildingBlockToClone)
      {
         var name = GetNewNameForClone(buildingBlockToClone);

         if (string.IsNullOrEmpty(name))
            return new MoBiEmptyCommand();

         var clone = InteractionTask.Clone(buildingBlockToClone).WithName(name);

         return AddToProject(clone);
      }

      public IMoBiCommand AddToProject(TBuildingBlock buildingBlockToAdd)
      {
         return AddToParent(buildingBlockToAdd, Context.CurrentProject, null);
      }

      public IMoBiCommand AddToProject(IBuildingBlock buildingBlock)
      {
         return AddToProject(buildingBlock as TBuildingBlock);
      }

      public override IMoBiCommand GetRemoveCommand(TBuildingBlock objectToRemove, MoBiProject parent,
         IBuildingBlock buildingBlock)
      {
         return new RemoveProjectBuildingBlockCommand<TBuildingBlock>(objectToRemove);
      }

      public override IMoBiCommand GetAddCommand(TBuildingBlock itemToAdd, MoBiProject parent,
         IBuildingBlock buildingBlock)
      {
         return new AddProjectBuildingBlockCommand<TBuildingBlock>(itemToAdd);
      }
   }
}