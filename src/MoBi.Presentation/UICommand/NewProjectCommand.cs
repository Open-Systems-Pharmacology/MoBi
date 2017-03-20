using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.UICommand
{
   public abstract class NewProjectCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;
      private readonly ReactionDimensionMode _reactionDimensionMode;

      protected NewProjectCommand(IProjectTask projectTask, ReactionDimensionMode reactionDimensionMode)
      {
         _projectTask = projectTask;
         _reactionDimensionMode = reactionDimensionMode;
      }

      public void Execute()
      {
         _projectTask.New(_reactionDimensionMode);
      }
   }

   public class NewAmountProjectCommand : NewProjectCommand
   {
      public NewAmountProjectCommand(IProjectTask projectTask) : base(projectTask, ReactionDimensionMode.AmountBased)
      {
      }
   }

   public class NewConcentrationProjectCommand : NewProjectCommand
   {
      public NewConcentrationProjectCommand(IProjectTask projectTask)
         : base(projectTask, ReactionDimensionMode.ConcentrationBased)
      {
      }
   }
}

