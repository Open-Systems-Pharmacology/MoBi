﻿using System;
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