using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Events;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Services;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_ObservedDataTask : ContextSpecification<IObservedDataTask>
   {
      protected IDataImporter _dataImporter;
      protected IDimensionFactory _dimensionFactory;
      protected IMoBiContext _context;
      protected IDialogCreator _dialogCreator;
      protected DataRepository _dataRepository;
      protected MoBiProject _project;
      private IInteractionTask _interactionTask;
      private IDataRepositoryExportTask _dataRepositoryTask;
      protected IContainerTask _containerTask;
      private IObjectTypeResolver _objectTypeResolver;
      private IBuildingBlockRepository _buildingBlockRepository;
      protected IObjectBaseNamingTask _namingTask;

      protected override void Context()
      {
         _dataImporter = A.Fake<IDataImporter>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _dataRepository = new DataRepository {new BaseGrid("", DimensionFactoryForSpecs.Factory.Dimension("Time"))};
         _interactionTask = A.Fake<IInteractionTask>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _containerTask = A.Fake<IContainerTask>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _namingTask = A.Fake<IObjectBaseNamingTask>();
         sut = new ObservedDataTask(_dataImporter, _context, _dialogCreator, _interactionTask, _dataRepositoryTask, _containerTask, _objectTypeResolver, _buildingBlockRepository, _namingTask);

         _project = DomainHelperForSpecs.NewProject();
         A.CallTo(() => _context.Project).Returns(_project);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }
   }

   internal class When_removing_multiple_results_from_a_simulation : concern_for_ObservedDataTask
   {
      private IReadOnlyList<DataRepository> _repositories;
      private DataRepository _currentResult;
      private DataRepository _historicResult;
      private MoBiSimulation _moBiSimulation;

      protected override void Context()
      {
         base.Context();
         _currentResult = new DataRepository("id1");
         _historicResult = new DataRepository("id2");
         _repositories = new List<DataRepository> {_currentResult, _historicResult};
         _moBiSimulation = new MoBiSimulation {ResultsDataRepository = _currentResult};
         _moBiSimulation.HistoricResults.Add(_historicResult);

         _project.AddSimulation(_moBiSimulation);

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.RemoveResultsFromSimulations(_repositories);
      }

      [Observation]
      public void the_data_repositories_must_be_removed_from_the_simulations()
      {
         _moBiSimulation.HistoricResults.Contains(_historicResult).ShouldBeFalse();
         _moBiSimulation.ResultsDataRepository.ShouldBeNull();
      }
   }

   internal class When_renaming_a_observed_data_Repository : concern_for_ObservedDataTask
   {
      private string _newName;
      private IDataRepositoryNamer _dataRepositoryNamer;

      protected override void Context()
      {
         base.Context();
         _dataRepository.Name = "OLD";
         _newName = "New";
         A.CallTo(() => _namingTask.RenameFor(_dataRepository, A<IReadOnlyList<string>>._)).Returns(_newName);
         _dataRepositoryNamer = A.Fake<IDataRepositoryNamer>();
         A.CallTo(() => _context.Resolve<IDataRepositoryNamer>()).Returns(_dataRepositoryNamer);
      }

      protected override void Because()
      {
         sut.Rename(_dataRepository);
      }

      [Observation]
      public void should_use_the_data_repository_renamer_to_update_data_repository_name()
      {
         A.CallTo(() => _dataRepositoryNamer.Rename(_dataRepository, _newName)).MustHaveHappened();
      }
   }

   public class When_removing_a_data_Repository : concern_for_ObservedDataTask
   {
      protected override void Context()
      {
         base.Context();
         _project.AddObservedData(_dataRepository);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.Delete(_dataRepository);
      }

      [Observation]
      public void should_remove_observed_data_from_current_project()
      {
         _project.AllObservedData.Contains(_dataRepository).ShouldBeFalse();
      }
   }

   public class When_not_removing_a_data_Repository : concern_for_ObservedDataTask
   {
      protected override void Context()
      {
         base.Context();
         _project.AddObservedData(_dataRepository);
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.Delete(_dataRepository);
      }

      [Observation]
      public void should_not_remove_observed_data_from_current_project()
      {
         _project.AllObservedData.Contains(_dataRepository).ShouldBeTrue();
      }
   }

   public class When_addding_data_from_Excel : concern_for_ObservedDataTask
   {
      private ObservedDataAddedEvent _event;
      private OSPSuite.Core.Import.ImporterConfiguration _importerConfiguration;

      protected override void Context()
      {
         base.Context();

         _importerConfiguration = A.Fake<OSPSuite.Core.Import.ImporterConfiguration>();
         _importerConfiguration.Id = "Id";
         _importerConfiguration.AddParameter(A.Fake<DataFormatParameter>());

         A.CallTo(() => _dimensionFactory.DimensionsSortedByName).Returns(new []
         {
            DimensionFactoryForSpecs.MassDimension,
            DimensionFactoryForSpecs.TimeDimension,
            Constants.Dimension.NO_DIMENSION
         });
         A.CallTo(() => _dimensionFactory.Dimension(Constants.Dimension.TIME)).Returns(DimensionFactoryForSpecs.TimeDimension);

         A.CallTo(() => _context.PublishEvent(A<ObservedDataAddedEvent>._)).Invokes(call => _event = call.GetArgument<ObservedDataAddedEvent>(0));
         _dataRepository.Name = "A";
         _dataRepository.BaseGrid.Name = "B";

         A.CallTo(() => _containerTask.CreateUniqueName(A<IEnumerable<IWithName>>._, "A", true)).Returns("A");

         _dataRepository.Add(new DataColumn("name", DimensionFactoryForSpecs.MassDimension, _dataRepository.BaseGrid));

         A.CallTo(() => _dataImporter.ImportDataSets(A<IReadOnlyList<MetaDataCategory>>.Ignored, A<IReadOnlyList<ColumnInfo>>.Ignored, A<DataImporterSettings>.Ignored, A<string>.Ignored))
            .Returns((new[] { _dataRepository }, _importerConfiguration));
      }

      protected override void Because()
      {
         sut.AddObservedDataToProject();
      }

      [Observation]
      public void the_container_task_should_be_used_to_create_a_unique_name()
      {
         A.CallTo(() => _containerTask.CreateUniqueName(A<IEnumerable<IWithName>>._, "A", true)).MustHaveHappened();
      }

      [Observation]
      public void data_repository_should_have_paths_applied_on_x_and_y_columns()
      {
         _dataRepository.BaseGrid.QuantityInfo.PathAsString.ShouldBeEqualTo("A|B");
         _dataRepository.AllButBaseGrid().First().QuantityInfo.PathAsString.ShouldBeEqualTo("A|name");
      }

      [Observation]
      public void should_add_data_repositroy_to_current_project()
      {
         _project.AllObservedData.Contains(_dataRepository).ShouldBeTrue();
      }

      [Observation]
      public void should_publish_right_add_observed_data_event()
      {
         A.CallTo(() => _context.PublishEvent(A<ObservedDataAddedEvent>._)).MustHaveHappened();
         _event.ShouldNotBeNull();
         _event.DataRepository.ShouldBeEqualTo(_dataRepository);
      }
   }

   public class When_adding_an_observed_data_to_project_that_already_exists : concern_for_ObservedDataTask
   {
      private DataRepository _newDataRepository;

      protected override void Context()
      {
         base.Context();
         _newDataRepository = new DataRepository("NEW");
         _dataRepository.Id = "XX";
         _project.AddObservedData(_dataRepository);
      }

      protected override void Because()
      {
         sut.AddObservedDataToProject(_dataRepository);
         sut.AddObservedDataToProject(_newDataRepository);
      }

      [Observation]
      public void should_only_add_the_new_repository()
      {
         _project.AllObservedData.ShouldOnlyContain(_dataRepository, _newDataRepository);
      }
   }

   public class When_removing_all_results_defined_in_a_simulation : concern_for_ObservedDataTask
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithName("TOTO");
         _simulation.HistoricResults.Add(new DataRepository("Rep1"));
         _simulation.HistoricResults.Add(new DataRepository("Rep2"));
         _simulation.ResultsDataRepository = new DataRepository("Res");

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.DeleteAllResultsFrom(_simulation);
      }

      [Observation]
      public void should_ask_the_user_if_he_really_wants_to_delete_the_results()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveAllResultsFrom(_simulation.Name), ViewResult.Yes)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_all_historical_results_and_current_results()
      {
         _simulation.HistoricResults.Count.ShouldBeEqualTo(0);
         _simulation.ResultsDataRepository.ShouldBeNull();
      }
   }

   public class When_removing_all_results_over_all_simulation : concern_for_ObservedDataTask
   {
      private IMoBiSimulation _simulation1;
      private IMoBiSimulation _simulation2;

      protected override void Context()
      {
         base.Context();
         _simulation1 = new MoBiSimulation().WithName("SIM1");
         _simulation2 = new MoBiSimulation().WithName("SIM1");
         _simulation1.HistoricResults.Add(new DataRepository("Rep1"));
         _simulation2.HistoricResults.Add(new DataRepository("Rep2"));
         _simulation1.ResultsDataRepository = new DataRepository("Res");
         _simulation2.ResultsDataRepository = new DataRepository("Res");

         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);

         _project.AddSimulation(_simulation1);
         _project.AddSimulation(_simulation2);
      }

      protected override void Because()
      {
         sut.DeleteAllResultsFromAllSimulation();
      }

      [Observation]
      public void should_ask_the_user_if_he_really_wants_to_delete_the_results()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveAllResultsFromProject(), ViewResult.Yes)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_all_historical_results_and_current_results()
      {
         _simulation1.HistoricResults.Count.ShouldBeEqualTo(0);
         _simulation1.ResultsDataRepository.ShouldBeNull();

         _simulation2.HistoricResults.Count.ShouldBeEqualTo(0);
         _simulation2.ResultsDataRepository.ShouldBeNull();
      }
   }
}