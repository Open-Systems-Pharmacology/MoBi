using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CalculateScaleFactorCommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksesForSimulation;

      public CalculateScaleFactorCommand(IActiveSubjectRetriever activeSubjectRetriever, IEditTasksForSimulation editTasksForSimulation)
         : base(activeSubjectRetriever)
      {
         _editTasksesForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksesForSimulation.CalculateScaleFactors(Subject);
      }
   }
}