using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ParameterStartValuesFolderNode : AbstractNode<ModuleViewItem>
   {
      public ParameterStartValuesFolderNode(Module module) : base(new ModuleViewItem(module).WithTarget(module.ParameterStartValuesCollection))
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ParameterStartValues;
         Icon = ApplicationIcons.ParameterValuesFolder;
      }
      public override string Id { get; }
   }
}