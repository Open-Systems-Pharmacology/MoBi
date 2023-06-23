using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiSimulation : ContextSpecification<MoBiSimulation>
   {
      protected override void Context()
      {
         sut = new MoBiSimulation();
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
         _moBiSimulation = new MoBiSimulation {DiagramManager = _simulationDiagramManager};
         sut.Model = new Model();
         sut.Model.Root = new Container();

         _moBiSimulation.OutputMappings.Add(new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(_moBiSimulation, new QuantitySelection("A|BC", QuantityType.Enzyme)),
            WeightedObservedData = new WeightedObservedData(DomainHelperForSpecs.ObservedData())
         });
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_moBiSimulation, _cloneManager);
      }

      [Observation]
      public void the_source_diagram_manager_creates_new_diagram_manager_for_target()
      {
         A.CallTo(() => _simulationDiagramManager.Create()).MustHaveHappened();
      }

      [Observation]
      public void should_have_updated_the_references_to_the_simulation_in_the_output_mappings()
      {
         sut.OutputMappings.Count().ShouldBeEqualTo(1);
         sut.OutputMappings.ElementAt(0).Simulation.ShouldBeEqualTo(sut);
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
      private CurveChart _chart;
      private Curve _curve;
      private readonly DataRepository _dataRepository = new DataRepository();
      private DataColumn _dataColumn;

      protected override void Context()
      {
         base.Context();
         _chart = new CurveChart();
         _curve = new Curve();
         _curve.SetxData(new DataColumn(), new MoBiDimensionFactory());
         _dataColumn = new DataColumn
         {
            Repository = _dataRepository
         };
         _curve.SetyData(_dataColumn, new MoBiDimensionFactory());
         _chart.AddCurve(_curve);

         sut.Update(A.Fake<SimulationConfiguration>(), A.Fake<IModel>());
         sut.Chart = _chart;
         sut.HasChanged = false;
         //make sure we do have curves initially
         sut.Chart.Curves.ShouldNotBeEmpty();
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
         sut.Chart.Curves.ShouldBeEmpty();
      }
   }
}