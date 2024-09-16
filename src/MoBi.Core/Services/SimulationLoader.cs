using System;
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

         var project = _context.CurrentProject;

         renameCollidingEntities(simulation.Modules, project.Modules);
         renameCollidingEntities(simulation.Configuration.ExpressionProfiles, project.ExpressionProfileCollection);

         if (simulation.Configuration.Individual != null)
            renameCollidingEntities(new[] { simulation.Configuration.Individual }, project.IndividualsCollection);

         if (shouldCloneSimulation)
            moBiSimulation = cloneSimulation(moBiSimulation);

         addSimulationConfigurationToProject(moBiSimulation, loadCommand);

         moBiSimulation.ResultsDataRepository = simulation.ResultsDataRepository;
         if (!_nameCorrector.CorrectName(project.Simulations, moBiSimulation))
            return;

         moBiSimulation.HasChanged = true;
         loadCommand.AddCommand(new AddSimulationCommand(moBiSimulation));
      }

      private void renameCollidingEntities(IEnumerable<IObjectBase> entitiesToRename, IReadOnlyList<IWithName> existingEntities)
      {
         var takenNames = existingEntities.AllNames();
         entitiesToRename.Where(x => takenNames.Contains(x.Name)).Each(x => _nameCorrector.AutoCorrectName(takenNames, x));
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

      private IMoBiSimulation cloneSimulation(IMoBiSimulation simulation)
      {
         return _cloneManager.CloneSimulation(simulation);
      }

      private void addSimulationConfigurationToProject(IMoBiSimulation simulation, ICommandCollector commandCollector)
      {
         var cloneForProjectEntities = _cloneManager.CloneSimulationConfiguration(simulation.Configuration);
         cloneForProjectEntities.ModuleConfigurations.Each(moduleConfiguration => commandCollector.AddCommand(new AddModuleCommand(moduleConfiguration.Module) { Silent = true }));

         addToProject(commandCollector, cloneForProjectEntities.Individual, individual => new AddIndividualBuildingBlockToProjectCommand(individual));
         cloneForProjectEntities.ExpressionProfiles.Each(expressionProfile => addToProject(commandCollector, expressionProfile, ep => new AddExpressionProfileBuildingBlockToProjectCommand(ep)));
      }

      private T addToProject<T>(ICommandCollector commandCollector, T buildingBlock, Func<T, ICommand> commandCreatorFunc) where T : class, IBuildingBlock
      {
         if (buildingBlock != null)
            commandCollector.AddCommand(commandCreatorFunc(buildingBlock));

         return buildingBlock;
      }
   }
}