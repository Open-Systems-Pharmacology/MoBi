using MoBi.Assets;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Nodes
{
   public class SimulationConfigurationNode : AbstractNode<SimulationConfiguration>
   {
      public SimulationConfigurationNode(SimulationConfiguration buildConfiguration)
         : base(buildConfiguration)
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.SimulationConfiguration;
      }

      public override string Id { get; }
   }
}