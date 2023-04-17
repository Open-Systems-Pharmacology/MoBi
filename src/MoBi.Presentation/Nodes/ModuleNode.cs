using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;

namespace MoBi.Presentation.Nodes
{
   public class ModuleNode : ObjectWithIdAndNameNode<Module>
   {
      public ModuleNode(Module module) : base(module)
      {
      }
   }
}