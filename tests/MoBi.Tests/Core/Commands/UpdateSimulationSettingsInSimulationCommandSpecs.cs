using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_UpdateSimulationSettingsInSimulationCommand : ContextSpecification<UpdateSimulationSettingsInSimulationCommand>
   {
      protected IMoBiSimulation _simulation;
      protected SimulationSettings _newSimulationSettings;
      protected IMoBiContext _context;
      protected SimulationSettings _oldSimulationSettings;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _oldSimulationSettings = new SimulationSettings();
         _simulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration
            {
               SimulationSettings = _oldSimulationSettings
            },
            Id = "simulationId"
         };
         _newSimulationSettings = new SimulationSettings();
         sut = new UpdateSimulationSettingsInSimulationCommand(_simulation, _newSimulationSettings);
      }
   }

   public class When_updating_simulation_settings_in_a_simulation : concern_for_UpdateSimulationSettingsInSimulationCommand
   {
      protected override void Because()
      {
         sut.Run(_context);
      }

      [Observation]
      public void the_simulation_settings_are_updated_in_the_simulation()
      {
         _simulation.Configuration.SimulationSettings.ShouldBeEqualTo(_newSimulationSettings);
      }
   }

   public class When_reversing_the_update_of_simulation_settings_in_a_simulation : concern_for_UpdateSimulationSettingsInSimulationCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Deserialize<SimulationSettings>(A<byte[]>._)).Returns(_oldSimulationSettings);
         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_original_simulation_settings_are_restored()
      {
         _simulation.Configuration.SimulationSettings.ShouldBeEqualTo(_oldSimulationSettings);
      }
   }
}