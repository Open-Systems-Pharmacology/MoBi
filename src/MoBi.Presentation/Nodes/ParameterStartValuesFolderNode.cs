using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ParameterStartValuesFolderNode : AbstractNode<ModuleParameterStartValuesCollectionViewItem>
   {
      public ParameterStartValuesFolderNode(Module module) : base(new ModuleParameterStartValuesCollectionViewItem(module))
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ParameterStartValues;
         Icon = ApplicationIcons.ParameterStartValuesFolder;
      }
      public override string Id { get; }
   }
}