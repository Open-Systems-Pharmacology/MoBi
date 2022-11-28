using MoBi.Presentation.Tasks;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadExpressionProfileUICommand : IUICommand
   {
      private readonly IExpressionProfileTask _expressionProfileTask;

      public LoadExpressionProfileUICommand(IExpressionProfileTask expressionProfileTask)
      {
         _expressionProfileTask = expressionProfileTask;
      }

      public void Execute()
      {
         _expressionProfileTask.LoadExpressionProfileIntoProject();
      }
   }
}