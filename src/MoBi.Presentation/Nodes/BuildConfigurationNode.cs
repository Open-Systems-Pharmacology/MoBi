using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class BuildConfigurationNode : AbstractNode<IMoBiBuildConfiguration>
   {
      public BuildConfigurationNode(IMoBiBuildConfiguration buildConfiguration)
         : base(buildConfiguration)
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.BuildConfiguration;
      }

      public override string Id { get; }
   }
}