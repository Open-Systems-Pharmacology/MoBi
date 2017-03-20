using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTaskForSimualtionTask : ContextSpecification<IEditTaskForSimulation>
   {
      protected IInteractionTaskContext _interactionTaskContext;
      protected IParameterIdentificationSimulationPathUpdater _parameterIdentificationSimulationPathUpdater;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _parameterIdentificationSimulationPathUpdater = A.Fake<IParameterIdentificationSimulationPathUpdater>();
         sut = new EditTaskForSimulation(_interactionTaskContext, _parameterIdentificationSimulationPathUpdater);
      }
   }

   internal class When_renaming_a_simulation : concern_for_EditTaskForSimualtionTask
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithName("Root");
         _simulation.HasChanged = false;
      }

      protected override void Because()
      {
         sut.Rename(_simulation, Enumerable.Empty<IObjectBase>(), null);
      }

      [Observation]
      public void should_call_the_parameter_identification_updater_to_change_update_parameter_identifications()
      {
         A.CallTo(() => _parameterIdentificationSimulationPathUpdater.UpdatePathsForRenamedSimulation(_simulation, "Root", A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void should_set_simualation_to_changed()
      {
         _simulation.HasChanged.ShouldBeTrue();
      }
   }
}