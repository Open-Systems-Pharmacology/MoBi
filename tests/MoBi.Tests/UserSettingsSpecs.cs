using System.Drawing;
using FakeItEasy;
using MoBi.Core;
using MoBi.Presentation.Settings;
using MoBi.UI.Settings;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Validation;

namespace MoBi.UI
{
   public abstract class concern_for_UserSettings : ContextSpecification<UserSettings>
   {
      protected override void Context()
      {
         var configuration = A.Fake<IMoBiConfiguration>();
         A.CallTo(() => configuration.CurrentUserFolderPath).Returns("C:\\temp");
         sut = new UserSettings(null, null, new DirectoryMapSettings(), configuration);
      }
   }

   public class When_cloning_user_settings : concern_for_UserSettings
   {
      private IUserSettings _clone;

      protected override void Context()
      {
         base.Context();
         sut.RenameDependentObjectsDefault = false;
         sut.MRUListItemCount = 15;
         sut.DecimalPlace = 7;
         sut.MaximumNumberOfCoresToUse = 2;
         sut.WarnForNonFiniteQuantities = true;
         sut.DefaultParameterGroupingModeForPIAndSA = ParameterGroupingModeIdForParameterAnalyzable.Advanced;
         sut.ShowAdvancedParameters = false;
         sut.GroupParameters = true;
         sut.NumberOfBins = 42;
         sut.NumberOfIndividualsPerBin = 99;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_create_a_new_instance()
      {
         ReferenceEquals(_clone, sut).ShouldBeFalse();
      }

      [Observation]
      public void should_copy_rename_dependent_objects_default()
      {
         _clone.RenameDependentObjectsDefault.ShouldBeFalse();
      }

      [Observation]
      public void should_copy_mru_list_item_count()
      {
         _clone.MRUListItemCount.ShouldBeEqualTo((uint)15);
      }

      [Observation]
      public void should_copy_decimal_place()
      {
         _clone.DecimalPlace.ShouldBeEqualTo((uint)7);
      }

      [Observation]
      public void should_copy_maximum_number_of_cores_to_use()
      {
         _clone.MaximumNumberOfCoresToUse.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_copy_warn_for_non_finite_quantities()
      {
         _clone.WarnForNonFiniteQuantities.ShouldBeTrue();
      }

      [Observation]
      public void should_copy_default_parameter_grouping_mode()
      {
         _clone.DefaultParameterGroupingModeForPIAndSA.ShouldBeEqualTo(ParameterGroupingModeIdForParameterAnalyzable.Advanced);
      }

      [Observation]
      public void should_copy_show_advanced_parameters()
      {
         _clone.ShowAdvancedParameters.ShouldBeFalse();
      }

      [Observation]
      public void should_copy_group_parameters()
      {
         _clone.GroupParameters.ShouldBeTrue();
      }

      [Observation]
      public void should_copy_number_of_bins()
      {
         _clone.NumberOfBins.ShouldBeEqualTo(42);
      }

      [Observation]
      public void should_copy_number_of_individuals_per_bin()
      {
         _clone.NumberOfIndividualsPerBin.ShouldBeEqualTo(99);
      }
   }

   public class When_cloning_user_settings_with_child_settings : concern_for_UserSettings
   {
      private IUserSettings _clone;

      protected override void Context()
      {
         base.Context();
         sut.DiagramOptions.SnapGridVisible = true;
         sut.DiagramOptions.DefaultNodeSizeMolecule = NodeSize.Large;
         sut.DiagramOptions.DiagramColors.MoleculeNode = Color.Red;

         sut.ChartOptions.SimulationInCurveName = true;
         sut.ChartOptions.DefaultChartBackColor = Color.Blue;

         sut.ValidationSettings.CheckDimensions = true;
         sut.ValidationSettings.CheckRules = true;
         sut.ValidationSettings.ShowPKSimObserverMessages = true;

         sut.ForceLayoutConfigutation.BaseGravitationalMass = 10F;
         sut.ForceLayoutConfigutation.MaxIterations = 500;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_deep_clone_diagram_options()
      {
         ReferenceEquals(_clone.DiagramOptions, sut.DiagramOptions).ShouldBeFalse();
         _clone.DiagramOptions.SnapGridVisible.ShouldBeTrue();
         _clone.DiagramOptions.DefaultNodeSizeMolecule.ShouldBeEqualTo(NodeSize.Large);
      }

      [Observation]
      public void should_deep_clone_diagram_colors()
      {
         ReferenceEquals(_clone.DiagramOptions.DiagramColors, sut.DiagramOptions.DiagramColors).ShouldBeFalse();
         _clone.DiagramOptions.DiagramColors.MoleculeNode.ShouldBeEqualTo(Color.Red);
      }

      [Observation]
      public void should_deep_clone_chart_options()
      {
         ReferenceEquals(_clone.ChartOptions, sut.ChartOptions).ShouldBeFalse();
         _clone.ChartOptions.SimulationInCurveName.ShouldBeTrue();
         _clone.ChartOptions.DefaultChartBackColor.ShouldBeEqualTo(Color.Blue);
      }

      [Observation]
      public void should_deep_clone_validation_settings()
      {
         ReferenceEquals(_clone.ValidationSettings, sut.ValidationSettings).ShouldBeFalse();
         _clone.ValidationSettings.CheckDimensions.ShouldBeTrue();
         _clone.ValidationSettings.CheckRules.ShouldBeTrue();
         _clone.ValidationSettings.ShowPKSimObserverMessages.ShouldBeTrue();
      }

      [Observation]
      public void should_deep_clone_force_layout_configuration()
      {
         ReferenceEquals(_clone.ForceLayoutConfigutation, sut.ForceLayoutConfigutation).ShouldBeFalse();
         _clone.ForceLayoutConfigutation.BaseGravitationalMass.ShouldBeEqualTo(10F);
         _clone.ForceLayoutConfigutation.MaxIterations.ShouldBeEqualTo(500);
      }

      [Observation]
      public void should_deep_clone_display_units()
      {
         ReferenceEquals(_clone.DisplayUnits, sut.DisplayUnits).ShouldBeFalse();
      }
   }

   public class When_cloning_user_settings_modifications_to_clone_should_not_affect_original : concern_for_UserSettings
   {
      private IUserSettings _clone;

      protected override void Context()
      {
         base.Context();
         sut.DiagramOptions.SnapGridVisible = false;
         sut.ChartOptions.SimulationInCurveName = false;
         sut.ValidationSettings.CheckDimensions = false;
         sut.RenameDependentObjectsDefault = true;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
         _clone.DiagramOptions.SnapGridVisible = true;
         _clone.ChartOptions.SimulationInCurveName = true;
         _clone.ValidationSettings.CheckDimensions = true;
         _clone.RenameDependentObjectsDefault = false;
      }

      [Observation]
      public void should_not_modify_original_diagram_options()
      {
         sut.DiagramOptions.SnapGridVisible.ShouldBeFalse();
      }

      [Observation]
      public void should_not_modify_original_chart_options()
      {
         sut.ChartOptions.SimulationInCurveName.ShouldBeFalse();
      }

      [Observation]
      public void should_not_modify_original_validation_settings()
      {
         sut.ValidationSettings.CheckDimensions.ShouldBeFalse();
      }

      [Observation]
      public void should_not_modify_original_direct_properties()
      {
         sut.RenameDependentObjectsDefault.ShouldBeTrue();
      }
   }

   public class When_updating_user_settings_from_a_clone : concern_for_UserSettings
   {
      private IUserSettings _clone;

      protected override void Context()
      {
         base.Context();
         sut.RenameDependentObjectsDefault = true;
         sut.MRUListItemCount = 5;
         sut.MaximumNumberOfCoresToUse = 4;
         sut.WarnForNonFiniteQuantities = false;
         sut.ShowAdvancedParameters = true;
         sut.GroupParameters = false;
         sut.NumberOfBins = 10;
         sut.NumberOfIndividualsPerBin = 50;
         sut.DiagramOptions.SnapGridVisible = false;
         sut.ChartOptions.SimulationInCurveName = false;
         sut.ValidationSettings.CheckDimensions = false;
         sut.ForceLayoutConfigutation.BaseGravitationalMass = 2F;

         _clone = sut.Clone();
         _clone.RenameDependentObjectsDefault = false;
         _clone.MRUListItemCount = 20;
         _clone.MaximumNumberOfCoresToUse = 8;
         _clone.WarnForNonFiniteQuantities = true;
         _clone.ShowAdvancedParameters = false;
         _clone.GroupParameters = true;
         _clone.NumberOfBins = 42;
         _clone.NumberOfIndividualsPerBin = 99;
         _clone.DiagramOptions.SnapGridVisible = true;
         _clone.ChartOptions.SimulationInCurveName = true;
         _clone.ValidationSettings.CheckDimensions = true;
         _clone.ForceLayoutConfigutation.BaseGravitationalMass = 10F;
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_clone);
      }

      [Observation]
      public void should_update_rename_dependent_objects_default()
      {
         sut.RenameDependentObjectsDefault.ShouldBeFalse();
      }

      [Observation]
      public void should_update_mru_list_item_count()
      {
         sut.MRUListItemCount.ShouldBeEqualTo((uint)20);
      }

      [Observation]
      public void should_update_maximum_number_of_cores_to_use()
      {
         sut.MaximumNumberOfCoresToUse.ShouldBeEqualTo(8);
      }

      [Observation]
      public void should_update_warn_for_non_finite_quantities()
      {
         sut.WarnForNonFiniteQuantities.ShouldBeTrue();
      }

      [Observation]
      public void should_update_show_advanced_parameters()
      {
         sut.ShowAdvancedParameters.ShouldBeFalse();
      }

      [Observation]
      public void should_update_group_parameters()
      {
         sut.GroupParameters.ShouldBeTrue();
      }

      [Observation]
      public void should_update_number_of_bins()
      {
         sut.NumberOfBins.ShouldBeEqualTo(42);
      }

      [Observation]
      public void should_update_number_of_individuals_per_bin()
      {
         sut.NumberOfIndividualsPerBin.ShouldBeEqualTo(99);
      }

      [Observation]
      public void should_update_diagram_options()
      {
         sut.DiagramOptions.SnapGridVisible.ShouldBeTrue();
      }

      [Observation]
      public void should_update_chart_options()
      {
         sut.ChartOptions.SimulationInCurveName.ShouldBeTrue();
      }

      [Observation]
      public void should_update_validation_settings()
      {
         sut.ValidationSettings.CheckDimensions.ShouldBeTrue();
      }

      [Observation]
      public void should_update_force_layout_configuration()
      {
         sut.ForceLayoutConfigutation.BaseGravitationalMass.ShouldBeEqualTo(10F);
      }

      [Observation]
      public void should_not_replace_original_sub_object_references()
      {
         // UpdatePropertiesFrom should copy values INTO existing sub-objects, not replace the object reference
         var originalDiagramOptions = sut.DiagramOptions;
         var originalChartOptions = sut.ChartOptions;
         var originalValidationSettings = sut.ValidationSettings;
         sut.UpdatePropertiesFrom(_clone);
         ReferenceEquals(sut.DiagramOptions, originalDiagramOptions).ShouldBeTrue();
         ReferenceEquals(sut.ChartOptions, originalChartOptions).ShouldBeTrue();
         ReferenceEquals(sut.ValidationSettings, originalValidationSettings).ShouldBeTrue();
      }
   }

   public class When_setting_decimal_place_to_a_valid_value : concern_for_UserSettings
   {
      protected override void Because()
      {
         sut.DecimalPlace = 15;
      }

      [Observation]
      public void should_not_have_validation_errors()
      {
         sut.Validate().IsEmpty.ShouldBeTrue();
      }
   }

   public class When_setting_decimal_place_to_an_invalid_value : concern_for_UserSettings
   {
      protected override void Because()
      {
         sut.DecimalPlace = 16;
      }

      [Observation]
      public void should_have_a_validation_error()
      {
         sut.Validate().IsEmpty.ShouldBeFalse();
      }
   }
}