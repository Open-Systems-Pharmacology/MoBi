using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_IndividualAndFilePathDTO : ContextSpecification<IndividualAndFilePathDTO>
   {
      protected override void Context()
      {
         sut = new IndividualAndFilePathDTO();
      }
   }

   public class When_the_dto_has_path_and_individual : concern_for_IndividualAndFilePathDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.FilePath = "filepath";
         sut.IndividualBuildingBlock = new IndividualBuildingBlock();
      }

      [Observation]
      public void the_validation_fails()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_the_dto_does_not_have_individual : concern_for_IndividualAndFilePathDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.FilePath = "filepath";
      }

      [Observation]
      public void the_validation_fails()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_the_dto_does_not_have_filepath : concern_for_IndividualAndFilePathDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.IndividualBuildingBlock = new IndividualBuildingBlock();
      }

      [Observation]
      public void the_validation_fails()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
