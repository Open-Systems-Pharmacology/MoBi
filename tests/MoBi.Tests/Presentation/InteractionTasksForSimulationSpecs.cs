using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForSimulation : ContextSpecification<InteractionTasksForSimulation>
   {
      protected IEditTasksForSimulation _editTask;
      protected IInteractionTaskContext _context;
      protected IMoBiApplicationController _applicationController;
      protected IDialogCreator _dialogCreator;
      protected ISimulationReferenceUpdater _simulationReferenceUpdater;
      protected ISimulationFactory _simulationFactory;
      protected IMoBiContext _moBiContext;
      protected IndividualBuildingBlock _individualBuildingBlock;
      protected MoBiProject _moBiProject;
      private ITemplateResolverTask _templateResolver;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _editTask = A.Fake<IEditTasksForSimulation>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _dialogCreator = A.Fake<IDialogCreator>();
         A.CallTo(() => _context.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _context.DialogCreator).Returns(_dialogCreator);
         _simulationReferenceUpdater = A.Fake<ISimulationReferenceUpdater>();
         _simulationFactory = A.Fake<ISimulationFactory>();
         _moBiContext = A.Fake<IMoBiContext>();
         _moBiProject = new MoBiProject();
         _individualBuildingBlock = new IndividualBuildingBlock().WithName("common individual");
         _moBiProject.AddIndividualBuildingBlock(_individualBuildingBlock);
         A.CallTo(() => _moBiContext.CurrentProject).Returns(_moBiProject);
         A.CallTo(() => _context.Context).Returns(_moBiContext);
         _templateResolver = A.Fake<ITemplateResolverTask>();

         sut = new InteractionTasksForSimulation(_context, _editTask, _simulationReferenceUpdater, _simulationFactory, _templateResolver);
      }
   }

   public class When_removing_multiple_simulations_from_project_but_canceling : concern_for_InteractionTasksForSimulation
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

   public class When_removing_multiple_simulations_from_project : concern_for_InteractionTasksForSimulation
   {
      private IReadOnlyList<IMoBiSimulation> _simulations;
      private MoBiMacroCommand _result;

      protected override void Context()
      {
         base.Context();
         _simulations = new List<IMoBiSimulation> { A.Fake<IMoBiSimulation>(), A.Fake<IMoBiSimulation>() };
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

   internal class When_adding_a_new_simulation : concern_for_InteractionTasksForSimulation
   {
      private ICreateSimulationConfigurationPresenter _presenter;
      private IMoBiSimulation _simulation;
      private SimulationConfiguration _simulationConfiguration;
      private IMoBiSimulation _configuredSimulation;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<ICreateSimulationConfigurationPresenter>();
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration();
         _configuredSimulation = new MoBiSimulation();
         _simulationConfiguration = new SimulationConfiguration();
         A.CallTo(() => _presenter.CreateBasedOn(_simulation, true)).Returns(_simulationConfiguration);
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).Returns(_presenter);
         A.CallTo(() => _applicationController.PresenterFor(_simulation)).Returns(A.Fake<IEditSimulationPresenter>());
         A.CallTo(() => _simulationFactory.Create()).Returns(_simulation);
         A.CallTo(() => _simulationFactory.CreateSimulationAndValidate(_simulationConfiguration, A<string>._)).Returns(_configuredSimulation);
      }

      protected override void Because()
      {
         sut.CreateSimulation();
      }

      [Observation]
      public void the_configuration_should_use_the_default_individual_for_the_project()
      {
         _simulation.Configuration.Individual.ShouldBeEqualTo(_individualBuildingBlock);
      }

      [Observation]
      public void should_get_create_presenter_from_application_controller()
      {
         A.CallTo(() => _applicationController.Start<ICreateSimulationConfigurationPresenter>()).MustHaveHappened();
      }

      [Observation]
      public void should_start_edit_action_for_the_new_simulation()
      {
         A.CallTo(() => _editTask.Edit(_configuredSimulation)).MustHaveHappened();
      }
   }
}