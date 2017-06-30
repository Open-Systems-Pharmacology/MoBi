using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.SimModel.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_EditTasksForSimulation : ContextSpecification<IEditTasksForSimulation>
   {
      protected IInteractionTaskContext _context;
      private ISimulationPersistor _simulationPersitor;
      private IDialogCreator _dialogCreator;
      private IForbiddenNamesRetriever _forbiddenNamesRetriever;
      private IDataRepositoryTask _dataRepositoryTask;
      private ISimModelExporter _simulationModelExporter;
      private IModelReportCreator _reportCreator;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _context = A.Fake<IInteractionTaskContext>();
         _simulationPersitor = A.Fake<ISimulationPersistor>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _forbiddenNamesRetriever = A.Fake<IForbiddenNamesRetriever>();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _simulationModelExporter = A.Fake<ISimModelExporter>();
         _reportCreator = A.Fake<IModelReportCreator>();
         _dimensionFactory= A.Fake<IDimensionFactory>();
         sut = new EditTasksForSimulation(_context, _simulationPersitor, _dialogCreator, _forbiddenNamesRetriever, _dataRepositoryTask, _reportCreator, _simulationModelExporter, _dimensionFactory);
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
}