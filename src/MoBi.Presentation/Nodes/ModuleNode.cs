using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ModuleNode : AbstractNode<Module>
   {
      public ModuleNode(Module module) : base(module)
      {
         Id = ShortGuid.NewGuid();
         Text = module.Name;
      }

      public override string Id { get; }
   }
}