using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Core
{
   public class When_creating_the_simulation_transfer_for_a_simulation_with_tracked_changes : StaticContextSpecification
   {
      private MoBiSimulation _simulation;
      private OriginalQuantityValue _originalQuantityValue;
      private SimulationTransfer _simulationTransfer;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation { HasUntraceableChanges = true };
         _originalQuantityValue = new OriginalQuantityValue { Path = "Organism|Volume", Value = 1, Type = OriginalQuantityValue.Types.Quantity };
         _simulation.AddOriginalQuantityValue(_originalQuantityValue);
      }

      protected override void Because()
      {
         _simulationTransfer = _simulation.ToSimulationTransfer();
      }

      [Observation]
      public void should_return_a_transfer_for_the_simulation()
      {
         _simulationTransfer.Simulation.ShouldBeEqualTo(_simulation);
      }

      [Observation]
      public void should_add_the_changes_tracked_in_the_simulation_to_the_transfer()
      {
         _simulationTransfer.OriginalQuantityValues.ShouldOnlyContain(_originalQuantityValue);
         _simulationTransfer.HasUntraceableChanges.ShouldBeTrue();
      }
   }

   public class When_creating_the_simulation_transfer_for_a_simulation_without_tracked_changes : StaticContextSpecification
   {
      private SimulationTransfer _simulationTransfer;

      protected override void Because()
      {
         _simulationTransfer = new MoBiSimulation().ToSimulationTransfer();
      }

      [Observation]
      public void should_return_a_transfer_without_changes()
      {
         _simulationTransfer.OriginalQuantityValues.ShouldBeEmpty();
         _simulationTransfer.HasUntraceableChanges.ShouldBeFalse();
      }
   }
}
