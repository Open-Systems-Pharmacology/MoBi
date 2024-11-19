using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
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

      void Update(SimulationConfiguration simulationConfiguration, IModel model);
      SolverSettings Solver { get; }
      OutputSchema OutputSchema { get; }

      /// <summary>
      ///    Returns true if the simulation uses <paramref name="templateBuildingBlock" />.
      ///    For module building blocks, the test checks if the <paramref name="templateBuildingBlock" /> is a member of
      ///    a module that's in use <see cref="Uses(Module)" />. If the <paramref name="templateBuildingBlock" /> is not a module
      ///    building block
      ///    the test is based on building block name and type.
      /// </summary>
      /// <returns>True if the simulation uses the <paramref name="templateBuildingBlock" />, otherwise false</returns>
      bool Uses(IBuildingBlock templateBuildingBlock);

      /// <summary>
      ///    Checks if the simulation has a module that shares a name with <paramref name="module" />
      ///    This indicates that the <paramref name="module" /> was used as a template
      /// </summary>
      /// <returns>True if the simulation has a matching module, otherwise false</returns>
      bool Uses(Module module);

      void MarkResultsOutOfDate();

      bool HasResults { get; }
      IReadOnlyList<Module> Modules { get; }
      IReadOnlyList<IBuildingBlock> BuildingBlocks();

      IReadOnlyCollection<OriginalQuantityValue> OriginalQuantityValues { get; }

      /// <summary>
      ///    Adds an original quantity value so that changes to quantities in the simulation can be tracked.
      ///    There can only be one original quantity value per path, adding a second <paramref name="quantityValue" />
      ///    with identical path has no affect
      /// </summary>
      void AddOriginalQuantityValue(OriginalQuantityValue quantityValue);

      void RemoveOriginalQuantityValue(OriginalQuantityValue quantityValue);
      OriginalQuantityValue OriginalQuantityValueFor(OriginalQuantityValue quantityValue);
      void ClearOriginalQuantities();
   }

   public class MoBiSimulation : ModelCoreSimulation, IMoBiSimulation
   {
      private readonly IList<ISimulationAnalysis> _allSimulationAnalyses = new List<ISimulationAnalysis>();
      private DataRepository _results;
      public IDiagramModel DiagramModel { get; set; }
      public CurveChart Chart { get; set; }
      public SimulationPredictedVsObservedChart PredictedVsObservedChart { get; set; }
      public SimulationResidualVsTimeChart ResidualVsTimeChart { get; set; }
      public string ParameterIdentificationWorkingDirectory { get; set; }
      public IDiagramManager<IMoBiSimulation> DiagramManager { get; set; }
      public OutputMappings OutputMappings { get; set; } = new OutputMappings();

      private readonly ICache<string, OriginalQuantityValue> _quantityValueCache = new Cache<string, OriginalQuantityValue>(onMissingKey: key => null);
      private bool _hasChanged;

      public MoBiSimulation()
      {
         HistoricResults = new Cache<string, DataRepository>(x => x.Id, x => null);
      }

      public bool HasChanged
      {
         get => _hasChanged || _quantityValueCache.Any();
         set => _hasChanged = value;
      }

      public OutputSchema OutputSchema => Settings.OutputSchema;

      public CurveChartTemplate ChartTemplateByName(string chartTemplate) => Settings.ChartTemplateByName(chartTemplate);

      public void RemoveAllChartTemplates() => Settings.RemoveAllChartTemplates();

      public bool Uses(IBuildingBlock templateBuildingBlock)
      {
         // We can consider the building block in-use if it belongs to a module that is in use.
         if (templateBuildingBlock.Module != null)
            return usesModuleBuildingBlock(templateBuildingBlock);

         // Simple name match for building blocks that do not belong to a module
         switch (templateBuildingBlock)
         {
            case IndividualBuildingBlock individualBuildingBlock:
               return string.Equals(Configuration.Individual?.Name, individualBuildingBlock.Name);
            case ExpressionProfileBuildingBlock expressionProfileBuildingBlock:
               return Configuration.ExpressionProfiles.ExistsByName(expressionProfileBuildingBlock.Name);
         }

         return false;
      }

      private bool usesModuleBuildingBlock(IBuildingBlock templateBuildingBlock) => BuildingBlocks().Any(buildingBlock => buildingBlock.IsTemplateMatchFor(templateBuildingBlock));

      public IReadOnlyList<Module> Modules => Configuration.ModuleConfigurations.Select(x => x.Module).ToList();

      public IReadOnlyList<IBuildingBlock> BuildingBlocks()
      {
         var buildingBlocks = Modules.SelectMany(module => module.BuildingBlocks).Concat(Configuration.ExpressionProfiles).ToList();

         if (Configuration.Individual != null)
            buildingBlocks.Add(Configuration.Individual);
         return buildingBlocks;
      }

      public IReadOnlyCollection<OriginalQuantityValue> OriginalQuantityValues => _quantityValueCache;

      public void AddOriginalQuantityValue(OriginalQuantityValue quantityValue)
      {
         // if there's already a value set for this path and type, then ignore the add
         // we only store the first instance for a path
         if (_quantityValueCache[quantityValue.Id] == null)
            _quantityValueCache[quantityValue.Id] = quantityValue;
      }

      public void RemoveOriginalQuantityValue(OriginalQuantityValue quantityValue) => _quantityValueCache.Remove(quantityValue.Id);

      public OriginalQuantityValue OriginalQuantityValueFor(OriginalQuantityValue quantityValue) => _quantityValueCache[quantityValue.Id];

      public void ClearOriginalQuantities()
      {
         _quantityValueCache.Clear();
      }

      public SolverSettings Solver => Settings.Solver;

      public bool UsesObservedData(DataRepository dataRepository)
      {
         return OutputMappings.Any(x => x.UsesObservedData(dataRepository)) || Charts.Any(x => chartUsesObservedData(dataRepository, x));
      }

      private bool chartUsesObservedData(DataRepository dataRepository, CurveChart curveChart) => curveChart != null && curveChart.Curves.Any(c => Equals(c.yData.Repository, dataRepository));

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         Chart?.AcceptVisitor(visitor);
      }

      public void Update(SimulationConfiguration simulationConfiguration, IModel model)
      {
         Configuration = simulationConfiguration;
         Model = model;
      }

      public ICache<string, DataRepository> HistoricResults { get; }

      public void AddHistoricResults(DataRepository results) => HistoricResults.Add(results);

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceSimulation = source as IMoBiSimulation;
         if (sourceSimulation == null) return;

         OutputMappings.UpdatePropertiesFrom(sourceSimulation.OutputMappings, cloneManager);
         //Updating the properties will hold the reference to the source simulation, we need to reset usage
         //and make sure the output mapping is referencing THIS simulation
         OutputMappings.SwapSimulation(sourceSimulation, this);

         sourceSimulation.OriginalQuantityValues.Each(x => AddOriginalQuantityValue(new OriginalQuantityValue().WithPropertiesFrom(x)));

         this.UpdateDiagramFrom(sourceSimulation);
      }

      public void RemoveUsedObservedData(DataRepository dataRepository)
      {
         if (!UsesObservedData(dataRepository))
            return;

         var curveToRemove = Chart.Curves.Where(c => Equals(c.yData.Repository, dataRepository)).ToList();
         if (!curveToRemove.Any())
            return;

         curveToRemove.Each(curve => Chart.RemoveCurve(curve.Id));

         HasChanged = true;
      }

      public void RemoveOutputMappings(DataRepository dataRepository)
      {
         var outputsMatchingDeletedObservedData = OutputMappings.OutputMappingsUsingDataRepository(dataRepository).ToList();
         outputsMatchingDeletedObservedData.Each(OutputMappings.Remove);
      }

      public IEnumerable<CurveChart> Charts
      {
         get { yield return Chart; }
      }

      public void AddChartTemplate(CurveChartTemplate chartTemplate) => Settings.AddChartTemplate(chartTemplate);

      public void RemoveChartTemplate(string chartTemplateName) => Settings.RemoveChartTemplate(chartTemplateName);

      public IEnumerable<CurveChartTemplate> ChartTemplates => Settings.ChartTemplates;

      public CurveChartTemplate DefaultChartTemplate => Settings.DefaultChartTemplate;

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

      public void MarkResultsOutOfDate() => HasUpToDateResults = false;

      public bool HasResults => ResultsDataRepository != null;

      public bool Uses(Module module) => Configuration.ModuleConfigurations.Any(x => Equals(x.Module.Name, module.Name));

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