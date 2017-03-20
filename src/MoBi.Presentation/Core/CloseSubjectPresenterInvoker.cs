using OSPSuite.Utility.Events;
using MoBi.Core.Events;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Core
{
   public interface ICloseSubjectPresenterInvoker : ICloseSubjectPresenterInvokerBase, IListener<ProjectClosedEvent>, IListener<RemovedEvent>, IListener<SimulationRemovedEvent>
   {
   }

   public class CloseSubjectPresenterInvoker : CloseSubjectPresenterInvokerBase, ICloseSubjectPresenterInvoker
   {
      public CloseSubjectPresenterInvoker(IApplicationController applicationController) : base(applicationController)
      {
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         CloseAll();
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         foreach (var removedObject in eventToHandle.RemovedObjects)
         {
            Close(removedObject);
         }
      }

      public void Handle(SimulationRemovedEvent eventToHandle)
      {
         Close(eventToHandle.Simulation);
      }
   }
}
