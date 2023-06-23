using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class InitialConditionsFolderNode : AbstractNode<ModuleViewItem>
   {
      public InitialConditionsFolderNode(Module module) : base(new ModuleViewItem(module).WithTarget(module.InitialConditionsCollection))
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.InitialConditions;
         Icon = ApplicationIcons.InitialConditionsFolder;
      }
      public override string Id { get; }
   }
}