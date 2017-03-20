using OSPSuite.BDDHelper;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Journal;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_ReloadRelatedItemTaskSpecs : ContextSpecification<IReloadRelatedItemTask>
   {
      protected IApplicationConfiguration _applicationConfiguration;
      protected IContentLoader _contentLoader;
      protected IDialogCreator _dialogCreator;
      protected IRelatedItemSerializer _relatedItemSerializer;
      protected IInteractionTasksForSimulation _simulationTask;
      protected IMoBiContext _context;
      protected ICloneManagerForBuildingBlock _cloneManager;
      protected IBuildingBlockTaskRetriever _taskRetriever;
      protected IPKSimExportTask _pkSimExportTask;
      protected ISimulationLoader _simulationLoader;
      protected RelatedItem _relatedItem;
      protected IObjectIdResetter _objectIdResetter;
      protected IObservedDataTask _observedDataTask;
      private IParameterIdentificationTask _parameterIdentificationTask;
      private ISensitivityAnalysisTask _sensitivityAnalysisTask;

      protected override void Context()
      {
         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         _contentLoader = A.Fake<IContentLoader>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _relatedItemSerializer = A.Fake<IRelatedItemSerializer>();
         _simulationTask = A.Fake<InteractionTasksForSimulation>();
         _context = A.Fake<IMoBiContext>();
         _cloneManager = A.Fake<ICloneManagerForBuildingBlock>();
         _taskRetriever = A.Fake<IBuildingBlockTaskRetriever>();
         _pkSimExportTask = A.Fake<IPKSimExportTask>();
         _simulationLoader = A.Fake<ISimulationLoader>();
         _objectIdResetter = A.Fake<IObjectIdResetter>();
         _observedDataTask = A.Fake<IObservedDataTask>();
         _parameterIdentificationTask= A.Fake<IParameterIdentificationTask>();
         _sensitivityAnalysisTask= A.Fake<ISensitivityAnalysisTask>();
         sut = new ReloadRelatedItemTask(_applicationConfiguration, _contentLoader, _dialogCreator,
            _relatedItemSerializer, _context, _cloneManager, _taskRetriever, _pkSimExportTask, _simulationLoader, _observedDataTask, _objectIdResetter,_parameterIdentificationTask,_sensitivityAnalysisTask);
         _relatedItem = A.Fake<RelatedItem>();
      }

      protected override void Because()
      {
         sut.Load(_relatedItem);
      }
   }

   internal class When_reloading_a_simulation : concern_for_ReloadRelatedItemTaskSpecs
   {
      private IMoBiSimulation _simulation; 

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         A.CallTo(() => _relatedItemSerializer.Deserialize(_relatedItem)).Returns(_simulation);
         A.CallTo(() => _simulationLoader.AddSimulationToProject(_simulation)).Returns(new MoBiMacroCommand());
      }

      [Observation]
      public void should_add_simulation_to_project()
      {
         A.CallTo(() => _simulationLoader.AddSimulationToProject(_simulation)).MustHaveHappened();
      }
   }

   internal class When_reloading_a_building_block : concern_for_ReloadRelatedItemTaskSpecs
   {
      private readonly IBuildingBlock _buildingBlock = new ReactionBuildingBlock();
      private readonly IInteractionTasksForBuildingBlock _task = A.Fake<IInteractionTasksForBuildingBlock>();
      private readonly IMoBiCommand _addCommand = A.Fake<IMoBiCommand>();
      private readonly IBuildingBlock _clone = A.Fake<IBuildingBlock>();

      protected override void Context()
      {
         base.Context();
         _relatedItem = A.Fake<RelatedItem>();
         A.CallTo(() => _relatedItemSerializer.Deserialize(_relatedItem)).Returns(_buildingBlock);
         A.CallTo(() => _taskRetriever.TaskFor(_buildingBlock)).Returns(_task);
         A.CallTo(() => _cloneManager.CloneBuildingBlock(_buildingBlock)).Returns(_clone);
         A.CallTo(() => _task.AddToProject(_clone)).Returns(_addCommand);
      }

      [Observation]
      public void should_restore_the_building_block()
      {
         A.CallTo(() => _relatedItemSerializer.Deserialize(_relatedItem)).MustHaveHappened();
      }

      [Observation]
      public void should_clone_the_building_block()
      {
         A.CallTo(() => _cloneManager.CloneBuildingBlock(_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_the_correct_add_to_project_method()
      {
         A.CallTo(() => _taskRetriever.TaskFor(_buildingBlock)).MustHaveHappened();
         A.CallTo(() => _task.AddToProject(_clone)).MustHaveHappened();
      }

      [Observation]
      public void should_add_add_command_to_history()
      {
         A.CallTo(() => _context.AddToHistory(_addCommand)).MustHaveHappened();
      }
   }

   internal class When_reloading_some_observed_data : concern_for_ReloadRelatedItemTaskSpecs
   {
      private readonly IMoBiCommand _addCommand = A.Fake<IMoBiCommand>();
      private DataRepository _observedData;

      protected override void Context()
      {
         base.Context();
         _relatedItem = A.Fake<RelatedItem>();
         _observedData = new DataRepository();
         A.CallTo(() => _relatedItemSerializer.Deserialize(_relatedItem)).Returns(_observedData);
      }

      [Observation]
      public void should_reset_the_id_for_the_observed_data()
      {
         A.CallTo(() => _objectIdResetter.ResetIdFor(_observedData)).MustHaveHappened();
      }

      [Observation]
      public void should_execute_the_correct_add_to_project_method()
      {
         A.CallTo(() => _observedDataTask.AddObservedDataToProject(_observedData)).MustHaveHappened();
      }
   }
}