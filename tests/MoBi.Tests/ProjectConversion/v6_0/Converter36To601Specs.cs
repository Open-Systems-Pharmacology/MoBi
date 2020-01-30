using System;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.ProjectConversion.v6_0
{
   public class When_converting_a_project_from_3_6_to_6_0 : ContextWithLoadedProject
   {
      [Observation]
      public void should_throw_an_exception_that_the_project_is_too_old()
      {
         The.Action(() => LoadProject("EB_LV")).ShouldThrowAn<Exception>();
      }
   }
}