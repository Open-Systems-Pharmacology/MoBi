using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ModuleConfigurationNode : AbstractNode<ModuleConfigurationDTO>
   {
      public ModuleConfigurationNode(ModuleConfigurationDTO moduleConfiguration)
         : base(moduleConfiguration)
      {
         Id = moduleConfiguration.Module.Id;
         Text = moduleConfiguration.Module.Name;
      }

      public override string Id { get; }
   }
}