using System;
using System.IO;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests
{
   public class LoadProjectIntegrationTest : ContextForIntegration<IProjectTask>
   {
      private IMoBiContext _context;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _context = IoC.Resolve<IMoBiContext>();
         sut = IoC.Resolve<IProjectTask>();
         sut.OpenProjectFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "TestFiles", "PK_Manual_Diclofenac.mbp3"));
      }

      [Observation]
      public void should_be_able_to_load_Diclofenac_model()
      {
         _context.CurrentProject.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_set_the_name_of_the_project_to_the_name_of_the_file()
      {
         _context.CurrentProject.Name.ShouldBeEqualTo("PK_Manual_Diclofenac");
      }
   }
}