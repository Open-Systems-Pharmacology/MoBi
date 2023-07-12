using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
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
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTasksForSimulation : ContextSpecification<IEditTasksForSimulation>
   {
      protected IInteractionTaskContext _context;
      private ISimulationPersistor _simulationPersistor;
      private IDialogCreator _dialogCreator;
      private IDataRepositoryExportTask _dataRepositoryTask;
      private ISimModelExporter _simulationModelExporter;
      private IModelReportCreator _reportCreator;
      private IDimensionFactory _dimensionFactory;
      protected IParameterIdentificationSimulationPathUpdater _parameterIdentificationSimulationPathUpdater;
      protected IMoBiApplicationController _applicationController;

      protected override void Context()
      {
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
   }

   internal class When_renaming_a_simulation : concern_for_EditTasksForSimulation
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithName("Root");
         _simulation.HasChanged = false;
         
         A.CallTo(() => _context.RenameFor(_simulation, A<IReadOnlyList<string>>._))
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