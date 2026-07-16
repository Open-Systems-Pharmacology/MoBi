using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers;

public class ModuleConfigurationMapper : SnapshotMapperBase<OSPSuite.Core.Domain.ModuleConfiguration, ModuleConfiguration, SimulationContext>
{
   public override Task<OSPSuite.Core.Domain.ModuleConfiguration> MapToModel(ModuleConfiguration snapshot, SimulationContext context)
   {
      var module = context.MoBiProject().ModuleByName(snapshot.Module);
      var configuration = new OSPSuite.Core.Domain.ModuleConfiguration(module);

      if (!string.IsNullOrEmpty(snapshot.SelectedInitialConditions))
         configuration.SelectedInitialConditions = module.InitialConditionsCollection.FindByName(snapshot.SelectedInitialConditions);
      if (!string.IsNullOrEmpty(snapshot.SelectedParameterValues))
         configuration.SelectedParameterValues = module.ParameterValuesCollection.FindByName(snapshot.SelectedParameterValues);

      return Task.FromResult(configuration);
   }

   public override async Task<ModuleConfiguration> MapToSnapshot(OSPSuite.Core.Domain.ModuleConfiguration model)
   {
      var snapshot = await SnapshotFrom(model);

      snapshot.Module = model.Module.Name;

      if (model.SelectedInitialConditions != null)
         snapshot.SelectedInitialConditions = model.SelectedInitialConditions.Name;
      if (model.SelectedParameterValues != null)
         snapshot.SelectedParameterValues = model.SelectedParameterValues.Name;

      return snapshot;
   }
}