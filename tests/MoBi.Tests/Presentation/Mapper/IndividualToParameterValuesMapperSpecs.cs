using System;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Mapper
{
   public class concern_for_IndividualToParameterValuesMapper : ContextSpecification<IndividualToParameterValuesMapper>
   {
      private IObjectBaseFactory _objectFactory;
      private ICloneManagerForModel _cloneManager;
      private PathAndValueEntityToParameterValueMapper _individualParameterToParameterValueMapper;

      protected override void Context()
      {
         _objectFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _objectFactory.Create<ParameterValue>()).ReturnsLazily(x => new ParameterValue());
         A.CallTo(() => _objectFactory.Create<ParameterValuesBuildingBlock>()).ReturnsLazily(x => new ParameterValuesBuildingBlock());
         A.CallTo(() => _objectFactory.CreateObjectBaseFrom(A<IFormula>._)).ReturnsLazily(x => new ExplicitFormula().WithId(Guid.NewGuid().ToString()));

         _cloneManager = new CloneManagerForModel(_objectFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());

         _individualParameterToParameterValueMapper = new PathAndValueEntityToParameterValueMapper(_objectFactory, _cloneManager);
         sut = new IndividualToParameterValuesMapper(_individualParameterToParameterValueMapper, _objectFactory);
      }

      public class mapping_from_individual_to_values_building_block : concern_for_IndividualToParameterValuesMapper
      {
         private IndividualBuildingBlock _individual;
         private ParameterValuesBuildingBlock _result;

         protected override void Context()
         {
            base.Context();
            _individual = new IndividualBuildingBlock().WithName("BB Name");
            var individualParameter = createIndividualParameter("parameter1");
            addIndividualParameter(individualParameter);

            var formula1 = individualParameter.Formula;

            individualParameter = createIndividualParameter("parameter2");
            addIndividualParameter(individualParameter);

            individualParameter = createIndividualParameter("parameter3");
            individualParameter.Formula = formula1;

            // Substitute the formula for an existing one to make sure formulae are unique by name
            addIndividualParameter(individualParameter);
         }

         private void addIndividualParameter(IndividualParameter individualParameter)
         {
            _individual.Add(individualParameter);
            if (individualParameter.Formula != null)
               _individual.AddFormula(individualParameter.Formula);
         }

         private static IndividualParameter createIndividualParameter(string parameterName)
         {
            var individualParameter = new IndividualParameter().WithName(parameterName);
            individualParameter.Formula = new ExplicitFormula("X").WithName($"Formula for {parameterName}").WithId($"Id for {parameterName}");

            return individualParameter;
         }

         protected override void Because()
         {
            _result = sut.MapFrom(_individual);
         }

         [Observation]
         public void should_map_building_block_name()
         {
            _result.Name.ShouldBeEqualTo(_individual.Name);
         }

         [Observation]
         public void the_formula_cache_should_only_contain_a_single_instance_of_a_formula_with_name()
         {
            _result.FormulaCache.Count.ShouldBeEqualTo(2);
            _result.FormulaCache.Distinct(new NameComparer<IFormula>()).Count().ShouldBeEqualTo(_result.FormulaCache.Count);
         }
      }
   }
}