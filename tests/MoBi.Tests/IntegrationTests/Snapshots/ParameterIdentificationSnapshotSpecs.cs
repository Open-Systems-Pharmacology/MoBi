using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.IntegrationTests.Snapshots
{
   public class When_loading_a_snapshot_with_a_parameter_identification_referencing_a_simulation : ContextWithLoadedSnapshot
   {
      public override void GlobalContext()
      {
         base.GlobalContext();

         var validationTask = IoC.Resolve<IEntityValidationTask>();
         A.CallTo(() => validationTask.Validate(A<MoBiSimulation>._)).Returns(true);

         LoadSnapshot("snapshot_no_pksim_modules");
      }

      [Observation]
      public void the_simulation_is_loaded()
      {
         _project.Simulations.Count.ShouldBeEqualTo(1);
         _project.Simulations[0].Name.ShouldBeEqualTo("test");
      }

      [Observation]
      public void the_parameter_identification_retains_its_simulation()
      {
         var parameterIdentification = _project.AllParameterIdentifications.Single();
         parameterIdentification.AllSimulations.Select(x => x.Name).ShouldContain("test");
      }

      [Observation]
      public void the_identification_parameter_is_linked_to_the_simulation_parameter()
      {
         var parameterIdentification = _project.AllParameterIdentifications.Single();
         parameterIdentification.AllIdentificationParameters.Count.ShouldBeEqualTo(1);
      }
   }
}
