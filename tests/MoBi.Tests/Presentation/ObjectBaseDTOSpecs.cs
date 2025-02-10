using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_ObjectBaseDTO : ContextSpecification<ObjectBaseDTO>
   {
      protected override void Context()
      {
         sut = new ObjectBaseDTO();
      }
   }

   public class when_object_name_is_events_the_object_should_be_valid : concern_for_ObjectBaseDTO
   {
      protected override void Because()
      {
         sut.Name = "events";
      }

      [Observation]
      public void the_object_should_be_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }
}