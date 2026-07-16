using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Snapshots.Mappers;

public class SimulationConfigurationMapper : SnapshotMapperBase<OSPSuite.Core.Domain.Builder.SimulationConfiguration, SimulationConfiguration, SimulationContext>
{
   private readonly ISimulationConfigurationFactory _simulationConfigurationFactory;
   private readonly SimulationSettingsMapper _simulationSettingsMapper;
   private readonly ModuleConfigurationMapper _moduleConfigurationMapper;

   public SimulationConfigurationMapper(ISimulationConfigurationFactory simulationConfigurationFactory, SimulationSettingsMapper simulationSettingsMapper, ModuleConfigurationMapper moduleConfigurationMapper)
   {
      _simulationConfigurationFactory = simulationConfigurationFactory;
      _simulationSettingsMapper = simulationSettingsMapper;
      _moduleConfigurationMapper = moduleConfigurationMapper;
   }

   public override async Task<OSPSuite.Core.Domain.Builder.SimulationConfiguration> MapToModel(SimulationConfiguration snapshot, SimulationContext context)
   {
      var simulationSettings = await _simulationSettingsMapper.MapToModel(snapshot.Settings, context);
      var configuration = _simulationConfigurationFactory.Create(simulationSettings);

      if (!string.IsNullOrEmpty(snapshot.IndividualBuildingBlock))
         configuration.Individual = projectIndividualFor(context, snapshot.IndividualBuildingBlock);

      snapshot.ExpressionProfiles.Each(x => configuration.AddExpressionProfile(projectExpressionProfileFor(context, x)));

      _moduleConfigurationMapper.MapToModels(snapshot.ModuleConfigurations, context).Result.Each(configuration.AddModuleConfiguration);

      return configuration;
   }

   private IndividualBuildingBlock projectIndividualFor(SimulationContext context, string buildingBlockName)
   {
      return context.MoBiProject().IndividualsCollection.FindByName(buildingBlockName);
   }

   private static ExpressionProfileBuildingBlock projectExpressionProfileFor(SimulationContext context, string x)
   {
      return context.MoBiProject().ExpressionProfileCollection.FindByName(x);
   }

   public override async Task<SimulationConfiguration> MapToSnapshot(OSPSuite.Core.Domain.Builder.SimulationConfiguration model)
   {
      var snapshot = await SnapshotFrom(model);
      snapshot.Settings = await _simulationSettingsMapper.MapToSnapshot(model.SimulationSettings);

      if (model.Individual != null)
         snapshot.IndividualBuildingBlock = buildingBlockNameFrom(model.Individual);

      snapshot.ExpressionProfiles = buildingBlockNamesFrom(model.ExpressionProfiles);

      snapshot.ModuleConfigurations = mapModuleConfigurations(model.ModuleConfigurations);

      return snapshot;
   }

   private ModuleConfiguration[] mapModuleConfigurations(IReadOnlyList<OSPSuite.Core.Domain.ModuleConfiguration> moduleConfigurations) => 
      _moduleConfigurationMapper.MapToSnapshots(moduleConfigurations).Result;

   private string[] buildingBlockNamesFrom(IReadOnlyList<IBuildingBlock> buildingBlocks) => 
      buildingBlocks.Select(buildingBlockNameFrom).ToArray();

   private string buildingBlockNameFrom(IBuildingBlock simulationBuildingBlock) => 
      simulationBuildingBlock.Name;
}