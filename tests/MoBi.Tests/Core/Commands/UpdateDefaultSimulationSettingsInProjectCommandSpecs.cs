using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_UpdateDefaultSimulationSettingsInProjectCommand : ContextSpecification<UpdateDefaultSimulationSettingsInProjectCommand>
   {
      protected SimulationSettings _simulationSettings, _newSimulationSettings;
      protected IMoBiContext _context;
      protected MoBiProject _moBiProject;
      private readonly byte[] _oldBytes = new byte[1];
      private readonly byte[] _newBytes = new byte[1];

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _moBiProject = DomainHelperForSpecs.NewProject();
         A.CallTo(() => _context.CurrentProject).Returns(_moBiProject);

         _simulationSettings = new SimulationSettings();
         _newSimulationSettings = new SimulationSettings();
         _moBiProject.SimulationSettings = _simulationSettings;

         A.CallTo(() => _context.Serialize(_simulationSettings)).Returns(_oldBytes);
         A.CallTo(() => _context.Deserialize<SimulationSettings>(A<byte[]>.That.Matches(x => x.Equals(_oldBytes)))).Returns(_simulationSettings);

         A.CallTo(() => _context.Serialize(_newSimulationSettings)).Returns(_newBytes);
         A.CallTo(() => _context.Deserialize<SimulationSettings>(A<byte[]>.That.Matches(x => x.Equals(_newBytes)))).Returns(_newSimulationSettings);

         sut = new UpdateDefaultSimulationSettingsInProjectCommand(_newSimulationSettings);
      }
   }

   public class When_updating_the_default_simulation_settings_in_a_project : concern_for_UpdateDefaultSimulationSettingsInProjectCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_have_updated_the_default_simulation_settings_in_the_project()
      {
         _moBiProject.SimulationSettings.ShouldBeEqualTo(_newSimulationSettings);
      }
   }

   public class When_reverting_the_update_of_the_default_simulation_settings_in_a_project : concern_for_UpdateDefaultSimulationSettingsInProjectCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_have_updated_the_default_simulation_settings_in_the_project()
      {
         _moBiProject.SimulationSettings.ShouldBeEqualTo(_simulationSettings);
      }
   }
}