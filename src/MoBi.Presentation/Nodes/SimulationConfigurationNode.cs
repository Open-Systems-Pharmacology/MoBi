using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility;

namespace MoBi.Presentation.Nodes
{
   public class SimulationConfigurationNode : AbstractNode<SimulationConfiguration>
   {
      public SimulationConfigurationNode(SimulationConfiguration simulationConfiguration)
         : base(simulationConfiguration)
      {
         Id = ShortGuid.NewGuid();
         Text = AppConstants.Captions.SimulationConfiguration;
      }

      public override string Id { get; }
   }
}