using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.ProjectConversion.v9
{
   public class When_converting_a_project_to_9_0 : ContextWithLoadedProject
   {
      private MoBiProject _project;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _project = LoadProject("EB_LV");
      }

      [Observation]
      public void should_have_loaded_the_project_as_expected()
      {
         _project.ShouldNotBeNull();
      }
   }
}