using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTasksForSimulation : ContextSpecification<IEditTasksForSimulation>
   {
      protected IInteractionTaskContext _context;
      protected ISimulationPersistor _simulationPersistor;
      protected IDialogCreator _dialogCreator;
      private IDataRepositoryExportTask _dataRepositoryTask;
      private ISimModelExporter _simulationModelExporter;
      private IModelReportCreator _reportCreator;
      private IDimensionFactory _dimensionFactory;
      protected IParameterIdentificationSimulationPathUpdater _parameterIdentificationSimulationPathUpdater;
      protected IMoBiApplicationController _applicationController;
      protected MoBiProject _project;

      protected override void Context()
      {
         _project = new MoBiProject();
         _context = A.Fake<IInteractionTaskContext>();
         _simulationPersistor = A.Fake<ISimulationPersistor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _simulationModelExporter = A.Fake<ISimModelExporter>();
         _reportCreator = A.Fake<IModelReportCreator>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterIdentificationSimulationPathUpdater = A.Fake<IParameterIdentificationSimulationPathUpdater>();
         _applicationController = A.Fake<IMoBiApplicationController>();

         A.CallTo(() => _context.ApplicationController).Returns(_applicationController);
         A.CallTo(() => _context.Context.CurrentProject).Returns(_project);
         
         sut = new EditTasksForSimulation(_context, _simulationPersistor, _dialogCreator, _dataRepositoryTask, _reportCreator, _simulationModelExporter, _dimensionFactory, _parameterIdentificationSimulationPathUpdater);
      }
   }

   public class When_editing_a_simulation : concern_for_EditTasksForSimulation
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void should_register_the_simulation()
      {
         A.CallTo(() => _context.Context.Register(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void project_should_not_have_changed()
      {
         _project.HasChanged.ShouldBeFalse();
      }
   }

   public class When_saving_a_simulation_to_pkml : concern_for_EditTasksForSimulation
   {
      private IMoBiSimulation _simulation;
      private string _filePath;
      private OriginalQuantityValue _originalQuantityValue;
      private SimulationTransfer _savedSimulationTransfer;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithName("Sim");
         _originalQuantityValue = new OriginalQuantityValue { Path = "Organism|Volume", Value = 1, Type = OriginalQuantityValue.Types.Quantity };
         _simulation.AddOriginalQuantityValue(_originalQuantityValue);
         _simulation.HasUntraceableChanges = true;
         _filePath = "C:\\temp\\Simulation_Sim.pkml";
         A.CallTo(() => _dialogCreator.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._)).Returns(_filePath);
         A.CallTo(() => _simulationPersistor.Save(A<SimulationTransfer>._, _filePath))
            .Invokes(x => _savedSimulationTransfer = x.GetArgument<SimulationTransfer>(0));
      }

      protected override void Because()
      {
         sut.Save(_simulation);
      }

      [Observation]
      public void should_save_the_simulation_to_the_selected_file()
      {
         _savedSimulationTransfer.Simulation.ShouldBeEqualTo(_simulation);
      }

      [Observation]
      public void should_save_the_changes_tracked_in_the_simulation()
      {
         _savedSimulationTransfer.OriginalQuantityValues.ShouldOnlyContain(_originalQuantityValue);
         _savedSimulationTransfer.HasUntraceableChanges.ShouldBeTrue();
      }
   }

   public class When_saving_a_simulation_to_pkml_and_the_user_cancels : concern_for_EditTasksForSimulation
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithName("Sim");
         A.CallTo(() => _dialogCreator.AskForFileToSave(A<string>._, A<string>._, A<string>._, A<string>._, A<string>._)).Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.Save(_simulation);
      }

      [Observation]
      public void should_not_save_the_simulation()
      {
         A.CallTo(() => _simulationPersistor.Save(A<SimulationTransfer>._, A<string>._)).MustNotHaveHappened();
      }
   }

   internal class When_renaming_a_simulation : concern_for_EditTasksForSimulation
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithName("Root");
         _simulation.HasChanged = false;
         
         A.CallTo(() => _context.NamingTask.RenameFor(_simulation, A<IReadOnlyList<string>>._))
            .Invokes(x => _simulation.Name = "NEW_NAME");
      }

      protected override void Because()
      {
         sut.Rename(_simulation, Enumerable.Empty<IObjectBase>(), null);
      }

      [Observation]
      public void should_call_the_parameter_identification_updater_to_change_update_parameter_identifications()
      {
         A.CallTo(() => _parameterIdentificationSimulationPathUpdater.UpdatePathsForRenamedSimulation(_simulation, "Root", _simulation.Name)).MustHaveHappened();
      }

      [Observation]
      public void should_set_simulation_to_changed()
      {
         _simulation.HasChanged.ShouldBeTrue();
      }
   }
}