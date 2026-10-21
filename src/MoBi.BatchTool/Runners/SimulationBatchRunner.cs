using System.Linq;
using MoBi.BatchTool.Services;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.BatchTool.Runners
{
   public interface ISimulationBatchRunner
   {
      void Compute(IMoBiSimulation simulation);
   }

   public class SimulationBatchRunner : ISimulationBatchRunner
   {
      private readonly IBatchLogger _logger;
      private readonly ISimulationFactory _simulationFactory;
      private readonly IMoBiContext _context;
      private readonly ISimModelManager _simModelManager;

      public SimulationBatchRunner(IBatchLogger logger, ISimulationFactory simulationFactory,
         IMoBiContext context, ISimModelManager simModelManager)
      {
         _logger = logger;
         _simulationFactory = simulationFactory;
         _context = context;
         _simModelManager = simModelManager;
      }

      public void Compute(IMoBiSimulation simulation)
      {
         // var buildConfiguration = _buildConfigurationFactory.CreateFromReferencesUsedIn(simulation.MoBiBuildConfiguration);
         // buildConfiguration.ShowProgress = false;

         _logger.AddDebug("Creating new simulation from loaded building blocks");
         var results = _simulationFactory.CreateModelAndValidate(simulation.Configuration, "BatchRun", throwOnInvalid: false, validationOptions: simulation.ValidationOptions);
         if (results.IsInvalid)
            _logger.AddWarning(results.ValidationResult.Messages.SelectMany(x => x.Details).ToString());

         var newSimulation = new MoBiSimulation
         {
            // TODO SIMULATION_CONFIGURATION
            Configuration = simulation.Configuration,
            Model = results.Model,
            Id = "Sim",
            ValidationOptions = simulation.ValidationOptions
         };
         _context.Register(newSimulation);
         _logger.AddDebug("Running simulation");
         _simModelManager.RunSimulation(newSimulation);
      }
   }
}
