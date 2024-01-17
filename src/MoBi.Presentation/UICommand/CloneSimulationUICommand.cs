using FluentNHibernate.Utils;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CloneSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IInteractionTasksForSimulation _interactionTasksForSimulation;

      public CloneSimulationUICommand(IInteractionTasksForSimulation interactionTasksForSimulation)
      {
         _interactionTasksForSimulation = interactionTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _interactionTasksForSimulation.CloneSimulation(Subject);
      }
   }
}