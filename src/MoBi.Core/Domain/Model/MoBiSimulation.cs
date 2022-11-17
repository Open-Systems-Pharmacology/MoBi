using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiSimulation : IWithDiagramFor<IMoBiSimulation>, ISimulation, IWithChartTemplates
   {
      ICache<string, DataRepository> HistoricResults { get; }
      CurveChart Chart { get; set; }
      SimulationPredictedVsObservedChart PredictedVsObservedChart { get; set; }
      SimulationResidualVsTimeChart ResidualVsTimeChart { get; set; }

      IMoBiBuildConfiguration MoBiBuildConfiguration { get; }
      string ParameterIdentificationWorkingDirectory { get; set; }
      void Update(IMoBiBuildConfiguration buildConfiguration, IModel model);
      SolverSettings Solver { get; }
      OutputSchema OutputSchema { get; }

      /// <summary>
      ///    Returns true if the simulation as created using the <paramref name="templateBuildingBlock" /> otherwise false.
      /// </summary>
      bool IsCreatedBy(IBuildingBlock templateBuildingBlock);

      void MarkResultsOutOfDate();

      bool HasResults { get; }
   }

   public class MoBiSimulation : ModelCoreSimulation, IMoBiSimulation
   {
      private bool _hasChanged;
      private readonly IList<ISimulationAnalysis> _allSimulationAnalyses = new List<ISimulationAnalysis>();
      private DataRepository _results;
      public IDiagramModel DiagramModel { get; set; }
      public CurveChart Chart { get; set; }
      public SimulationPredictedVsObservedChart PredictedVsObservedChart { get; set; }
      public SimulationResidualVsTimeChart ResidualVsTimeChart { get; set; }
      public string ParameterIdentificationWorkingDirectory { get; set; }
      public IDiagramManager<IMoBiSimulation> DiagramManager { get; set; }
      public OutputMappings OutputMappings { get; set; } = new OutputMappings();

      public MoBiSimulation()
      {
         HistoricResults = new Cache<string, DataRepository>(x => x.Id, x => null);
      }

      public bool HasChanged
      {
         get => _hasChanged || MoBiBuildConfiguration.HasChangedBuildingBlocks();
         set => _hasChanged = value;
      }

      public OutputSchema OutputSchema => SimulationSettings.OutputSchema;

      public CurveChartTemplate ChartTemplateByName(string chartTemplate)
      {
         return SimulationSettings.ChartTemplateByName(chartTemplate);
      }

      public void RemoveAllChartTemplates()
      {
         SimulationSettings.RemoveAllChartTemplates();
      }

      public bool IsCreatedBy(IBuildingBlock templateBuildingBlock)
      {
         return MoBiBuildConfiguration.BuildingInfoForTemplate(templateBuildingBlock) != null;
      }

      public SolverSettings Solver => SimulationSettings.Solver;

      public bool UsesObservedData(DataRepository dataRepository)
      {
         return OutputMappings.Any(x => x.UsesObservedData(dataRepository)) || Charts.Any(x => chartUsesObservedData(dataRepository, x));
      }

      private bool chartUsesObservedData(DataRepository dataRepository, CurveChart curveChart)
      {
         return curveChart != null && curveChart.Curves.Any(c => Equals(c.yData.Repository, dataRepository));
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         Chart?.AcceptVisitor(visitor);
      }

      public void Update(IMoBiBuildConfiguration buildConfiguration, IModel model)
      {
         BuildConfiguration = buildConfiguration;
         Model = model;
      }

      public ICache<string, DataRepository> HistoricResults { get; }

      public IMoBiBuildConfiguration MoBiBuildConfiguration => BuildConfiguration as IMoBiBuildConfiguration;

      public void AddHistoricResults(DataRepository results)
      {
         HistoricResults.Add(results);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceSimulation = source as IMoBiSimulation;
         if (sourceSimulation == null) return;

         OutputMappings.UpdatePropertiesFrom(sourceSimulation.OutputMappings, cloneManager);
         //Updating the properties will hold the reference to the source simulation, we need to reset usage
         //and make sure the output mapping is referencing THIS simulation
         OutputMappings.SwapSimulation(sourceSimulation, this);

         this.UpdateDiagramFrom(sourceSimulation);
      }

      public double? TotalDrugMassPerBodyWeightFor(string compoundName)
      {
         return null;
      }

      public void RemoveUsedObservedData(DataRepository dataRepository)
      {
         

      }

      public void RemoveOutputMappings(DataRepository dataRepository)
      {
         
      }

      public IEnumerable<CurveChart> Charts
      {
         get { yield return Chart; }
      }

      public new IReactionBuildingBlock Reactions
      {
         get => BuildConfiguration.Reactions;
         set => BuildConfiguration.Reactions = value;
      }

      public void AddChartTemplate(CurveChartTemplate chartTemplate)
      {
         SimulationSettings.AddChartTemplate(chartTemplate);
      }

      public void RemoveChartTemplate(string chartTemplateName)
      {
         SimulationSettings.RemoveChartTemplate(chartTemplateName);
      }

      public IEnumerable<CurveChartTemplate> ChartTemplates => SimulationSettings.ChartTemplates;

      public CurveChartTemplate DefaultChartTemplate => SimulationSettings.DefaultChartTemplate;

      public bool IsLoaded { get; set; }

      public void RemoveAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         _allSimulationAnalyses.Remove(simulationAnalysis);
         HasChanged = true;
      }

      public void AddAnalysis(ISimulationAnalysis simulationAnalysis)
      {
         _allSimulationAnalyses.Add(simulationAnalysis);
         simulationAnalysis.Analysable = this;
         HasChanged = true;
      }

      public IEnumerable<ISimulationAnalysis> Analyses => _allSimulationAnalyses;

      public bool HasUpToDateResults { get; private set; }

      public void MarkResultsOutOfDate()
      {
         HasUpToDateResults = false;
      }

      public bool HasResults => ResultsDataRepository != null;

      public DataRepository ResultsDataRepository
      {
         get => _results;
         set
         {
            _results = value;
            HasUpToDateResults = true;
         }
      }

      public bool ComesFromPKSim => Creation.Origin == Origins.PKSim;
   }
}
