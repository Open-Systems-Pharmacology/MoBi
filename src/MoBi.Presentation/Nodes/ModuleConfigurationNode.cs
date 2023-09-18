using System;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Presentation.Nodes;

namespace MoBi.Presentation.Nodes
{
   public class ModuleConfigurationNode : AbstractNode<ModuleConfigurationDTO>
   {
      public ModuleConfigurationNode(ModuleConfigurationDTO moduleConfiguration)
         : base(moduleConfiguration)
      {
         // Do not use the module Id here because there can be many configurations
         // using the same module
         Id = Guid.NewGuid().ToString();
         Text = moduleConfiguration.Module.Name;
      }

      public string ModuleName => Tag.Module.Name;

      protected override void UpdateText()
      {
         Text = Tag.Module.Name;
      }

      public override string Id { get; }
      public ApplicationIcon BaseIcon => ApplicationIcons.Module;
   }
}