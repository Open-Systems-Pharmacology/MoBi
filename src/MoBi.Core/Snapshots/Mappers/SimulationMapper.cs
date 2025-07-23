using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
   private readonly ValueOriginMapper _valueOriginMapper;

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
      IQuantityValueInSimulationChangeTracker quantityChangeTracker,
      ValueOriginMapper valueOriginMapper)
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
      _valueOriginMapper = valueOriginMapper;
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

      updateScaleDivisors(simulation, snapshot.ScaleDivisors, context);

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

      snapshot.ScaleDivisors = allScaleDivisorsChangedByUserFrom(simulation);

      return snapshot;
   }

   private void updateParameters(MoBiSimulation simulation, LocalizedParameter[] snapshotParameters, SnapshotContext snapshotContext)
   {
      snapshotParameters?.Each(x =>
      {
         var parameter = new ObjectPath(x.Path.ToPathArray()).TryResolve<IParameter>(simulation.Model.Root);
         if (parameter != null)
            changeQuantity(parameter, x, simulation, snapshotContext);
      });
   }

   private void updateScaleDivisors(MoBiSimulation simulation, ScaleDivisor[] snapshotScaleDivisors, SnapshotContext snapshotContext)
   {
      snapshotScaleDivisors?.Each(x =>
      {
         var moleculeAmount = new ObjectPath(x.Path.ToPathArray()).TryResolve<MoleculeAmount>(simulation.Model.Root);
         if (moleculeAmount != null)
            changeScaleDivisor(moleculeAmount, x, simulation, snapshotContext);
      });
   }

   private void changeScaleDivisor(MoleculeAmount moleculeAmount, ScaleDivisor scaleDivisor, MoBiSimulation simulation, SnapshotContext snapshotContext)
   {
      _quantityChangeTracker.TrackScaleChange(moleculeAmount, simulation, x =>
      {
         x.ScaleDivisor = scaleDivisor.Value;
      }, withEvents: false);
   }

   private void changeQuantity(IParameter parameter, LocalizedParameter snapshotParameter, MoBiSimulation simulation, SnapshotContext snapshotContext)
   {
      if (!snapshotParameter.Value.HasValue)
         return;

      _quantityChangeTracker.TrackQuantityChange(parameter, simulation, x =>
      {
         parameter.IsDefault = false;
         x.Value = snapshotParameter.Value.Value;
         x.DisplayUnit = _dimensionFactory.FindUnit(snapshotParameter.Unit).unit;
         _valueOriginMapper.UpdateValueOrigin(x.ValueOrigin, snapshotParameter.ValueOrigin);
      }, withEvents: false);
   }

   private ScaleDivisor[] allScaleDivisorsChangedByUserFrom(MoBiSimulation simulation)
   {
      var allAmountsToExport = new List<ScaleDivisor>();
      simulation.OriginalQuantityValues.Where(x => x.Type == OriginalQuantityValue.Types.ScaleDivisor).Each(x =>
      {
         var moleculeAmount = new ObjectPath(x.Path.ToPathArray()).TryResolve<MoleculeAmount>(simulation.Model.Root);
         if (moleculeAmount != null)
            allAmountsToExport.Add(new ScaleDivisor {Path = x.Path, Value = moleculeAmount.ScaleDivisor});
      });

      return allAmountsToExport.ToArray();
   }

   private Task<LocalizedParameter[]> allParametersChangedByUserFrom(MoBiSimulation simulation)
   {
      var allParametersToExport = new List<IParameter>();
      simulation.OriginalQuantityValues.Where(x => x.Type == OriginalQuantityValue.Types.Quantity).Each(x =>
      {
         var parameter = new ObjectPath(x.Path.ToPathArray()).TryResolve<IParameter>(simulation.Model.Root);
         if (parameter != null && parameter.ShouldExportToSnapshot())
            allParametersToExport.Add(parameter);
      });
      
      return _parameterMapper.LocalizedParametersFrom(allParametersToExport);
   }
}