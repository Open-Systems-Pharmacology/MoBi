using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using System.Runtime.Remoting.Contexts;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateSimulationCommand : ContextSpecification<UpdateSimulationCommand>
   {
      protected IMoBiSimulation _simulation;
      protected IModel _model;
      protected SimulationConfiguration _simulationConfiguration;
      private IBuildingBlock _buildingBlock;
      protected SimulationConfiguration _oldBuildConfiguration;

      protected override void Context()
      {
         _simulation = A.Fake<IMoBiSimulation>();
         _model = A.Fake<IModel>();
         _simulationConfiguration = new SimulationConfiguration();
         _buildingBlock = A.Fake<IBuildingBlock>().WithName("toto");
         _oldBuildConfiguration = _simulation.Configuration;
         _simulation.HasUntraceableChanges = true;
         sut = new UpdateSimulationCommand(_simulation, _model, _simulationConfiguration, _buildingBlock);
      }
   }

   internal class When_reversing_an_update_simulation_command : concern_for_UpdateSimulationCommand
   {
      private IMoBiContext _context;
      private SimulationUnloadEvent _event;
      private IProject _project;
      private ParameterIdentification _parameterIdentification;
      private ISimulationReferenceUpdater _simulationReferenceUpdater;
      private ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IProject>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _context = A.Fake<IMoBiContext>();
         _simulationReferenceUpdater = A.Fake<ISimulationReferenceUpdater>();
         _simulationParameterOriginIdUpdater = A.Fake<ISimulationParameterOriginIdUpdater>();

         A.CallTo(() => _context.PublishEvent(A<SimulationUnloadEvent>._))
            .Invokes(x => _event = x.GetArgument<SimulationUnloadEvent>(0));

         A.CallTo(() => _context.Project).Returns(_project);
         A.CallTo(() => _context.Resolve<ISimulationReferenceUpdater>()).Returns(_simulationReferenceUpdater);
         A.CallTo(() => _context.Resolve<ISimulationParameterOriginIdUpdater>()).Returns(_simulationParameterOriginIdUpdater);
         A.CallTo(() => _project.AllParameterIdentifications).Returns(new[] { _parameterIdentification });
         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
         _parameterIdentification.AddSimulation(_simulation);
         
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_untraceable_changes_should_be_restored()
      {
         _simulation.HasUntraceableChanges.ShouldBeTrue();
      }
   }

   internal class When_executing_an_update_simulation_command : concern_for_UpdateSimulationCommand
   {
      private IMoBiContext _context;
      private SimulationUnloadEvent _event;
      private IProject _project;
      private ParameterIdentification _parameterIdentification;
      private ISimulationReferenceUpdater _simulationReferenceUpdater;
      private ISimulationParameterOriginIdUpdater _simulationParameterOriginIdUpdater;

      protected override void Context()
      {
         base.Context();
         _project = A.Fake<IProject>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _context = A.Fake<IMoBiContext>();
         _simulationReferenceUpdater = A.Fake<ISimulationReferenceUpdater>();
         _simulationParameterOriginIdUpdater= A.Fake<ISimulationParameterOriginIdUpdater>();

         A.CallTo(() => _context.PublishEvent(A<SimulationUnloadEvent>._))
            .Invokes(x => _event = x.GetArgument<SimulationUnloadEvent>(0));

         A.CallTo(() => _context.Project).Returns(_project);
         A.CallTo(() => _context.Resolve<ISimulationReferenceUpdater>()).Returns(_simulationReferenceUpdater);
         A.CallTo(() => _context.Resolve<ISimulationParameterOriginIdUpdater>()).Returns(_simulationParameterOriginIdUpdater);
         A.CallTo(() => _project.AllParameterIdentifications).Returns(new[] {_parameterIdentification});
         _parameterIdentification.AddSimulation(_simulation);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_untraceable_changes_should_be_removed()
      {
         _simulation.HasUntraceableChanges.ShouldBeFalse();
      }

      [Observation]
      public void should_replace_the_simulation_references_in_the_simulation_being_updated()
      {
         A.CallTo(() => _simulationReferenceUpdater.SwapSimulationInParameterAnalysables(_simulation, _simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_simulation_origin_id_in_all_parameters_of_the_simulation()
      {
         A.CallTo(() => _simulationParameterOriginIdUpdater.UpdateSimulationId(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_change_the_node_ids_in_diagram_model()
      {
         A.CallTo(() => _simulation.DiagramModel.ReplaceNodeIds(A<IDictionary<string, string>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_call_update_for_simulation()
      {
         A.CallTo(() => _simulation.Update(_simulationConfiguration, _model)).MustHaveHappened();
      }

      [Observation]
      public void should_serialize_old_values()
      {
         A.CallTo(() => _context.Serialize(_simulation.Configuration)).MustHaveHappened();
         A.CallTo(() => _context.Serialize(_simulation.Model)).MustHaveHappened();
      }

      [Observation]
      public void should_set_simulation_has_changed_flag()
      {
         _simulation.HasChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_update_object_registration()
      {
         A.CallTo(() => _context.Register(_simulation)).MustHaveHappened();
         A.CallTo(() => _context.UnregisterSimulation(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_notify_the_simulation_unloaded_event_before_updating_the_simulation()
      {
         _event.Simulation.Configuration.ShouldBeEqualTo(_oldBuildConfiguration);
      }
   }
}