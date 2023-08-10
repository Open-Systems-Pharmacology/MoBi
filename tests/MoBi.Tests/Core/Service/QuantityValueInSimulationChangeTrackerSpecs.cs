using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Service
{
   public class concern_for_QuantityValueChangeTracker : ContextSpecification<QuantityValueInSimulationChangeTracker>
   {
      protected IEventPublisher _eventPublisher;
      protected IQuantityToParameterValueMapper _quantityToParameterValueMapper;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _quantityToParameterValueMapper = new QuantityToParameterValueMapper(new EntityPathResolverForSpecs());
      }
   }

   public class When_tracking_change_and_change_back_to_a_quantity_in_a_simulation : concern_for_QuantityValueChangeTracker
   {
      private IQuantity _quantity;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();

         _quantity = new Parameter
         {
            Name = "parameterName",
            Value = 1.0,
            Formula = new ExplicitFormula("1"),
            Dimension = DimensionFactoryForSpecs.MassDimension,
            DisplayUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit
         };

         var container = new Container().WithName("topContainer");
         container.Add(_quantity);
      }

      protected override void Because()
      {
         new QuantityValueInSimulationChangeTracker(_quantityToParameterValueMapper, _eventPublisher, new EntityPathResolverForSpecs()).TrackChanges(_quantity, _simulation, x => x.Value = 3.0);
         new QuantityValueInSimulationChangeTracker(_quantityToParameterValueMapper, _eventPublisher, new EntityPathResolverForSpecs()).TrackChanges(_quantity, _simulation, x => x.Value = 1.0);
      }

      [Observation]
      public void the_simulation_should_contain_a_tracker_object_with_the_original_value()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_tracking_no_changes_to_a_quantity_in_a_simulation : concern_for_QuantityValueChangeTracker
   {
      private IQuantity _quantity;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();

         _quantity = new Parameter
         {
            Name = "parameterName",
            Value = 1.0,
            Formula = new ExplicitFormula("1"),
            Dimension = DimensionFactoryForSpecs.MassDimension,
            DisplayUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit
         };

         var container = new Container().WithName("topContainer");
         container.Add(_quantity);

         sut = new QuantityValueInSimulationChangeTracker(_quantityToParameterValueMapper, _eventPublisher, new EntityPathResolverForSpecs());
      }

      protected override void Because()
      {
         sut.TrackChanges(_quantity, _simulation, x => { });
      }

      [Observation]
      public void the_simulation_should_contain_a_tracker_object_with_the_original_value()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }

      [Observation]
      public void the_event_should_be_published()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>.That.Matches(e => e.Simulation == _simulation))).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_tracking_changes_to_a_quantity_in_a_simulation : concern_for_QuantityValueChangeTracker
   {
      private IQuantity _quantity;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         
         _quantity = new Parameter
         {
            Name = "parameterName",
            Value = 1.0,
            Formula = new ExplicitFormula("1"),
            Dimension = DimensionFactoryForSpecs.MassDimension,
            DisplayUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit
         };

         var container = new Container().WithName("topContainer");
         container.Add(_quantity);
         sut = new QuantityValueInSimulationChangeTracker(_quantityToParameterValueMapper, _eventPublisher, new EntityPathResolverForSpecs());
      }

      protected override void Because()
      {
         sut.TrackChanges(_quantity, _simulation, x => x.Value = 2.0);
      }

      [Observation]
      public void the_simulation_should_contain_a_tracker_object_with_the_original_value()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(1);
         _simulation.OriginalQuantityValues.First().Value.ShouldBeEqualTo(1.0);
      }

      [Observation]
      public void the_event_should_be_published()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>.That.Matches(e => e.Simulation == _simulation))).MustHaveHappenedOnceExactly();
      }
   }
}
