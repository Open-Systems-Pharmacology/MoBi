using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public class concern_for_PathAndValueEntityDTO : ContextSpecification<PathAndValueEntityDTO<IndividualParameter, IndividualParameterDTO>>
   {
      protected Unit _displayUnit;

      protected override void Context()
      {
         sut = new IndividualParameterDTO(new IndividualParameter { DistributionType = DistributionType.Discrete });
         _displayUnit = new Unit("the unit", 1.0, 0);
      }
   }

   public class When_getting_the_value_and_display_unit_of_a_distributed_path_and_value_entity_dto_with_mean : concern_for_PathAndValueEntityDTO
   {
      protected override void Because()
      {
         sut.AddSubParameter(new IndividualParameterDTO(new IndividualParameter { Value = 5.0 }.WithName(Constants.Distribution.MEAN).WithDisplayUnit(_displayUnit)));
      }

      [Observation]
      public void should_return_the_unit_of_the_mean_parameter()
      {
         sut.DisplayUnit.ShouldBeEqualTo(_displayUnit);
      }
      [Observation]
      public void should_return_the_value_of_the_mean_parameter()
      {
         sut.Value.ShouldBeEqualTo(5.0);
      }
   }

   public class When_getting_the_value_and_display_unit_of_a_distributed_path_and_value_entity_dto_without_mean : concern_for_PathAndValueEntityDTO
   {
      [Observation]
      public void should_return_the_unit_of_the_mean_parameter()
      {
         sut.DisplayUnit.ShouldBeNull();
      }
      [Observation]
      public void should_return_the_value_of_the_mean_parameter()
      {
         sut.Value.ShouldBeNull();
      }
   }
}
