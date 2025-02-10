using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
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
         return loadCommand.RunCommand(_context);
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

         if (shouldCloneSimulation)
            moBiSimulation = cloneSimulation(moBiSimulation);

         renameCollidingEntities(moBiSimulation.Configuration.ExpressionProfiles, project.ExpressionProfileCollection);

         if (moBiSimulation.Configuration.Individual != null)
            renameCollidingEntities(new[] { moBiSimulation.Configuration.Individual }, project.IndividualsCollection);

         renameCollidingEntities(moBiSimulation.Modules, project.Modules);

         moBiSimulation.ResultsDataRepository = simulation.ResultsDataRepository;

         var originalSimulationName = moBiSimulation.Name;
         if (!_nameCorrector.CorrectName(project.Simulations, moBiSimulation))
            return;

         if (originalSimulationName != moBiSimulation.Name) //has been renamed
         {
            correctModuleNames(moBiSimulation.Modules, moBiSimulation.Name, project.Modules, originalSimulationName);
            loadCommand.Add(new RenameModelCommand(moBiSimulation.Model, moBiSimulation.Name));
         }


         addSimulationConfigurationToProject(moBiSimulation, loadCommand);

         moBiSimulation.HasChanged = true;
         loadCommand.AddCommand(new AddSimulationCommand(moBiSimulation));
      }

      private void correctModuleNames(IReadOnlyList<Module> modulesToRename, string simulationName, IReadOnlyList<Module> existingModules, string originalSimulationName)
      {
         //these names are unique so better creating a HashSet.
         var takenNames = existingModules.AllNames().ToHashSet();

         modulesToRename
            .Each(module=> renameModulesAfterSimulation(module, simulationName, originalSimulationName));

         // Correct any remaining name conflicts
         modulesToRename.Where(x => takenNames.Contains(x.Name)).Each(x => _nameCorrector.AutoCorrectName(takenNames, x));
      }

      private void renameModulesAfterSimulation(Module module, string simulationName, string originalSimulationName)
      {
         if (module.Name.StartsWith(originalSimulationName))
            module.Name = module.Name.Replace(originalSimulationName, simulationName);
      }
      
      private void renameCollidingEntities<T>(IEnumerable<T> entitiesToRename, IReadOnlyList<IWithName> existingEntities) where T : IObjectBase
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
         return loadCommand.RunCommand(_context);
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