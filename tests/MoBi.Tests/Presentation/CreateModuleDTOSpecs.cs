using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_CreateModuleDTO : ContextSpecification<CreateModuleDTO>
   {
      protected override void Context()
      {
         sut = new CreateModuleDTO();
      }
   }

   public class When_validating_dto_with_empty_name : concern_for_CreateModuleDTO
   {
      protected override void Because()
      {
         sut.Name = string.Empty;
      }

      [Observation]
      public void the_dto_should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_validating_the_dto_with_forbidden_name : concern_for_CreateModuleDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.AddUsedNames(new[] { "name" });
      }

      protected override void Because()
      {
         // case and whitespace variation
         sut.Name = "namE ";
      }

      [Observation]
      public void the_dto_should_be_invalid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
