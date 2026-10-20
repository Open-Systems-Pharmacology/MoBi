using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_FinalOptionsPresenter : ContextSpecification<FinalOptionsPresenter>
   {
      protected IFinalOptionsView _view;
      protected IValidationOptionsPresenter _validationOptionsPresenter;
      protected IUserSettings _userSettings;
      protected ValidationSettings _editedValidationSettings;
      protected FinalOptionsDTO _finalOptionsDTO;
      protected SimulationConfiguration _simulationConfiguration;

      protected override void Context()
      {
         _view = A.Fake<IFinalOptionsView>();
         _validationOptionsPresenter = A.Fake<IValidationOptionsPresenter>();
         _userSettings = A.Fake<IUserSettings>();
         _userSettings.CheckDimensions = true;
         _userSettings.CheckCircularReference = true;
         _userSettings.CheckRules = false;
         _userSettings.WarnForNonFiniteQuantities = true;
         _simulationConfiguration = SimulationConfigurationCheckingForNegativeValues(true);

         A.CallTo(() => _validationOptionsPresenter.Edit(A<ValidationSettings>._))
            .Invokes(x => _editedValidationSettings = x.GetArgument<ValidationSettings>(0));
         A.CallTo(() => _view.BindTo(A<FinalOptionsDTO>._))
            .Invokes(x => _finalOptionsDTO = x.GetArgument<FinalOptionsDTO>(0));

         sut = new FinalOptionsPresenter(_view, _validationOptionsPresenter, _userSettings);
      }

      protected static SimulationConfiguration SimulationConfigurationCheckingForNegativeValues(bool checkForNegativeValues)
      {
         var solverSettings = new SolverSettings();
         solverSettings.Add(DomainHelperForSpecs.ConstantParameterWithValue().WithName(Constants.Parameters.CHECK_FOR_NEGATIVE_VALUES));
         solverSettings.CheckForNegativeValues = checkForNegativeValues;

         return new SimulationConfiguration { SimulationSettings = new SimulationSettings { Solver = solverSettings } };
      }
   }

   public class When_creating_the_final_options_presenter : concern_for_FinalOptionsPresenter
   {
      [Observation]
      public void should_add_the_validation_options_view_to_the_view()
      {
         A.CallTo(() => _view.SetValidationOptionsView(_validationOptionsPresenter.View)).MustHaveHappened();
      }
   }

   public class When_the_final_options_presenter_is_editing_a_simulation_without_validation_options : concern_for_FinalOptionsPresenter
   {
      protected override void Because()
      {
         sut.Edit(_simulationConfiguration, new SimulationValidationOptions());
      }

      [Observation]
      public void should_start_from_the_user_validation_settings()
      {
         _editedValidationSettings.ShouldNotBeNull();
         _editedValidationSettings.ShouldNotBeEqualTo(_userSettings.ValidationSettings);
         _editedValidationSettings.CheckDimensions.ShouldBeTrue();
         _editedValidationSettings.CheckCircularReference.ShouldBeTrue();
         _editedValidationSettings.CheckRules.ShouldBeFalse();
      }

      [Observation]
      public void should_start_from_the_user_setting_for_non_finite_quantity_warnings()
      {
         _finalOptionsDTO.WarnForNonFiniteQuantities.ShouldBeTrue();
      }

      [Observation]
      public void should_bind_the_view_to_the_check_for_negative_values_solver_setting_of_the_simulation_configuration()
      {
         _finalOptionsDTO.CheckForNegativeValues.ShouldBeTrue();
      }
   }

   public class When_the_final_options_presenter_is_editing_a_simulation_with_validation_options : concern_for_FinalOptionsPresenter
   {
      protected override void Because()
      {
         sut.Edit(_simulationConfiguration, new SimulationValidationOptions { CheckRules = true, CheckCircularReference = false, WarnForNonFiniteQuantities = false });
      }

      [Observation]
      public void should_use_the_validation_options_of_the_simulation_that_are_set()
      {
         _editedValidationSettings.CheckRules.ShouldBeTrue();
         _editedValidationSettings.CheckCircularReference.ShouldBeFalse();
         _finalOptionsDTO.WarnForNonFiniteQuantities.ShouldBeFalse();
      }

      [Observation]
      public void should_use_the_user_settings_for_the_validation_options_that_are_not_set()
      {
         _editedValidationSettings.CheckDimensions.ShouldBeTrue();
      }
   }

   public class When_the_user_changes_the_final_options : concern_for_FinalOptionsPresenter
   {
      private SimulationConfiguration _newSimulationConfiguration;
      private SimulationValidationOptions _result;

      protected override void Context()
      {
         base.Context();
         _newSimulationConfiguration = SimulationConfigurationCheckingForNegativeValues(true);
         sut.Edit(_simulationConfiguration, new SimulationValidationOptions());
         _editedValidationSettings.CheckCircularReference = false;
         _editedValidationSettings.CheckRules = true;
         _finalOptionsDTO.WarnForNonFiniteQuantities = false;
         _finalOptionsDTO.CheckForNegativeValues = false;
      }

      protected override void Because()
      {
         sut.ApplyOptionsTo(_newSimulationConfiguration);
         _result = sut.ValidationOptions;
      }

      [Observation]
      public void should_return_validation_options_with_all_the_edited_values_set()
      {
         _result.CheckCircularReference.ShouldBeEqualTo(false);
         _result.CheckRules.ShouldBeEqualTo(true);
         _result.CheckDimensions.ShouldBeEqualTo(true);
         _result.WarnForNonFiniteQuantities.ShouldBeEqualTo(false);
      }

      [Observation]
      public void should_not_change_the_user_settings()
      {
         _userSettings.CheckCircularReference.ShouldBeTrue();
         _userSettings.CheckRules.ShouldBeFalse();
         _userSettings.WarnForNonFiniteQuantities.ShouldBeTrue();
      }

      [Observation]
      public void should_update_the_check_for_negative_values_solver_setting_of_the_simulation_configuration()
      {
         _newSimulationConfiguration.SimulationSettings.Solver.CheckForNegativeValues.ShouldBeFalse();
      }

      [Observation]
      public void should_not_change_the_solver_settings_of_the_edited_simulation_configuration()
      {
         _simulationConfiguration.SimulationSettings.Solver.CheckForNegativeValues.ShouldBeTrue();
      }
   }
}
