using MoBi.Core.Domain.Model.Diagram;
using MoBi.UI.Diagram.DiagramManagers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Tests.Diagram
{
   public abstract class concern_for_MoBiReactionDiagramManager : ContextSpecification<MoBiReactionDiagramManager>
   {
      protected override void Context()
      {
         sut = new MoBiReactionDiagramManager();
      }
   }

   public class When_creating_a_new_diagram_manager : concern_for_MoBiReactionDiagramManager
   {
      [Observation]
      public void the_manager_must_implement_the_interface()
      {
         sut.Create().IsAnImplementationOf<IMoBiReactionDiagramManager>().ShouldBeTrue();
      }
   }
}
