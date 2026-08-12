using System.Linq;
using FakeItEasy;
using MoBi.Core;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using Converter121To130 = MoBi.Core.Serialization.Converter.v13.Converter121To130;
using CoreConverter121To130 = OSPSuite.Core.Converters.v13.Converter121To130;

namespace MoBi.ProjectConversion.v13
{
   public abstract class concern_for_Converter121To130 : ContextSpecification<Converter121To130>
   {
      protected override void Context()
      {
         sut = new Converter121To130(new CoreConverter121To130(A.Fake<ISolverSettingsFactory>()), new ObjectTypeResolver());
      }
   }

   public class When_converting_a_simulation_with_observed_data_used_in_charts_and_output_mappings : concern_for_Converter121To130
   {
      private MoBiSimulation _simulation;
      private MoBiProject _project;
      private DataRepository _plottedObservedData;
      private DataRepository _mappedObservedData;
      private DataRepository _unusedObservedData;
      private (int version, bool converted) _result;

      protected override void Context()
      {
         base.Context();
         _plottedObservedData = DomainHelperForSpecs.ObservedData("plotted");
         _mappedObservedData = DomainHelperForSpecs.ObservedData("mapped");
         _unusedObservedData = DomainHelperForSpecs.ObservedData("unused");

         _project = DomainHelperForSpecs.NewProject();
         _project.AddObservedData(_plottedObservedData);
         _project.AddObservedData(_mappedObservedData);
         _project.AddObservedData(_unusedObservedData);

         _simulation = new MoBiSimulation { Configuration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() } };

         var chart = new MoBiSimulationTimeProfileChart();
         var curve = new Curve();
         curve.SetxData(_plottedObservedData.BaseGrid, DimensionFactoryForSpecs.Factory);
         curve.SetyData(_plottedObservedData.AllButBaseGrid().First(), DimensionFactoryForSpecs.Factory);
         chart.AddCurve(curve);
         _simulation.AddAnalysis(chart);

         _simulation.OutputMappings.Add(new OutputMapping { WeightedObservedData = new WeightedObservedData(_mappedObservedData) });
      }

      protected override void Because()
      {
         _result = sut.Convert(_simulation, _project);
      }

      [Observation]
      public void should_seed_the_used_observed_data_from_the_chart_curves_and_output_mappings()
      {
         _simulation.UsedObservedData.Select(x => x.Id).ShouldOnlyContain(_plottedObservedData.Id, _mappedObservedData.Id);
      }

      [Observation]
      public void should_report_the_conversion_to_v13()
      {
         _result.version.ShouldBeEqualTo(ProjectVersions.V13_0);
         _result.converted.ShouldBeTrue();
      }
   }

   public class When_converting_a_simulation_already_tracking_observed_data : concern_for_Converter121To130
   {
      private MoBiSimulation _simulation;
      private MoBiProject _project;
      private DataRepository _trackedObservedData;

      protected override void Context()
      {
         base.Context();
         _trackedObservedData = DomainHelperForSpecs.ObservedData("tracked");
         _project = DomainHelperForSpecs.NewProject();
         _project.AddObservedData(_trackedObservedData);

         _simulation = new MoBiSimulation { Configuration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() } };
         _simulation.AddUsedObservedData(_trackedObservedData);
      }

      protected override void Because()
      {
         sut.Convert(_simulation, _project);
      }

      [Observation]
      public void should_leave_the_tracked_observed_data_unchanged()
      {
         _simulation.UsedObservedData.Select(x => x.Id).ShouldOnlyContain(_trackedObservedData.Id);
      }
   }

   public class When_converting_a_simulation_without_observed_data : concern_for_Converter121To130
   {
      private MoBiSimulation _simulation;
      private MoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = DomainHelperForSpecs.NewProject();
         _simulation = new MoBiSimulation { Configuration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() } };
      }

      protected override void Because()
      {
         sut.Convert(_simulation, _project);
      }

      [Observation]
      public void should_not_track_any_observed_data()
      {
         _simulation.UsedObservedData.Any().ShouldBeFalse();
      }
   }
}
