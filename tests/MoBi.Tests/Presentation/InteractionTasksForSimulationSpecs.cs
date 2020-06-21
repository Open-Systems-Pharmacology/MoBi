using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForSimulationSpecs : ContextSpecification<IInteractionTasksForSimulation>
   {
      protected IEditTasksForSimulation _editTask;
      private IInteractionTaskContext _context;
      protected IMoBiApplicationController _applicationController;
      protected IDialogCreator _dialogCreator;
      protected ISimulationReferenceUpdater _simulationReferenceUpdater;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTasksForSimulation>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _context.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _context.DialogCreator).Returns(_dialogCreator);
         _simulationReferenceUpdater = A.Fake<ISimulationReferenceUpdater>();
         sut = new InteractionTasksForSimulation(_context, _editTask, _simulationReferenceUpdater);
      }
   }

   public class When_removing_multiple_simulations_from_project_but_canceling : concern_for_InteractionTasksForSimulationSpecs
   {
      private IReadOnlyList<IMoBiSimulation> _simulations;
      private IMoBiCommand _result;

      protected override void Context()
      {
         base.Context();
         _simulations = new List<IMoBiSimulation> { A.Fake<IMoBiSimulation>(), A.Fake<IMoBiSimulation>() };
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         _result = sut.RemoveMultipleSimulations(_simulations);
      }

      [Observation]
      public void must_not_remove_simulations_from_parameter_identifications()
      {
         A.CallTo(() => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(A<ISimulation>._)).MustNotHaveHappened();
      }

      [Observation]
      public void returns_the_correct_number_of_commands()
      {
         _result.IsEmpty().ShouldBeTrue();
      }
   }

   public class When_removing_multiple_simulations_from_project : concern_for_InteractionTasksForSimulationSpecs
   {
      private IReadOnlyList<IMoBiSimulation> _simulations;
      private MoBiMacroCommand _result;

      protected override void Context()
      {
         base.Context();
         _simulations = new List<IMoBiSimulation> {A.Fake<IMoBiSimulation>(), A.Fake<IMoBiSimulation>()};
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         _result = sut.RemoveMultipleSimulations(_simulations) as MoBiMacroCommand;
      }

      [Observation]
      public void should_remove_all_simulations_from_parameter_identifications()
      {
         A.CallTo(() => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(_simulations[0])).MustHaveHappened();
         A.CallTo(() => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(_simulations[1])).MustHaveHappened();
      }

      [Observation]
      public void the_commands_should_be_remove_simulation_type()
      {
         _result.All().Each(command => command.ShouldBeAnInstanceOf<RemoveSimulationCommand>());
      }

      [Observation]
      public void returns_the_correct_number_of_commands()
      {
         _result.Count.ShouldBeEqualTo(_simulations.Count);
      }
   }

   internal class When_adding_a_new_simulation : concern_for_InteractionTasksForSimulationSpecs
   {
      private ICreateSimulationPresenter _presenter;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateSimulationPresenter>();
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _presenter.Create()).Returns(_simulation);
         A.CallTo(() => _applicationController.Start<ICreateSimulationPresenter>()).Returns(_presenter);
         A.CallTo(() => _applicationController.PresenterFor(_simulation)).Returns(A.Fake<IEditSimulationPresenter>());
      }

      protected override void Because()
      {
         sut.CreateSimulation();
      }

      [Observation]
      public void should_get_create_presenter_from_application_controller()
      {
         A.CallTo(() => _applicationController.Start<ICreateSimulationPresenter>()).MustHaveHappened();
      }

      [Observation]
      public void should_start_edit_action_for_the_new_simulaation()
      {
         A.CallTo(() => _editTask.Edit(_simulation)).MustHaveHappened();
      }
   }
}