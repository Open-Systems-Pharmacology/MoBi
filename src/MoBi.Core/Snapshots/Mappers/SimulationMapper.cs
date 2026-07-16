using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Core.Chart;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Domain.UnitSystem;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Extensions;
using ModelSimulationPredictedVsObservedChart = OSPSuite.Core.Chart.Simulations.SimulationPredictedVsObservedChart;
using ModelSimulationResidualVsTimeChart = OSPSuite.Core.Chart.Simulations.SimulationResidualVsTimeChart;

namespace MoBi.Core.Snapshots.Mappers;

public class SimulationMapper : ObjectBaseSnapshotMapperBase<MoBiSimulation, Simulation, SimulationContext, MoBiProject>
{
   private readonly OutputMappingMapper _outputMappingMapper;
   private readonly MoBiSimulationTimeProfileChartMapper _timeProfileChartMapper;
   private readonly ISimulationFactory _simulationFactory;
   private readonly SimulationConfigurationMapper _simulationConfigurationMapper;
   private readonly OutputSelectionsMapper _outputSelectionsMapper;
   private readonly SimulationPredictedVsObservedChartMapper _predictedVsObservedChartMapper;
   private readonly SimulationResidualVsTimeChartMapper _residualsVsTimeChartMapper;
   private readonly IOSPSuiteLogger _logger;
   private readonly ParameterMapper _parameterMapper;
   private readonly IMoBiDimensionFactory _dimensionFactory;
   private readonly IQuantityValueInSimulationChangeTracker _quantityChangeTracker;
   private readonly ValueOriginMapper _valueOriginMapper;

   public SimulationMapper(
      SimulationConfigurationMapper simulationConfigurationMapper,
      OutputMappingMapper outputMappingMapper,
      MoBiSimulationTimeProfileChartMapper timeProfileChartMapper,
      ISimulationFactory simulationFactory,
      OutputSelectionsMapper outputSelectionsMapper,
      SimulationPredictedVsObservedChartMapper predictedVsObservedChartMapper,
      SimulationResidualVsTimeChartMapper residualsVsTimeChartMapper,
      IOSPSuiteLogger logger,
      ParameterMapper parameterMapper,
      IMoBiDimensionFactory dimensionFactory,
      IQuantityValueInSimulationChangeTracker quantityChangeTracker,
      ValueOriginMapper valueOriginMapper)
   {
      _simulationConfigurationMapper = simulationConfigurationMapper;
      _outputMappingMapper = outputMappingMapper;
      _timeProfileChartMapper = timeProfileChartMapper;
      _simulationFactory = simulationFactory;
      _outputSelectionsMapper = outputSelectionsMapper;
      _predictedVsObservedChartMapper = predictedVsObservedChartMapper;
      _residualsVsTimeChartMapper = residualsVsTimeChartMapper;
      _logger = logger;
      _parameterMapper = parameterMapper;
      _dimensionFactory = dimensionFactory;
      _quantityChangeTracker = quantityChangeTracker;
      _valueOriginMapper = valueOriginMapper;
   }

   public override async Task<MoBiSimulation> MapToModel(Simulation snapshot, SimulationContext context)
   {
      _logger.AddInfo(Captions.LoadingSimulation(snapshot.Name, context.NumberOfSimulationsLoaded + 1, context.NumberOfSimulationsToLoad), context.Project.Name);
      var configuration = await _simulationConfigurationMapper.MapToModel(snapshot.Configuration, context);

      var (simulation, _) = _simulationFactory.CreateSimulationAndValidate(configuration, snapshot.Name);
      var mobiSimulation = simulation as MoBiSimulation;

      var snapshotContextWithSimulation = new SnapshotContextWithSimulation(mobiSimulation, context);
      mobiSimulation.Settings.OutputSelections = await _outputSelectionsMapper.MapToModel(snapshot.OutputSelections, snapshotContextWithSimulation);

      var simulationAnalysisContext = new SimulationAnalysisContext(context.Project.AllObservedData, context);

      snapshot.Charts?.Each(x => mobiSimulation.AddAnalysis(_timeProfileChartMapper.MapToModel(x, simulationAnalysisContext).Result));
      snapshot.PredictedVsObservedCharts?.Each(x => mobiSimulation.AddAnalysis(_predictedVsObservedChartMapper.MapToModel(x, simulationAnalysisContext).Result));
      snapshot.ResidualVsTimeCharts?.Each(x => mobiSimulation.AddAnalysis(_residualsVsTimeChartMapper.MapToModel(x, simulationAnalysisContext).Result));

      snapshot.OutputMappings?.Each(x => mobiSimulation.OutputMappings.Add(_outputMappingMapper.MapToModel(x, snapshotContextWithSimulation).Result));

      updateParameters(mobiSimulation, snapshot.Parameters);

      updateScaleDivisors(mobiSimulation, snapshot.ScaleDivisors);

      return mobiSimulation;
   }

