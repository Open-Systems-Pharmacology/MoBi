using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ParameterStartValuesFolderNode : AbstractNode<Classification>
   {
      public ParameterStartValuesFolderNode() : base(new Classification())
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ParameterStartValues;
         Icon = ApplicationIcons.ParameterStartValuesFolder;
      }
      public override string Id { get; }
   }
}