using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Model;

public interface IMoBiSimulation : IWithDiagramFor<IMoBiSimulation>, ISimulation, IWithChartTemplates
{
   ICache<string, DataRepository> HistoricResults { get; }

   void Update(SimulationConfiguration simulationConfiguration, IModel model, IReadOnlyCollection<SimulationEntitySource> simulationEntitySources);
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
   ///    During project conversion from V11 to V12, any changes that are present in the simulation
   ///    won't be traceable back to a specific building block. This flag is used to indicate that to a user
   /// </summary>
   bool HasUntraceableChanges { get; set; }

   /// <summary>
   ///    Adds an original quantity value so that changes to quantities in the simulation can be tracked.
   ///    There can only be one original quantity value per path, adding a second <paramref name="quantityValue" />
   ///    with identical path has no affect
   /// </summary>
   void AddOriginalQuantityValue(OriginalQuantityValue quantityValue);

   void RemoveOriginalQuantityValue(OriginalQuantityValue quantityValue);
   OriginalQuantityValue OriginalQuantityValueFor(OriginalQuantityValue quantityValue);
   void ClearOriginalQuantities();

   /// <summary>
   ///    Whether the model has been constructed. Reading this does NOT trigger lazy materialization of a
   ///    simulation loaded as a shell, so it is safe to use to decide whether model-dependent work is needed.
   /// </summary>
   bool IsModelLoaded { get; }
}

public class MoBiSimulation : ModelCoreSimulation, IMoBiSimulation
{
   private readonly IList<ISimulationAnalysis> _allSimulationAnalyses = new List<ISimulationAnalysis>();
   private DataRepository _results;
   public IDiagramModel DiagramModel { get; set; }

   public string ParameterIdentificationWorkingDirectory { get; set; }
   public IDiagramManager<IMoBiSimulation> DiagramManager { get; set; }
   public OutputMappings OutputMappings { get; set; } = new OutputMappings();

   /// <summary>
   ///    During project conversion from V11 to V12, any changes that are present in the simulation
   ///    won't be traceable back to a specific building block. This flag is used to indicate that to a user
   /// </summary>
   public bool HasUntraceableChanges { get; set; }

   private readonly ICache<string, OriginalQuantityValue> _quantityValueCache = new Cache<string, OriginalQuantityValue>(onMissingKey: key => null);
   private bool _hasChanged;
   private bool _modelMaterialized;
   private bool _materializingModel;
   private Action _lazyModelLoader;

   public MoBiSimulation()
   {
      HistoricResults = new Cache<string, DataRepository>(x => x.Id, x => null);
   }

   /// <summary>
   ///    When the project is loaded, a simulation is created as a "shell" whose <see cref="Model" /> is not yet
   ///    constructed. The lazy loader passed here deserializes and assigns the model the first time
   ///    <see cref="Model" /> is accessed. This keeps model construction (the expensive part of loading a
   ///    project) deferred until the model is actually needed.
   /// </summary>
   public void SetLazyModelLoader(Action lazyModelLoader) => _lazyModelLoader = lazyModelLoader;

   /// <summary>
   ///    Whether the model has been constructed/assigned. Reading this does NOT trigger lazy materialization,
   ///    so it can be used to decide whether work that depends on the model is necessary (e.g. unregistration).
   /// </summary>
   public bool IsModelLoaded => _modelMaterialized;

   /// <summary>
   ///    When set, the <see cref="Model" /> getter returns the (possibly null) backing model WITHOUT triggering
   ///    lazy materialization. Used only while serializing a changed shell on save, so that the retained model
   ///    bytes can be reused instead of building the model just to re-serialize it unchanged.
   /// </summary>
   public bool SuppressModelMaterialization { get; set; }

   public override IModel Model
   {
      get
      {
         // A shell was loaded without a model. Materialize it on first access.
         if (!SuppressModelMaterialization && !_modelMaterialized && !_materializingModel && _lazyModelLoader != null)
         {
            // _materializingModel guards re-entrancy: registering the materialized model re-reads Model via AcceptVisitor.
            _materializingModel = true;
            try
            {
               _lazyModelLoader();
            }
            finally
            {
               _materializingModel = false;
            }
         }

         return base.Model;
      }
      set
      {
         base.Model = value;
         _modelMaterialized = true;
      }
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
      var buildingBlocks = Configuration.ModuleConfigurations.SelectMany(usedBuildingBlocksFrom).Concat(Configuration.ExpressionProfiles).ToList();

      if (Configuration.Individual != null)
         buildingBlocks.Add(Configuration.Individual);
      return buildingBlocks;
   }

   private IReadOnlyList<IBuildingBlock> usedBuildingBlocksFrom(ModuleConfiguration moduleConfiguration)
   {
      var module = moduleConfiguration.Module;
      // Only add selected InitialConditions and ParameterValues, not ones that are not selected
      return module.BuildingBlocks
         .Except(module.ParameterValuesCollection.Concat<IBuildingBlock>(module.InitialConditionsCollection))
         .Concat(new List<IBuildingBlock> { moduleConfiguration.SelectedInitialConditions, moduleConfiguration.SelectedParameterValues })
         .Where(x => x != null).ToList();
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
      _allSimulationAnalyses.OfType<MoBiSimulationTimeProfileChart>().Each(chart => chart.AcceptVisitor(visitor));
   }

   public void Update(SimulationConfiguration simulationConfiguration, IModel model, IReadOnlyCollection<SimulationEntitySource> simulationEntitySources)
   {
      Configuration = simulationConfiguration;
      Model = model;
      simulationEntitySources.Each(EntitySources.Add);
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
      HasUntraceableChanges = sourceSimulation.HasUntraceableChanges;

      this.UpdateDiagramFrom(sourceSimulation);
      _allSimulationAnalyses.Clear();
      sourceSimulation.Analyses.OfType<IUpdatable>().Each(analysis =>
      {
         var clone = cloneManager.Clone(analysis);
         if (clone is ISimulationAnalysis simulationAnalysis)
            AddAnalysis(simulationAnalysis);
      });
   }

   public void RemoveUsedObservedData(DataRepository dataRepository)
   {
      if (!UsesObservedData(dataRepository))
         return;

      Charts.Each(chart =>
      {
         var curvesToRemove = chart.Curves.Where(c => Equals(c.yData.Repository, dataRepository)).ToList();
         curvesToRemove.Each(curve => chart.RemoveCurve(curve.Id));
      });

      HasChanged = true;
   }

   public void RemoveOutputMappings(DataRepository dataRepository)
   {
      var outputsMatchingDeletedObservedData = OutputMappings.OutputMappingsUsingDataRepository(dataRepository).ToList();
      outputsMatchingDeletedObservedData.Each(OutputMappings.Remove);
   }

   public IEnumerable<CurveChart> Charts => _allSimulationAnalyses.OfType<CurveChart>();

   public void AddChartTemplate(CurveChartTemplate chartTemplate) => Settings.AddChartTemplate(chartTemplate);

   public void RemoveChartTemplate(string chartTemplateName) => Settings.RemoveChartTemplate(chartTemplateName);

   public IEnumerable<CurveChartTemplate> ChartTemplates => Settings.ChartTemplates;

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