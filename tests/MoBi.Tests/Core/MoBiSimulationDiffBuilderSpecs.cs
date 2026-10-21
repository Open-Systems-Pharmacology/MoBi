using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Container;

namespace MoBi.Core
{
   public abstract class concern_for_ObjectComparer : ContextForIntegration<IObjectComparer>
   {
      protected IObjectBase _object1;
      protected IObjectBase _object2;
      protected ComparerSettings _comparerSettings;
      protected DiffReport _report;

      protected override void Context()
      {
         sut = IoC.Resolve<IObjectComparer>();
         _comparerSettings = new ComparerSettings();
      }

      protected override void Because()
      {
         _report = sut.Compare(_object1, _object2, _comparerSettings);
      }
   }

   class When_comparing_two_similar_simulations : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var m1 = new Model().WithName("S1");
         var m2 = new Model().WithName("S1");

         var bc1 = new SimulationConfiguration();
         var bc2 = new SimulationConfiguration();

         var sim1 = new MoBiSimulation { Configuration = bc1, Model = m1 }.WithName("S1");
         var sim2 = new MoBiSimulation { Configuration = bc2, Model = m2 }.WithName("S1");

         _object1 = sim1;
         _object2 = sim2;
      }

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }

   abstract class When_comparing_two_simulations_differing_only_by_their_validation_options : concern_for_ObjectComparer
   {
      protected override void Context()
      {
         base.Context();
         var sim1 = new MoBiSimulation { Configuration = new SimulationConfiguration(), Model = new Model().WithName("S1"), ValidationOptions = ValidationOptions1 }.WithName("S1");
         var sim2 = new MoBiSimulation { Configuration = new SimulationConfiguration(), Model = new Model().WithName("S1"), ValidationOptions = ValidationOptions2 }.WithName("S1");

         _object1 = sim1;
         _object2 = sim2;
      }

      protected abstract SimulationValidationOptions ValidationOptions1 { get; }
      protected abstract SimulationValidationOptions ValidationOptions2 { get; }

      protected PropertyValueDiffItem TheDifference => _report.OfType<PropertyValueDiffItem>().Single();
   }

   class When_comparing_two_simulations_with_different_values_for_a_validation_option : When_comparing_two_simulations_differing_only_by_their_validation_options
   {
      protected override SimulationValidationOptions ValidationOptions1 => new SimulationValidationOptions { CheckRules = true };
      protected override SimulationValidationOptions ValidationOptions2 => new SimulationValidationOptions { CheckRules = false };

      [Observation]
      public void should_report_the_validation_option_difference()
      {
         _report.Count.ShouldBeEqualTo(1);
         TheDifference.PropertyName.ShouldBeEqualTo(AppConstants.Captions.ValidateRules);
         TheDifference.FormattedValue1.ShouldBeEqualTo(true.ToString());
         TheDifference.FormattedValue2.ShouldBeEqualTo(false.ToString());
      }
   }

   class When_comparing_two_simulations_where_only_one_sets_a_validation_option : When_comparing_two_simulations_differing_only_by_their_validation_options
   {
      protected override SimulationValidationOptions ValidationOptions1 => new SimulationValidationOptions();
      protected override SimulationValidationOptions ValidationOptions2 => new SimulationValidationOptions { WarnForNonFiniteQuantities = false };

      [Observation]
      public void should_report_the_option_using_the_user_setting_as_different_from_the_explicit_value()
      {
         _report.Count.ShouldBeEqualTo(1);
         TheDifference.PropertyName.ShouldBeEqualTo(AppConstants.Captions.ShowWarningsForNonFiniteQuantities);
         TheDifference.FormattedValue1.ShouldBeEqualTo(AppConstants.Captions.ValidationOptionFromUserSettings);
         TheDifference.FormattedValue2.ShouldBeEqualTo(false.ToString());
      }
   }

   class When_comparing_two_simulations_with_the_same_validation_options : When_comparing_two_simulations_differing_only_by_their_validation_options
   {
      protected override SimulationValidationOptions ValidationOptions1 => new SimulationValidationOptions { CheckRules = true, CheckDimensions = false };
      protected override SimulationValidationOptions ValidationOptions2 => new SimulationValidationOptions { CheckRules = true, CheckDimensions = false };

      [Observation]
      public void should_not_report_any_differences()
      {
         _report.ShouldBeEmpty();
      }
   }
}