using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface ISimulationLoader
   {
      ICommand AddSimulationToProject(IMoBiSimulation simulation);
      ICommand AddSimulationToProject(SimulationTransfer simulationTransfer);
   }

   internal class SimulationLoader : ISimulationLoader
   {
      private readonly INameCorrector _nameCorrector;
      private readonly IMoBiContext _context;
      private readonly ICloneManagerForSimulation _cloneManager;

      public SimulationLoader(ICloneManagerForSimulation cloneManager, INameCorrector nameCorrector, IMoBiContext context)
      {
         _cloneManager = cloneManager;
         _nameCorrector = nameCorrector;
         _context = context;
      }

      public ICommand AddSimulationToProject(IMoBiSimulation simulation)
      {
         var loadCommand = createLoadCommand(simulation);
         var shouldCloneSimulation = _context.CurrentProject.Simulations.ExistsById(simulation.Id);
         addSimulationToProject(simulation, loadCommand, shouldCloneSimulation);
         return loadCommand.Run(_context);
      }

      private static MoBiMacroCommand createLoadCommand(IMoBiSimulation simulation)
      {
         return new MoBiMacroCommand
         {
            Description = AppConstants.Commands.AddToProjectDescription("PK-Sim Simulation", simulation.Name)
         };
      }

      private void addSimulationToProject(IMoBiSimulation simulation, MoBiMacroCommand loadCommand, bool shouldCloneSimulation)
      {
         var moBiSimulation = simulation;

         simulation.MoBiBuildConfiguration.AllBuildingBlockInfos().Each(checkTemplateBuildingBlock);
         var project = _context.CurrentProject;

         if (shouldCloneSimulation)
            moBiSimulation = cloneSimulation(moBiSimulation);

         addBuildConfigurationToProject(project, moBiSimulation.MoBiBuildConfiguration, loadCommand);

         moBiSimulation.Results = simulation.Results;
         if (!_nameCorrector.CorrectName(project.Simulations, moBiSimulation))
            return;

         moBiSimulation.HasChanged = true;
         loadCommand.AddCommand(new AddSimulationCommand(moBiSimulation));
      }

      public ICommand AddSimulationToProject(SimulationTransfer simulationTransfer)
      {
         var project = _context.CurrentProject;
         project.Favorites.AddFavorites(simulationTransfer.Favorites);
         var simulation = simulationTransfer.Simulation.DowncastTo<IMoBiSimulation>();
         var loadCommand = createLoadCommand(simulation);
         //We always clone the simulation from Transfer as it may be loaded twice
         addSimulationToProject(simulation, loadCommand, shouldCloneSimulation: true);
         addObservedDataToProject(simulationTransfer.AllObservedData, loadCommand);
         return loadCommand.Run(_context);
      }

      private void addObservedDataToProject(IEnumerable<DataRepository> allObservedData, MoBiMacroCommand loadCommand)
      {
         if (allObservedData == null)
            return;

         var project = _context.CurrentProject;

         allObservedData.Where(observedData => !project.AllObservedData.ExistsById(observedData.Id))
            .Each(x => loadCommand.AddCommand(new AddObservedDataToProjectCommand(x)));
      }

      private void checkTemplateBuildingBlock(IBuildingBlockInfo buildingBlockInfo)
      {
         if (!string.IsNullOrEmpty(buildingBlockInfo.TemplateBuildingBlockId)) return;
         buildingBlockInfo.TemplateBuildingBlockId = buildingBlockInfo.UntypedBuildingBlock.Id;
         buildingBlockInfo.UntypedTemplateBuildingBlock = buildingBlockInfo.UntypedBuildingBlock;
      }

      private IMoBiSimulation cloneSimulation(IMoBiSimulation simulation)
      {
         return _cloneManager.CloneSimulation(simulation);
      }

      private void addBuildConfigurationToProject(IMoBiProject project, IMoBiBuildConfiguration config, ICommandCollector commandCollector)
      {
         var copyMolecules = addToProject(commandCollector, createForProject(project.MoleculeBlockCollection, config.MoleculesInfo));
         if (copyMolecules == null)
            copyMolecules = config.Molecules;

         var copySpatialStructure = addToProject(commandCollector, createForProject(project.SpatialStructureCollection, config.SpatialStructureInfo));
         if (copySpatialStructure == null)
            copySpatialStructure = config.MoBiSpatialStructure;

         addToProject(commandCollector, createForProject(project.ReactionBlockCollection, config.ReactionsInfo));
         addToProject(commandCollector, createForProject(project.PassiveTransportCollection, config.PassiveTransportsInfo));
         addToProject(commandCollector, createForProject(project.ObserverBlockCollection, config.ObserversInfo));
         addToProject(commandCollector, createForProject(project.SimulationSettingsCollection, config.SimulationSettingsInfo));
         addToProject(commandCollector, createForProject(project.EventBlockCollection, config.EventGroupsInfo));

         var psv = addToProject(commandCollector, createForProject(project.ParametersStartValueBlockCollection, config.ParameterStartValuesInfo));
         if (psv != null)
         {
            updateTemplateBuildingBlockIds(psv, copyMolecules.Id, copySpatialStructure.Id, config.ParameterStartValuesInfo.BuildingBlock);
         }

         var msv = addToProject(commandCollector, createForProject(project.MoleculeStartValueBlockCollection, config.MoleculeStartValuesInfo));
         if (msv != null)
         {
            updateTemplateBuildingBlockIds(msv, copyMolecules.Id, copySpatialStructure.Id, config.MoleculeStartValuesInfo.BuildingBlock);
         }
      }

      private static void updateTemplateBuildingBlockIds<T>(IStartValuesBuildingBlock<T> startValues, string moleculeBuildingBlockId,
         string spatialStructureId, IStartValuesBuildingBlock<T> buildingBlock) where T : class, IStartValue
      {
         startValues.MoleculeBuildingBlockId = moleculeBuildingBlockId;
         startValues.SpatialStructureId = spatialStructureId;
         buildingBlock.MoleculeBuildingBlockId = startValues.MoleculeBuildingBlockId;
         buildingBlock.SpatialStructureId = startValues.SpatialStructureId;
      }

      private T createForProject<T>(IReadOnlyCollection<T> projectBuildingBlocks, IBuildingBlockInfo<T> buildingBlockInfo)
         where T : class, IBuildingBlock
      {
         //this was already added to the project
         var templateBuildingBlock = projectBuildingBlocks.FindById(buildingBlockInfo.TemplateBuildingBlockId);
         if (templateBuildingBlock != null)
         {
            buildingBlockInfo.TemplateBuildingBlock = templateBuildingBlock;
            return null;
         }

         templateBuildingBlock = buildingBlockInfo.BuildingBlock;
         if (!_nameCorrector.CorrectName(projectBuildingBlocks, templateBuildingBlock))
            return null;

         buildingBlockInfo.TemplateBuildingBlock = templateBuildingBlock;
         buildingBlockInfo.BuildingBlock = _cloneManager.CloneBuidingBlock(templateBuildingBlock);
         return templateBuildingBlock;
      }

      private T addToProject<T>(ICommandCollector commandCollector, T buildingBlock) where T : class, IBuildingBlock
      {
         if (buildingBlock != null)
            commandCollector.AddCommand(new AddBuildingBlockCommand<T>(buildingBlock));

         return buildingBlock;
      }
   }
}