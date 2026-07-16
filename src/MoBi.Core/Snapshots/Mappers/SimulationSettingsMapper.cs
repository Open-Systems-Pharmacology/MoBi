using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots.Mappers;
using System.Threading.Tasks;
using ModelSimulationSettings = OSPSuite.Core.Domain.Builder.SimulationSettings;

namespace MoBi.Core.Snapshots.Mappers;

public class SimulationSettingsMapper : ObjectBaseSnapshotMapperBase<ModelSimulationSettings, SimulationSettings, SimulationContext>
{
   private readonly SolverSettingsMapper _solverSettingsMapper;
   private readonly IObjectBaseFactory _objectBaseFactory;
   private readonly OutputSchemaMapper _outputSchemaMapper;


   public SimulationSettingsMapper(OutputSchemaMapper outputSchemaMapper, SolverSettingsMapper solverSettingsMapper, IObjectBaseFactory objectBaseFactory)
   {
      _outputSchemaMapper = outputSchemaMapper;
      _solverSettingsMapper = solverSettingsMapper;
      _objectBaseFactory = objectBaseFactory;
   }

   public override async Task<ModelSimulationSettings> MapToModel(SimulationSettings snapshot, SimulationContext context)
   {
      var settings = _objectBaseFactory.Create<ModelSimulationSettings>();
      MapSnapshotPropertiesToModel(snapshot, settings);
      settings.OutputSchema = await _outputSchemaMapper.MapToModel(snapshot.OutputSchema, context);
      settings.Solver = await _solverSettingsMapper.MapToModel(snapshot.Solver, context);
      settings.RandomSeed = snapshot.RandomSeed;
      return settings;
   }

   public override async Task<SimulationSettings> MapToSnapshot(ModelSimulationSettings model)
   {
      var snapshot = await SnapshotFrom(model);
      snapshot.Solver = await _solverSettingsMapper.MapToSnapshot(model.Solver);
      snapshot.OutputSchema = await _outputSchemaMapper.MapToSnapshot(model.OutputSchema);
         
      snapshot.RandomSeed = model.RandomSeed;

      return snapshot;
   }
}