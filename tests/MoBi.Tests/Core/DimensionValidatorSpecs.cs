using System;
using System.Linq;
using System.Threading;
using FakeItEasy;
using MoBi.Assets;
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
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.FuncParser;

namespace MoBi.Core
{
   public abstract class concern_for_DimensionValidator : ContextForIntegration<IDimensionValidator>
   {
      protected ObjectPathFactory _pathFactory;
      protected IUserSettings _userSettings;
      protected SimulationConfiguration _buildConfiguration;

      protected override void Context()
      {
         _pathFactory = new ObjectPathFactory(new AliasCreator());
         _userSettings = A.Fake<IUserSettings>();
         _userSettings.CheckDimensions = true;
         _buildConfiguration = DomainFactoryForSpecs.CreateDefaultConfiguration();

         sut = new DimensionValidator(new DimensionParser(), _pathFactory, _userSettings);
      }
   }

   public class When_running_two_validations_concurrently_on_the_same_validator : concern_for_DimensionValidator
   {
      private IContainer _slowContainer;
      private IContainer _fastContainer;
      private IParameter _firstParameter;
      private IParameter _secondParameter;
      private IParameter _thirdParameter;
      private SimulationBuilder _simulationBuilder;
      private ManualResetEventSlim _firstVisitDone;
      private ManualResetEventSlim _secondValidationDone;
      private ValidationResult _firstResult;
      private ValidationResult _secondResult;

      protected override void Context()
      {
         base.Context();
         _firstVisitDone = new ManualResetEventSlim();
         _secondValidationDone = new ManualResetEventSlim();

         //parameters whose formula has no dimension: each visit adds exactly one dimension mismatch warning
         var topContainer = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First();
         addParameterWithDimensionlessFormula("NODIM_P1", topContainer);
         addParameterWithDimensionlessFormula("NODIM_P2", topContainer);
         addParameterWithDimensionlessFormula("NODIM_P3", topContainer);

         var creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
         _simulationBuilder = creationResult.SimulationBuilder;

         //visit the built model parameters: only entities tracked by the simulation builder can carry validation messages
         _firstParameter = modelParameterNamed(creationResult, "NODIM_P1");
         _secondParameter = modelParameterNamed(creationResult, "NODIM_P2");
         _thirdParameter = modelParameterNamed(creationResult, "NODIM_P3");

         //the slow container pauses its traversal until the second validation has completed on the same validator instance
         _slowContainer = A.Fake<IContainer>();
         A.CallTo(() => _slowContainer.AcceptVisitor(A<IVisitor>._)).Invokes(call =>
         {
            var visitor = call.GetArgument<IVisitor>(0);
            visit(visitor, _firstParameter);
            _firstVisitDone.Set();
            _secondValidationDone.Wait(TimeSpan.FromSeconds(5));
            visit(visitor, _secondParameter);
         });

         _fastContainer = A.Fake<IContainer>();
         A.CallTo(() => _fastContainer.AcceptVisitor(A<IVisitor>._)).Invokes(call => visit(call.GetArgument<IVisitor>(0), _thirdParameter));
      }

      private static void addParameterWithDimensionlessFormula(string name, IContainer topContainer) =>
         new Parameter().WithName(name).WithParentContainer(topContainer)
            .WithFormula(new ExplicitFormula().WithFormulaString("111"))
            .WithDimension(new Dimension(new BaseDimensionRepresentation(), "Dimensionless", ""));

      private static IParameter modelParameterNamed(CreationResult creationResult, string name) =>
         creationResult.Model.Root.GetAllChildren<IParameter>().First(x => x.Name.Equals(name));

      private static void visit(IVisitor visitor, IParameter parameter) => visitor.DowncastTo<IVisitor<IUsingFormula>>().Visit(parameter);

      protected override void Because()
      {
         var firstValidation = sut.Validate(new[] {_slowContainer}, _simulationBuilder);
         _firstVisitDone.Wait(TimeSpan.FromSeconds(5));

         _secondResult = sut.Validate(new[] {_fastContainer}, _simulationBuilder).Result;
         _secondValidationDone.Set();

         _firstResult = firstValidation.Result;
      }

      [Observation]
      public void the_first_validation_should_report_the_messages_of_its_own_run()
      {
         _firstResult.Messages.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_second_validation_should_report_the_messages_of_its_own_run()
      {
         _secondResult.Messages.Count().ShouldBeEqualTo(1);
      }
   }

   internal class When_validating_a_parameter_from_pksim_that_should_not_be_shown_in_the_default_validation : concern_for_DimensionValidator
   {
      private IContainer _root;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();

         var lengthDimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, AppConstants.DimensionNames.LENGTH, "cm");
         var molWeightDimension = new Dimension(new BaseDimensionRepresentation { AmountExponent = 1, MassExponent = -1 }, "MW", "mol/g");
         var areaDimension = new Dimension(new BaseDimensionRepresentation { LengthExponent = 2,}, AppConstants.DimensionNames.AREA, "cm�");

