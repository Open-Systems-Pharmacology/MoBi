using System;
using FakeItEasy;
using MoBi.Core.Extensions;
using MoBi.Helpers;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_ExpressionParameterToParameterStartValueMapper : ContextSpecification<ExpressionParameterToParameterStartValueMapper>
   {
      private IObjectBaseFactory _objectFactory;
      private CloneManagerForModel _cloneManager;

      protected override void Context()
      {
         _objectFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectFactory.Create<ParameterStartValue>()).ReturnsLazily(x => new ParameterStartValue());
         A.CallTo(() => _objectFactory.Create<ParameterStartValuesBuildingBlock>()).ReturnsLazily(x => new ParameterStartValuesBuildingBlock());
         A.CallTo(() => _objectFactory.CreateObjectBaseFrom(A<IFormula>._)).ReturnsLazily(x => new ExplicitFormula().WithId(Guid.NewGuid().ToString()));

         _cloneManager = new CloneManagerForModel(_objectFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());

         sut = new ExpressionParameterToParameterStartValueMapper(_objectFactory, _cloneManager);
      }
   }

   public class When_mapping_expression_parameters_to_parameter_start_values : concern_for_ExpressionParameterToParameterStartValueMapper
   {
      private ExpressionParameter _expressionParameter;
      private ParameterStartValue _result;

      protected override void Context()
      {
         base.Context();
         var dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Concentration);
         var pathElements = new[] { "The", "Path", "Entries", "Name" };
         var path = new ObjectPath();
         var theName = "TheName";
         _expressionParameter = new ExpressionParameter
         {
            ContainerPath = pathElements.ContainerPath(),
            Description = "Description",
            Dimension = dimension,
            DisplayUnit = dimension.DefaultUnit,
            Formula = new ExplicitFormula("X"),
            Icon = string.Empty,
            Name = theName,
            Id = "TheId",
            ParentContainer = new Container(),
            Path = path
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_expressionParameter);
      }

      [Observation]
      public void all_properties_should_be_mapped()
      {
         _expressionParameter.Name.ShouldBeEqualTo(_result.Name);
         _expressionParameter.ContainerPath.PathAsString.ShouldBeEqualTo(_result.ContainerPath.PathAsString);
         _expressionParameter.Description.ShouldBeEqualTo(_result.Description);
         _expressionParameter.DimensionName().ShouldBeEqualTo(_result.DimensionName());
         _expressionParameter.Formula.Name.ShouldBeEqualTo(_result.Formula.Name);
         _expressionParameter.Path.PathAsString.ShouldBeEqualTo(_result.Path.PathAsString);
         _expressionParameter.Value.ShouldBeEqualTo(_result.Value);
      }
   }
}