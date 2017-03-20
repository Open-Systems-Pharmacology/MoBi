using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Core
{
   public class Workspace : IWithWorkspaceLayout
   {
      public IWorkspaceLayout WorkspaceLayout { get; set; }

      public Workspace()
      {
         WorkspaceLayout = new WorkspaceLayout();
      }
   }
}