         var radiusFormula = new ExplicitFormula()
            .WithFormulaString("0,0333 * (MW * 1E9) ^ 0,4226 * 1E-8")
            .WithDimension(lengthDimension);

         radiusFormula.AddObjectPath(new FormulaUsablePath("Root", "MW").WithAlias("MW").WithDimension(molWeightDimension));

         var bsaFormula = new ExplicitFormula()
            .WithFormulaString("MW ^ 0,4226")
            .WithDimension(areaDimension);
         bsaFormula.AddObjectPath(new FormulaUsablePath("Root", "MW").WithAlias("MW").WithDimension(molWeightDimension));


         var pksimPara = new Parameter()
            .WithName(AppConstants.Parameters.RADIUS_SOLUTE)
            .WithFormula(radiusFormula)
            .WithDimension(lengthDimension);

         var molweight = new Parameter()
            .WithName("MW")
            .WithDimension(molWeightDimension)
            .WithFormula(new ConstantFormula(1));

         var BSA = new Parameter()
            .WithName(AppConstants.Parameters.BSA)
            .WithFormula(bsaFormula)
            .WithDimension(areaDimension);

         _root = new Container{pksimPara, molweight, BSA }.WithName("Root");

         radiusFormula.ResolveObjectPathsFor(pksimPara);
         bsaFormula.ResolveObjectPathsFor(BSA);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_root, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid, _result.Messages.Select(x=>x.Text).ToString("\n"));
      }
   }

   internal class When_validating_a_parameter_whose_dimension_can_not_be_calculated : concern_for_DimensionValidator
   {
      private IContainer _root;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();

         _root = new Container().WithName("Root");
         var length = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "cm");
         var molWeight = new Dimension(new BaseDimensionRepresentation { AmountExponent = 1, MassExponent = -1 }, "MW", "mol/g");

         var explicitFormula = new ExplicitFormula().WithFormulaString("exp(MW)").WithDimension(length);
         explicitFormula.AddObjectPath(new FormulaUsablePath(new[] { "Root", "MW" }).WithAlias("MW").WithDimension(molWeight));


         var pksimPara = new Parameter()
            .WithName(AppConstants.Parameters.RADIUS_SOLUTE)
            .WithParentContainer(_root)
            .WithFormula(explicitFormula)
            .WithDimension(length);

         new Parameter().WithName("MW").WithParentContainer(_root).WithDimension(molWeight).WithFormula(new ConstantFormula(1));
         explicitFormula.ResolveObjectPathsFor(pksimPara);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_root, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_validating_a_valid_entity : concern_for_DimensionValidator
   {
      private IContainer _root;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = new Container().WithName("Root");
         var dimension = new Dimension(new BaseDimensionRepresentation(), "Dimensionless", "");
         new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("111").WithDimension(dimension))
            .WithDimension(dimension);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_root, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_contain_no_message()
      {
         _result.Messages.ShouldBeEmpty();
      }
   }

   public class When_validating_an_invalid_entity : concern_for_DimensionValidator
   {
      private IContainer topContainer;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         topContainer = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First();
         
         var parameter = new Parameter().WithName("").WithParentContainer(topContainer)
            .WithFormula(new ExplicitFormula().WithFormulaString("111"))
            .WithDimension(new Dimension(new BaseDimensionRepresentation(), "Dimensionless", ""));

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_invalid()
      {
         _result.ValidationState.ShouldNotBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_contain_a_message()
      {
         _result.Messages.Count().ShouldBeGreaterThan(0);
      }
   }

   public class When_validating_a_formula_using_object_dimension_and_number_formula : concern_for_DimensionValidator
   {
      private IContainer _root;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First();
         var dimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Height", "m");
         new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("111").WithDimension(dimension))
            .WithDimension(dimension);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }

      [Observation]
      public void should_have_the_no_messages()
      {
         _result.Messages.ShouldBeEmpty();
      }
   }

   public class When_validating_a_formula_Using_object_dimension_and_formula_constants_and_operations : concern_for_DimensionValidator
   {
      private IContainer _root;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First();
         var dimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Height", "m");

         var parameter = new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("1+1").WithDimension(dimension))
            .WithDimension(dimension);


         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid_with_warnings()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }

      [Observation]
      public void should_have_some_warning_messages()
      {
         _result.Messages.Any().ShouldBeTrue();
      }
   }

   public class When_validating_a_Formula_using_object_dimension_and_a_not_constant : concern_for_DimensionValidator
   {
      private IContainer _root;
      private IParameter _para;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First();
         var dimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Height", "m");
         _para = new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("a+b").WithDimension(dimension))
            .WithDimension(dimension);
         var dimension1 = new Dimension(new BaseDimensionRepresentation {LengthExponent = 0}, "Height", "m");
         var a = new Parameter().WithName("b").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("1").WithDimension(dimension1))
            .WithDimension(dimension1);
         var b = new Parameter().WithName("a").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("2").WithDimension(dimension1))
            .WithDimension(dimension1);
         _para.Formula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, a).WithDimension(dimension1));
         _para.Formula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, b).WithDimension(dimension1));
         _para.Formula.ResolveObjectPathsFor(_para);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid_with_warnings()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }

      [Observation]
      public void should_have_the_right_messages()
      {
         _result.Messages.Single().Text.ShouldBeEqualTo(AppConstants.Validation.FormulaDimensionMismatch($"thename|{_pathFactory.CreateAbsoluteObjectPath(_para).PathAsString}", _para.Dimension.Name));
      }
   }

   public class When_validating_a_Formula_using_object_dimension_and_formula_with_miss_matching_dimensions : concern_for_DimensionValidator
   {
      private IContainer _root;
      private IParameter _para;
      private string _formulaString;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First();
         var dimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Height", "m");
         _formulaString = "a+b";
         _para = new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString(_formulaString).WithDimension(dimension))
            .WithDimension(dimension);
         var a = new Parameter().WithName("b").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("1").WithDimension(dimension))
            .WithDimension(dimension);
         var dimension1 = new Dimension(new BaseDimensionRepresentation {LengthExponent = 0}, "Height", "m");
         var b = new Parameter().WithName("a").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("2").WithDimension(dimension1))
            .WithDimension(dimension1);

         _para.Formula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, a));
         _para.Formula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, b));
         _para.Formula.ResolveObjectPathsFor(_para);

         _userSettings.ShowCannotCalcErrors = true;

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid_with_warnings()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }

      [Observation]
      public void should_have_the_correct_messages()
      {
         _result.Messages.Single().Text.ShouldBeEqualTo($"Arguments of PLUS-function must have the same dimension (Formula: {_formulaString})");
      }
   }

   public class When_validating_a_parameter_dimension_and_formula_with_matching_dimensions_and_matching_RHS : concern_for_DimensionValidator
   {
      private IContainer _root;
      private IParameter _para;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = new Container().WithName("Root");
         var dimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Height", "m");
         var rhsDimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1, TimeExponent = -1}, "Height per Time", "m");

         _para = new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("a").WithDimension(dimension))
            .WithRHS(new ExplicitFormula().WithFormulaString("b").WithDimension(rhsDimension))
            .WithDimension(dimension);

         var a = new Parameter().WithName("a").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("1").WithDimension(dimension))
            .WithDimension(dimension);

         var b = new Parameter().WithName("b").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("2").WithDimension(rhsDimension))
            .WithDimension(rhsDimension);

         _para.Formula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, a).WithDimension(dimension));
         _para.RHSFormula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, b).WithDimension(rhsDimension));
         _para.Formula.ResolveObjectPathsFor(_para);
         _para.RHSFormula.ResolveObjectPathsFor(_para);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_root, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_valid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.Valid);
      }
   }

   public class When_validating_a_parameter_dimension_and_formula_with_matching_dimensions_and_missmatching_RHS : concern_for_DimensionValidator
   {
      private IContainer _root;
      private IParameter _para;
      private ValidationResult _result;
      private CreationResult _creationResult;

      protected override void Context()
      {
         base.Context();
         _root = _buildConfiguration.All<SpatialStructure>().First().TopContainers.First(); ;
         var dimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Height", "m");
         var rhsdimension = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1, TimeExponent = -1}, "Height per Time", "m");
         var rhsdimension1 = new Dimension(new BaseDimensionRepresentation {LengthExponent = 2, TimeExponent = -1}, "Height per Time", "m");

         _para = new Parameter().WithName("Parameter").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("a").WithDimension(dimension))
            .WithRHS(new ExplicitFormula().WithFormulaString("b").WithDimension(rhsdimension))
            .WithDimension(dimension);

         var a = new Parameter().WithName("a").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("1").WithDimension(dimension))
            .WithDimension(dimension);

         var b = new Parameter().WithName("b").WithParentContainer(_root)
            .WithFormula(new ExplicitFormula().WithFormulaString("2").WithDimension(rhsdimension1))
            .WithDimension(rhsdimension1);

         _para.Formula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, a).WithDimension(dimension));
         _para.RHSFormula.AddObjectPath(_pathFactory.CreateRelativeFormulaUsablePath(_para, b).WithDimension(rhsdimension1));
         _para.Formula.ResolveObjectPathsFor(_para);
         _para.RHSFormula.ResolveObjectPathsFor(_para);

         _creationResult = DomainFactoryForSpecs.CreateModelFor(_buildConfiguration, "thename");
      }

      protected override void Because()
      {
         _result = sut.Validate(_creationResult.Model, _creationResult.SimulationBuilder).Result;
      }

      [Observation]
      public void should_return_invalid()
      {
         _result.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }

      [Observation]
      public void should_have_the_right_messages()
      {
         _result.Messages.Single().Text.ShouldBeEqualTo(AppConstants.Validation.FormulaDimensionMismatch($"thename|{_pathFactory.CreateAbsoluteObjectPath(_para).PathAsString}", _para.RHSFormula.Dimension.Name));
      }
   }
}