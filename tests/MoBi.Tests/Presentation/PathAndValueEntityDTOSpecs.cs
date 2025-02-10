using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation
{
   public abstract class concern_for_PathAndValueEntityDTO : ContextSpecification<PathAndValueEntityDTO<IndividualParameter, IndividualParameterDTO>>
   {
      protected Unit _displayUnit;

      protected override void Context()
      {
         sut = new IndividualParameterDTO(GetDTO());
         sut.DistributionValue = 0.4;
      }

      protected abstract IndividualParameter GetDTO();
   }

   public class When_getting_the_value_and_display_unit_of_a_distributed_path_and_value_entity_dto : concern_for_PathAndValueEntityDTO
   {
      protected override IndividualParameter GetDTO()
      {
         return new IndividualParameter { DistributionType = DistributionType.Discrete, Value = 5.0 };
      }

      [Observation]
      public void should_return_the_value_of_the_distribution()
      {
         sut.Value.ShouldBeEqualTo(0.4);
      }
   }

   public class When_getting_the_value_and_display_unit_of_a_non_distributed_path_and_value_entity_dto : concern_for_PathAndValueEntityDTO
   {
      protected override IndividualParameter GetDTO()
      {
         return new IndividualParameter { Value = 5.0 };
      }

      [Observation]
      public void should_return_the_value_of_the_parameter()
      {
         sut.Value.ShouldBeEqualTo(5.0);
      }
   }
}
