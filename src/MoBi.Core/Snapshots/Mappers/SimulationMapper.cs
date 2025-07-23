using System.Collections.Generic;
using System.Threading.Tasks;
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

namespace MoBi.Core.Snapshots.Mappers;

public class SimulationMapper : ObjectBaseSnapshotMapperBase<MoBiSimulation, Simulation, SimulationContext, MoBiProject>
{
   private readonly OutputMappingMapper _outputMappingMapper;
   private readonly CurveChartMapper _curveChartMapper;
   private readonly ISimulationFactory _simulationFactory;
   private readonly SimulationConfigurationMapper _simulationConfigurationMapper;
   private readonly OutputSelectionsMapper _outputSelectionsMapper;
   private readonly SimulationPredictedVsObservedChartMapper _predictedVsObservedChartMapper;
   private readonly SimulationResidualVsTimeChartMapper _residualsVsTimeChartMapper;
   private readonly IOSPSuiteLogger _logger;
   private readonly ParameterMapper _parameterMapper;
   private readonly ICoreSimulationRunner _simulationRunner;
   private readonly IMoBiDimensionFactory _dimensionFactory;
   private readonly IQuantityValueInSimulationChangeTracker _quantityChangeTracker;

   public SimulationMapper(
      SimulationConfigurationMapper simulationConfigurationMapper,
      OutputMappingMapper outputMappingMapper,
      CurveChartMapper curveChartMapper,
      ISimulationFactory simulationFactory,
      OutputSelectionsMapper outputSelectionsMapper,
      SimulationPredictedVsObservedChartMapper predictedVsObservedChartMapper,
      SimulationResidualVsTimeChartMapper residualsVsTimeChartMapper,
      IOSPSuiteLogger logger,
      ParameterMapper parameterMapper,
      ICoreSimulationRunner simulationRunner,
      IMoBiDimensionFactory dimensionFactory,
      IQuantityValueInSimulationChangeTracker quantityChangeTracker)
   {
      _simulationConfigurationMapper = simulationConfigurationMapper;
      _outputMappingMapper = outputMappingMapper;
      _curveChartMapper = curveChartMapper;
      _simulationFactory = simulationFactory;
      _outputSelectionsMapper = outputSelectionsMapper;
      _predictedVsObservedChartMapper = predictedVsObservedChartMapper;
      _residualsVsTimeChartMapper = residualsVsTimeChartMapper;
      _logger = logger;
      _parameterMapper = parameterMapper;
      _simulationRunner = simulationRunner;
      _dimensionFactory = dimensionFactory;
      _quantityChangeTracker = quantityChangeTracker;
   }

   public override async Task<MoBiSimulation> MapToModel(Simulation snapshot, SimulationContext context)
   {
      _logger.AddInfo(Captions.LoadingSimulation(snapshot.Name, context.NumberOfSimulationsLoaded + 1, context.NumberOfSimulationsToLoad), context.Project.Name);
      var configuration = await _simulationConfigurationMapper.MapToModel(snapshot.Configuration, context);

      var simulation = _simulationFactory.CreateSimulationAndValidate(configuration, snapshot.Name) as MoBiSimulation;

      var snapshotContextWithSimulation = new SnapshotContextWithSimulation(simulation, context);
      simulation.Settings.OutputSelections = await _outputSelectionsMapper.MapToModel(snapshot.OutputSelections, snapshotContextWithSimulation);

      var simulationAnalysisContext = new SimulationAnalysisContext(context.Project.AllObservedData, context);
      if (snapshot.Chart != null)
         simulation.Chart = await _curveChartMapper.MapToModel(snapshot.Chart, simulationAnalysisContext);

      if (snapshot.SimulationPredictedVsObservedChart != null)
         simulation.PredictedVsObservedChart = await _predictedVsObservedChartMapper.MapToModel(snapshot.SimulationPredictedVsObservedChart, simulationAnalysisContext);

      if (snapshot.SimulationResidualVsTimeChart != null)
         simulation.ResidualVsTimeChart = await _residualsVsTimeChartMapper.MapToModel(snapshot.SimulationResidualVsTimeChart, simulationAnalysisContext);

      snapshot.OutputMappings?.Each(x => simulation.OutputMappings.Add(_outputMappingMapper.MapToModel(x, snapshotContextWithSimulation).Result));

      updateParameters(simulation, snapshot.Parameters, context);

      if (context.Run)
         await _simulationRunner.RunSimulationAsync(simulation);

      return simulation;
   }

   public override async Task<Simulation> MapToSnapshot(MoBiSimulation simulation, MoBiProject project)
   {
      var snapshot = await SnapshotFrom(simulation);
      snapshot.OutputMappings = await _outputMappingMapper.MapToSnapshots(simulation.OutputMappings.All);
      snapshot.Configuration = await _simulationConfigurationMapper.MapToSnapshot(simulation.Configuration);
      snapshot.OutputSelections = await _outputSelectionsMapper.MapToSnapshot(simulation.OutputSelections);

      if (simulation.Chart != null)
         snapshot.Chart = await _curveChartMapper.MapToSnapshot(simulation.Chart);

      if (simulation.PredictedVsObservedChart != null)
         snapshot.SimulationPredictedVsObservedChart = await _predictedVsObservedChartMapper.MapToSnapshot(simulation.PredictedVsObservedChart);
      if (simulation.ResidualVsTimeChart != null)
         snapshot.SimulationResidualVsTimeChart = await _residualsVsTimeChartMapper.MapToSnapshot(simulation.ResidualVsTimeChart);

      snapshot.ParameterIdentificationWorkingDirectory = simulation.ParameterIdentificationWorkingDirectory;

      snapshot.Parameters = await allParametersChangedByUserFrom(simulation);

      return snapshot;
   }

   private void updateParameters(MoBiSimulation simulation, LocalizedParameter[] snapshotParameters, SnapshotContext snapshotContext)
   {
      snapshotParameters.Each(snapshotParameter =>
      {
         var parameter = new ObjectPath(snapshotParameter.Path.ToPathArray()).TryResolve<IParameter>(simulation.Model.Root);
         if (parameter != null && snapshotParameter.Value.HasValue)
            changeQuantity(parameter, snapshotParameter.Value.Value, snapshotParameter.Unit, simulation);
      });
   }

   private void changeQuantity(IQuantity quantity, double snapshotParameterValue, string snapshotParameterUnit, MoBiSimulation simulation)
   {
      _quantityChangeTracker.TrackQuantityChange(quantity, simulation, x =>
      {
         x.Value = snapshotParameterValue;
         x.DisplayUnit = _dimensionFactory.FindUnit(snapshotParameterUnit).unit;
      }, withEvents: false);
   }

   private Task<LocalizedParameter[]> allParametersChangedByUserFrom(MoBiSimulation simulation)
   {
      var allParametersToExport = new List<IParameter>();
      simulation.OriginalQuantityValues.Each(x =>
      {
         var parameter = new ObjectPath(x.Path.ToPathArray()).TryResolve<IParameter>(simulation.Model.Root);
         if (parameter != null && parameter.ShouldExportToSnapshot())
            allParametersToExport.Add(parameter);
      });

      return _parameterMapper.LocalizedParametersFrom(allParametersToExport);
   }
}