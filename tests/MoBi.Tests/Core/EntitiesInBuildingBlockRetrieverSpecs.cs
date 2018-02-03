using MoBi.Core.Domain.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core
{
   public abstract class concern_for_EntitiesInBuildingBlockRetriever : ContextSpecification<IEntitiesInBuildingBlockRetriever<IParameter>>
   {
      protected IParameter _parameter1;
      protected IParameter _parameter2;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         sut = new EntitiesInBuildingBlockRetriever<IParameter>();
         var buildingBlock = new SpatialStructure();
         var topContainer = new Container().WithName("ROOT");
         _parameter1 = new Parameter().WithName("P1").WithFormula(new ConstantFormula(1));
         _parameter2 = new Parameter().WithName("P2").WithFormula(new ConstantFormula(2));
         topContainer.Add(_parameter1);
         topContainer.Add(_parameter2);
         buildingBlock.Add(topContainer);
         _buildingBlock = buildingBlock;
      }
   }

   public class When_retrieving_all_parameters_defined_in_a_building_block : concern_for_EntitiesInBuildingBlockRetriever
   {
      [Observation]
      public void should_return_all_parameters_in_the_given_building_block_if_no_criteria_is_given()
      {
         sut.AllFrom(_buildingBlock).ShouldOnlyContain(_parameter1, _parameter2);
      }

      [Observation]
      public void should_return_all_parameters_matching_the_criteria_in_the_given_building_block()
      {
         sut.AllFrom(_buildingBlock, x => x.Value == 2).ShouldOnlyContain(_parameter2);
      }
   }
}