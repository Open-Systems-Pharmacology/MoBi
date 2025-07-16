using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Extensions;
using System.Threading.Tasks;

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
   private readonly ICoreSimulationRunner _simulationRunner;

   public SimulationMapper(
      SimulationConfigurationMapper simulationConfigurationMapper,
      OutputMappingMapper outputMappingMapper,
      CurveChartMapper curveChartMapper,
      ISimulationFactory simulationFactory,
      OutputSelectionsMapper outputSelectionsMapper,
      SimulationPredictedVsObservedChartMapper predictedVsObservedChartMapper,
      SimulationResidualVsTimeChartMapper residualsVsTimeChartMapper,
      IOSPSuiteLogger logger,
      ICoreSimulationRunner simulationRunner)
   {
      _simulationConfigurationMapper = simulationConfigurationMapper;
      _outputMappingMapper = outputMappingMapper;
      _curveChartMapper = curveChartMapper;
      _simulationFactory = simulationFactory;
      _outputSelectionsMapper = outputSelectionsMapper;
      _predictedVsObservedChartMapper = predictedVsObservedChartMapper;
      _residualsVsTimeChartMapper = residualsVsTimeChartMapper;
      _logger = logger;
      _simulationRunner = simulationRunner;
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

      return snapshot;
   }
}