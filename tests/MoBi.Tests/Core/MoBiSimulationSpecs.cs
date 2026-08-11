using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Chart;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.UnitSystem;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSimulation : ContextSpecification<MoBiSimulation>
   {
      protected override void Context()
      {
         sut = new MoBiSimulation();
      }
   }

   public class When_checking_if_a_simulation_uses_a_module : concern_for_MoBiSimulation
   {
      private bool _result;

      protected override void Context()
      {
         base.Context();
         sut.Configuration = new SimulationConfiguration();
         sut.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName("a Module")));
      }

      protected override void Because()
      {
         _result = sut.Uses(new Module().WithName("a Module"));
      }

      [Observation]
      public void the_simulation_should_indicate_it_uses_the_module()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_checking_if_a_simulation_uses_a_building_block_but_the_building_block_is_used_when_the_module_is_used : concern_for_MoBiSimulation
   {
      private bool _result;
      private SpatialStructure _templateBuildingBlock;

      protected override void Context()
      {
         base.Context();
         sut.Configuration = new SimulationConfiguration();
         var simulationModule = new Module().WithName("a Module");
         simulationModule.Add(new SpatialStructure().WithName("a Building Block"));

         sut.Configuration.AddModuleConfiguration(new ModuleConfiguration(simulationModule));

         _templateBuildingBlock = new SpatialStructure
         {
            Module = new Module().WithName("a Module")
         }.WithName("a Building Block");
      }

      protected override void Because()
      {
         _result = sut.Uses(_templateBuildingBlock);
      }

      [Observation]
      public void the_simulation_should_indicate_it_uses_the_building_block_because_the_module_is_used()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_checking_if_a_simulation_uses_a_building_block_but_the_building_block_is_not_used_when_the_module_is_used : concern_for_MoBiSimulation
   {
      private bool _result;
      private SpatialStructure _templateBuildingBlock;

      protected override void Context()
      {
         base.Context();
         sut.Configuration = new SimulationConfiguration();
         sut.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName("a Module")));
         _templateBuildingBlock = new SpatialStructure
         {
            Module = new Module().WithName("a Module")
         };
      }

      protected override void Because()
      {
         _result = sut.Uses(_templateBuildingBlock);
      }

      [Observation]
      public void the_simulation_should_indicate_it_uses_the_building_block_because_the_module_is_used()
      {
         _result.ShouldBeFalse();
      }
   }

   public class When_setting_the_results_for_the_simulation : concern_for_MoBiSimulation
   {
      protected override void Context()
      {
         base.Context();
         sut.Creation.Version = "a version";
         sut.ResultsDataRepository = A.Fake<DataRepository>();
         sut.Creation.Version = "another version";
      }

      protected override void Because()
      {
         sut.ResultsDataRepository = A.Fake<DataRepository>();
      }

      [Observation]
      public void the_results_should_be_considered_up_to_date()
      {
         sut.HasUpToDateResults.ShouldBeTrue();
      }
   }

   public class When_updating_properties_from_source_simulation : concern_for_MoBiSimulation
   {
      private ICloneManager _cloneManager;
      private MoBiSimulation _moBiSimulation;
      private ISimulationDiagramManager _simulationDiagramManager;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         _simulationDiagramManager = A.Fake<ISimulationDiagramManager>();
         _moBiSimulation = new MoBiSimulation { DiagramManager = _simulationDiagramManager };
         sut.Model = new Model();
         sut.Model.Root = new Container();

         _moBiSimulation.AddAnalysis(new MoBiSimulationTimeProfileChart());

         var originalQuantityValue = new OriginalQuantityValue
         {
            Path = "A|BC",
         };
         _moBiSimulation.AddOriginalQuantityValue(originalQuantityValue);

         _moBiSimulation.OutputMappings.Add(new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(_moBiSimulation, new QuantitySelection("A|BC", QuantityType.Enzyme)),
            WeightedObservedData = new WeightedObservedData(DomainHelperForSpecs.ObservedData())
         });
         _moBiSimulation.AddUsedObservedData(DomainHelperForSpecs.ObservedData("usedData"));
         _moBiSimulation.HasUntraceableChanges = true;
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_moBiSimulation, _cloneManager);
      }

      [Observation]
      public void should_have_transferred_properties()
      {
         sut.HasUntraceableChanges.ShouldBeEqualTo(_moBiSimulation.HasUntraceableChanges);
      }

      [Observation]
      public void the_simulation_has_updated_original_quantity_values()
      {
         sut.OriginalQuantityValues.Count.ShouldBeEqualTo(1);
         sut.OriginalQuantityValues.ElementAt(0).Path.ShouldBeEqualTo("A|BC");
         sut.OriginalQuantityValues.ElementAt(0).ShouldNotBeEqualTo(_moBiSimulation.OriginalQuantityValues.First());
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_manager_for_target()
      {
         A.CallTo(() => _simulationDiagramManager.Create()).MustHaveHappened();
      }

      [Observation]
      public void the_chart_should_be_cloned()
      {
         A.CallTo(() => _cloneManager.Clone(A<IUpdatable>.That.IsEqualTo(_moBiSimulation.Charts.First()))).MustHaveHappened();
      }

      [Observation]
      public void should_have_updated_the_references_to_the_simulation_in_the_output_mappings()
      {
         sut.OutputMappings.Count().ShouldBeEqualTo(1);
         sut.OutputMappings.ElementAt(0).Simulation.ShouldBeEqualTo(sut);
      }

      [Observation]
      public void should_have_copied_the_used_observed_data_referencing_the_target_simulation()
      {
         sut.UsedObservedData.Count().ShouldBeEqualTo(1);
         sut.UsedObservedData.ElementAt(0).Id.ShouldBeEqualTo("usedData");
         sut.UsedObservedData.ElementAt(0).Simulation.ShouldBeEqualTo(sut);
         sut.UsedObservedData.ElementAt(0).ShouldNotBeEqualTo(_moBiSimulation.UsedObservedData.First());
      }
   }

   public class When_updating_properties_from_a_source_simulation_that_already_has_analyses : concern_for_MoBiSimulation
   {
      private ICloneManager _cloneManager;
      private MoBiSimulation _sourceSimulation;
      private MoBiSimulationTimeProfileChart _existingChart;
      private MoBiSimulationTimeProfileChart _sourceChart;
      private MoBiSimulationTimeProfileChart _clonedChart;

      protected override void Context()
      {
         base.Context();
         _cloneManager = A.Fake<ICloneManager>();
         sut.Model = new Model();
         sut.Model.Root = new Container();

         _existingChart = new MoBiSimulationTimeProfileChart().WithName("Existing");
         sut.AddAnalysis(_existingChart);

         _sourceSimulation = new MoBiSimulation();
         _sourceChart = new MoBiSimulationTimeProfileChart().WithName("Source");
         _sourceSimulation.AddAnalysis(_sourceChart);

         _clonedChart = new MoBiSimulationTimeProfileChart().WithName("Cloned");
         A.CallTo(() => _cloneManager.Clone(A<IUpdatable>.That.IsEqualTo(_sourceChart))).Returns(_clonedChart);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceSimulation, _cloneManager);
      }

      [Observation]
      public void should_clear_existing_analyses_before_adding_cloned_ones()
      {
         sut.Analyses.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_contain_only_the_cloned_chart_from_the_source()
      {
         sut.Analyses.ShouldContain(_clonedChart);
      }

      [Observation]
      public void should_not_contain_the_previously_existing_chart()
      {
         sut.Analyses.ShouldNotContain(_existingChart);
      }
   }

   public class When_removing_analyses_from_the_simulation : concern_for_MoBiSimulation
   {
      private ISimulationAnalysis _simulationAnalysis;

      protected override void Context()
      {
         base.Context();
         _simulationAnalysis = A.Fake<ISimulationAnalysis>();
         sut.AddAnalysis(_simulationAnalysis);
      }

      protected override void Because()
      {
         sut.RemoveAnalysis(_simulationAnalysis);
      }

      [Observation]
      public void the_simulation_must_indicate_it_has_changed()
      {
         sut.HasChanged.ShouldBeTrue();
      }
   }

   public class When_adding_new_analyses_to_the_simulation : concern_for_MoBiSimulation
   {
      protected override void Because()
      {
         sut.AddAnalysis(A.Fake<ISimulationAnalysis>());
      }

      [Observation]
      public void the_simulation_must_indicate_it_has_changed()
      {
         sut.HasChanged.ShouldBeTrue();
      }
   }

   public class When_removing_observed_data_from_the_simulation : concern_for_MoBiSimulation
   {
      private MoBiSimulationTimeProfileChart _chart;
      private Curve _curve;
      private readonly DataRepository _dataRepository = new DataRepository();
      private DataColumn _dataColumn;
      private readonly List<SimulationEntitySource> _simulationEntitySources = new List<SimulationEntitySource>();

      protected override void Context()
      {
         base.Context();
         _chart = new MoBiSimulationTimeProfileChart();
         _curve = new Curve();
         _curve.SetxData(new DataColumn(), new MoBiDimensionFactory());
         _dataColumn = new DataColumn
         {
            Repository = _dataRepository
         };
         _curve.SetyData(_dataColumn, new MoBiDimensionFactory());
         _chart.AddCurve(_curve);

         sut.Update(A.Fake<SimulationConfiguration>(), A.Fake<IModel>(), _simulationEntitySources);
         sut.AddAnalysis(_chart);
         sut.HasChanged = false;
         //make sure we do have curves initially
         sut.Charts.First().Curves.ShouldNotBeEmpty();
      }

      protected override void Because()
      {
         sut.RemoveUsedObservedData(_dataRepository);
      }

      [Observation]
      public void the_simulation_must_indicate_it_has_changed()
      {
         sut.HasChanged.ShouldBeTrue();
      }

      [Observation]
      public void the_observed_data_should_have_been_removed()
      {
         sut.Charts.First().Curves.ShouldBeEmpty();
      }
   }

   public abstract class Using_selectable_building_blocks<TBuildingBlock> : concern_for_MoBiSimulation where TBuildingBlock : BuildingBlock, new()
   {
      protected bool _result;
      protected TBuildingBlock _templateBuildingBlock;
      protected TBuildingBlock _simulationBuildingBlock;
      private SimulationConfiguration _simulationConfiguration;
      protected ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         base.Context();
         _simulationConfiguration = new SimulationConfiguration();
         sut.Configuration = _simulationConfiguration;
         var simulationModule = new Module().WithName("a Module");
         _simulationBuildingBlock = new TBuildingBlock().WithName("bb");
         simulationModule.Add(_simulationBuildingBlock);
         _moduleConfiguration = new ModuleConfiguration(simulationModule);
         sut.Configuration.AddModuleConfiguration(_moduleConfiguration);


         var templateModule = new Module().WithName("a Module");
         _templateBuildingBlock = new TBuildingBlock().WithName("bb");
         templateModule.Add(_templateBuildingBlock);
         SelectBuildingBlock();
      }

      public abstract void SelectBuildingBlock();

      protected override void Because()
      {
         _result = sut.Uses(_templateBuildingBlock);
      }
   }

   public class When_checking_if_a_simulation_uses_an_selected_initial_condition : Using_selectable_building_blocks<InitialConditionsBuildingBlock>
   {
      [Observation]
      public void the_simulation_should_indicate_the_building_block_is_not_used()
      {
         _result.ShouldBeTrue();
      }

      public override void SelectBuildingBlock()
      {
         _moduleConfiguration.SelectedInitialConditions = _templateBuildingBlock;
      }
   }

   public class When_checking_if_a_simulation_uses_an_selected_parameter_values : Using_selectable_building_blocks<ParameterValuesBuildingBlock>
   {
      [Observation]
      public void the_simulation_should_indicate_the_building_block_is_not_used()
      {
         _result.ShouldBeTrue();
      }

      public override void SelectBuildingBlock()
      {
         _moduleConfiguration.SelectedParameterValues = _templateBuildingBlock;
      }
   }

   public class When_checking_if_a_simulation_uses_an_unselected_initial_condition : Using_selectable_building_blocks<InitialConditionsBuildingBlock>
   {
      [Observation]
      public void the_simulation_should_indicate_the_building_block_is_not_used()
      {
         _result.ShouldBeFalse();
      }

      public override void SelectBuildingBlock()
      {
         _moduleConfiguration.SelectedInitialConditions = null;
      }
   }

   public class When_checking_if_a_simulation_uses_an_unselected_parameter_values : Using_selectable_building_blocks<ParameterValuesBuildingBlock>
   {
      [Observation]
      public void the_simulation_should_indicate_the_building_block_is_not_used()
      {
         _result.ShouldBeFalse();
      }

      public override void SelectBuildingBlock()
      {
         _moduleConfiguration.SelectedParameterValues = null;
      }
   }

   public abstract class concern_for_MoBiSimulation_lazy_model : concern_for_MoBiSimulation
   {
      protected IModel _model;
      protected int _loaderCallCount;

      protected override void Context()
      {
         base.Context();
         _model = A.Fake<IModel>();
         _loaderCallCount = 0;
         // Simulate the shell wiring done by the mapper: the loader constructs and assigns the model.
         sut.SetLazyModelLoader(() =>
         {
            _loaderCallCount++;
            sut.Model = _model;
         });
      }
   }

   public class When_a_shell_simulation_has_not_been_accessed : concern_for_MoBiSimulation_lazy_model
   {
      [Observation]
      public void should_report_that_the_model_is_not_loaded_without_triggering_materialization()
      {
         sut.IsModelLoaded.ShouldBeFalse();
         _loaderCallCount.ShouldBeEqualTo(0);
      }
   }

   public class When_the_model_of_a_shell_simulation_is_accessed : concern_for_MoBiSimulation_lazy_model
   {
      private IModel _result;

      protected override void Because()
      {
         _result = sut.Model;
      }

      [Observation]
      public void should_materialize_and_return_the_model()
      {
         _result.ShouldBeEqualTo(_model);
      }

      [Observation]
      public void should_report_the_model_as_loaded()
      {
         sut.IsModelLoaded.ShouldBeTrue();
      }

      [Observation]
      public void should_only_invoke_the_lazy_loader_once_even_on_repeated_access()
      {
         var unused = sut.Model;
         unused = sut.Model;
         _loaderCallCount.ShouldBeEqualTo(1);
      }
   }

   public class When_the_model_is_assigned_directly_on_a_simulation : concern_for_MoBiSimulation_lazy_model
   {
      protected override void Because()
      {
         sut.Model = _model;
      }

      [Observation]
      public void should_be_considered_loaded_and_not_invoke_the_lazy_loader()
      {
         sut.IsModelLoaded.ShouldBeTrue();
         var unused = sut.Model;
         _loaderCallCount.ShouldBeEqualTo(0);
      }
   }

   public class When_the_lazy_model_loader_fails_before_assigning_a_model : concern_for_MoBiSimulation_lazy_model
   {
      protected override void Context()
      {
         base.Context();
         // a materialization that fails (e.g. corrupt content) before a model is assigned
         sut.SetLazyModelLoader(() => throw new System.InvalidOperationException("materialization failed"));
      }

      [Observation]
      public void should_leave_the_simulation_as_an_unloaded_shell_so_it_stays_retryable_and_save_safe()
      {
         The.Action(() => { var unused = sut.Model; }).ShouldThrowAn<System.InvalidOperationException>();
         sut.IsModelLoaded.ShouldBeFalse();
      }
   }

   public class When_adding_used_observed_data_to_a_simulation : concern_for_MoBiSimulation
   {
      private DataRepository _observedData;

      protected override void Context()
      {
         base.Context();
         _observedData = DomainHelperForSpecs.ObservedData();
      }

      protected override void Because()
      {
         sut.AddUsedObservedData(_observedData);
         //adding the same observed data twice should not create a second entry
         sut.AddUsedObservedData(_observedData);
      }

      [Observation]
      public void the_simulation_should_track_the_observed_data_only_once()
      {
         sut.UsedObservedData.Count().ShouldBeEqualTo(1);
         sut.UsedObservedData.ElementAt(0).Id.ShouldBeEqualTo(_observedData.Id);
      }

      [Observation]
      public void the_tracked_observed_data_should_reference_the_simulation()
      {
         sut.UsedObservedData.ElementAt(0).Simulation.ShouldBeEqualTo(sut);
      }

      [Observation]
      public void the_simulation_should_indicate_that_it_uses_the_observed_data()
      {
         sut.UsesObservedData(_observedData).ShouldBeTrue();
      }
   }

   public class When_removing_used_observed_data_from_a_simulation : concern_for_MoBiSimulation
   {
      private DataRepository _observedData;

      protected override void Context()
      {
         base.Context();
         _observedData = DomainHelperForSpecs.ObservedData();
         sut.AddUsedObservedData(_observedData);
         sut.HasChanged = false;
      }

      protected override void Because()
      {
         sut.RemoveUsedObservedData(_observedData);
      }

      [Observation]
      public void the_simulation_should_not_track_the_observed_data_anymore()
      {
         sut.UsedObservedData.Any().ShouldBeFalse();
         sut.UsesObservedData(_observedData).ShouldBeFalse();
      }

      [Observation]
      public void the_simulation_should_be_marked_as_changed()
      {
         sut.HasChanged.ShouldBeTrue();
      }
   }

}