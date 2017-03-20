using MoBi.Assets;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;
using MoBi.Core.Domain.Model;

namespace MoBi.Presentation.Nodes
{
   public class BuildConfigurationNode : AbstractNode<IMoBiBuildConfiguration>
   {
      private readonly string _id;

      public BuildConfigurationNode(IMoBiBuildConfiguration buildConfiguration)
         : base(buildConfiguration)
      {
         _id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.BuildConfiguration;
      }

      public override string Id
      {
         get { return _id; }
      }
   }
}