using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class ExpressionProfilesNode : AbstractNode
   {
      public ExpressionProfilesNode()
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.ExpressionProfiles;
         Icon = ApplicationIcons.ExpressionProfileFolder;
      }
      public override string Id { get; }
      public override object TagAsObject { get; }
   }
}