using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
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

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         sut = DomainHelperForSpecs.QuantityValueChangeTracker(_eventPublisher);
      }
   }

   public class When_tracking_change_to_a_quantity_and_scale_in_a_simulation : concern_for_QuantityValueChangeTracker
   {
      private MoleculeAmount _quantity;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();

         _quantity = new MoleculeAmount
         {
            Name = "moleculeAmountName",
            Value = 1.0,
            Dimension = DimensionFactoryForSpecs.MassDimension,
            DisplayUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit,
            ScaleDivisor = 1.0
         };

         var container = new Container().WithName("topContainer");
         container.Add(_quantity);
      }

      protected override void Because()
      {
         sut.TrackQuantityChange(_quantity, _simulation, x => x.Value = 3.0);
         sut.TrackScaleChange(_quantity, _simulation, x => x.ScaleDivisor = 3.0, true);
      }

      [Observation]
      public void the_simulation_should_contain_separate_trackers_for_value_and_scale()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_tracking_change_to_scale_divisor_a_simulation : concern_for_QuantityValueChangeTracker
   {
      private MoleculeAmount _quantity;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();

         _quantity = new MoleculeAmount
         {
            Name = "moleculeAmountName",
            Value = 1.0,
            Dimension = DimensionFactoryForSpecs.MassDimension,
            DisplayUnit = DimensionFactoryForSpecs.MassDimension.DefaultUnit,
            ScaleDivisor = 1.0
         };

         var container = new Container().WithName("topContainer");
         container.Add(_quantity);
      }

      protected override void Because()
      {
         sut.TrackScaleChange(_quantity, _simulation, x => x.ScaleDivisor = 3.0, true);
         sut.TrackScaleChange(_quantity, _simulation, x => x.ScaleDivisor = 1.0, true);
      }

      [Observation]
      public void the_simulation_should_contain_a_tracker_object_with_the_original_value()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
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
         sut.TrackQuantityChange(_quantity, _simulation, x => x.Value = 3.0);
         sut.TrackQuantityChange(_quantity, _simulation, x => x.Value = 1.0);
      }

      [Observation]
      public void the_simulation_should_contain_a_tracker_object_with_the_original_value()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }
   }

   public abstract class When_tracking_no_changes_to_a_quantity_in_a_simulation : concern_for_QuantityValueChangeTracker
   {
      protected IQuantity _quantity;
      protected IMoBiSimulation _simulation;

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

      [Observation]
      public void the_simulation_should_contain_a_tracker_object_with_the_original_value()
      {
         _simulation.OriginalQuantityValues.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_tracking_no_changes_to_a_quantity_in_a_simulation_without_events : When_tracking_no_changes_to_a_quantity_in_a_simulation
   {
      protected override void Because()
      {
         sut.TrackQuantityChange(_quantity, _simulation, x => { }, false);
      }

      [Observation]
      public void the_event_should_be_published()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>._)).MustNotHaveHappened();
      }
   }

   public class When_tracking_no_changes_to_a_quantity_in_a_simulation_with_events : When_tracking_no_changes_to_a_quantity_in_a_simulation
   {
      protected override void Because()
      {
         sut.TrackQuantityChange(_quantity, _simulation, x => { }, true);
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
      }

      protected override void Because()
      {
         sut.TrackQuantityChange(_quantity, _simulation, x => x.Value = 2.0);
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