   public override async Task<Simulation> MapToSnapshot(MoBiSimulation simulation, MoBiProject project)
   {
      var snapshot = await SnapshotFrom(simulation);
      snapshot.OutputMappings = await _outputMappingMapper.MapToSnapshots(simulation.OutputMappings.All);
      snapshot.Configuration = await _simulationConfigurationMapper.MapToSnapshot(simulation.Configuration);
      snapshot.OutputSelections = await _outputSelectionsMapper.MapToSnapshot(simulation.OutputSelections);

      var timeProfileCharts = simulation.Analyses.OfType<MoBiSimulationTimeProfileChart>().ToList();
      if (timeProfileCharts.Any())
         snapshot.Charts = await Task.WhenAll(timeProfileCharts.Select(c => _timeProfileChartMapper.MapToSnapshot(c)));

      var predictedVsObservedCharts = simulation.Analyses.OfType<ModelSimulationPredictedVsObservedChart>().ToList();
      if (predictedVsObservedCharts.Any())
         snapshot.PredictedVsObservedCharts = await Task.WhenAll(predictedVsObservedCharts.Select(c => _predictedVsObservedChartMapper.MapToSnapshot(c)));

      var residualCharts = simulation.Analyses.OfType<ModelSimulationResidualVsTimeChart>().ToList();
      if (residualCharts.Any())
         snapshot.ResidualVsTimeCharts = await Task.WhenAll(residualCharts.Select(c => _residualsVsTimeChartMapper.MapToSnapshot(c)));

      snapshot.ParameterIdentificationWorkingDirectory = simulation.ParameterIdentificationWorkingDirectory;

      snapshot.Parameters = await allParametersChangedByUserFrom(simulation);

      snapshot.ScaleDivisors = allScaleDivisorsChangedByUserFrom(simulation);

      return snapshot;
   }

   private void updateParameters(MoBiSimulation simulation, LocalizedParameter[] snapshotParameters)
   {
      snapshotParameters?.Each(x =>
      {
         var parameter = simulation.Model.Root.EntityAt<IParameter>(x.Path);
         if (parameter != null)
            changeQuantity(parameter, x, simulation);
      });
   }

   private void updateScaleDivisors(MoBiSimulation simulation, ScaleDivisor[] snapshotScaleDivisors)
   {
      snapshotScaleDivisors?.Each(x =>
      {
         var moleculeAmount = simulation.Model.Root.EntityAt<MoleculeAmount>(x.Path);
         if (moleculeAmount != null)
            changeScaleDivisor(moleculeAmount, x, simulation);
      });
   }

   private void changeScaleDivisor(MoleculeAmount moleculeAmount, ScaleDivisor scaleDivisor, MoBiSimulation simulation)
   {
      _quantityChangeTracker.TrackScaleChange(moleculeAmount, simulation, x => x.ScaleDivisor = scaleDivisor.Value);
   }

   private void changeQuantity(IParameter parameter, LocalizedParameter snapshotParameter, MoBiSimulation simulation)
   {
      if (!snapshotParameter.Value.HasValue)
         return;

      _quantityChangeTracker.TrackQuantityChange(parameter, simulation, x =>
      {
         parameter.IsDefault = false;
         x.Value = snapshotParameter.Value.Value;
         x.DisplayUnit = _dimensionFactory.FindUnit(snapshotParameter.Unit).unit;
         _valueOriginMapper.UpdateValueOrigin(x.ValueOrigin, snapshotParameter.ValueOrigin);
      });
   }

   private ScaleDivisor[] allScaleDivisorsChangedByUserFrom(MoBiSimulation simulation)
   {
      var allAmountsToExport = new List<ScaleDivisor>();
      simulation.OriginalQuantityValues.Where(x => x.Type == OriginalQuantityValue.Types.ScaleDivisor).Each(x =>
      {
         var moleculeAmount = simulation.Model.Root.EntityAt<MoleculeAmount>(x.Path);
         if (moleculeAmount != null)
            allAmountsToExport.Add(new ScaleDivisor { Path = x.Path, Value = moleculeAmount.ScaleDivisor });
      });

      return allAmountsToExport.ToArray();
   }

   private Task<LocalizedParameter[]> allParametersChangedByUserFrom(MoBiSimulation simulation)
   {
      var allParametersToExport = new List<IParameter>();
      simulation.OriginalQuantityValues.Where(x => x.Type == OriginalQuantityValue.Types.Quantity).Each(x =>
      {
         var parameter = simulation.Model.Root.EntityAt<IParameter>(x.Path);
         if (parameter != null && parameter.ShouldExportToSnapshot())
            allParametersToExport.Add(parameter);
      });

      return _parameterMapper.LocalizedParametersFrom(allParametersToExport);
   }
}