using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using MoBi.IntegrationTests;
using MoBi.Presentation.Settings;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.FuncParser;

namespace MoBi.Core
{
   public abstract class concern_for_DimensionValidatorWithValidationSettings : ContextForIntegration<IDimensionValidator>
   {
      protected IUserSettings _userSettings;
      protected CreationResult _creationResult;

      protected override void Context()
      {
         _userSettings = A.Fake<IUserSettings>();
         sut = new DimensionValidator(new DimensionParser(), new ObjectPathFactory(new AliasCreator()), _userSettings);

         var buildConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();
         var topContainer = buildConfiguration.All<SpatialStructure>().First().TopContainers.First();

         //a dimensionless formula in a parameter with a dimension adds one dimension warning when dimensions are checked
         new Parameter().WithName("NODIM").WithParentContainer(topContainer)
            .WithFormula(new ExplicitFormula().WithFormulaString("111"))
            .WithDimension(new Dimension(new BaseDimensionRepresentation(), "Dimensionless", ""));

         _creationResult = DomainFactoryForSpecs.CreateModelFor(buildConfiguration, "thename");
      }
   }

   public class When_validating_a_model_with_validation_settings_disabling_the_dimension_check_enabled_in_the_user_settings : concern_for_DimensionValidatorWithValidationSettings
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _userSettings.CheckDimensions = true;
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder, new ValidationSettings { CheckDimensions = false }).Result;
      }

      [Observation]
      public void should_not_check_the_dimensions()
      {
         _result.Messages.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_validating_a_model_with_validation_settings_enabling_the_dimension_check_disabled_in_the_user_settings : concern_for_DimensionValidatorWithValidationSettings
   {
      private ValidationResult _result;

      protected override void Context()
      {
         base.Context();
         _userSettings.CheckDimensions = false;
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder, new ValidationSettings { CheckDimensions = true }).Result;
      }

      [Observation]
      public void should_check_the_dimensions()
      {
         _result.Messages.Count().ShouldBeGreaterThan(0);
      }
   }
}
