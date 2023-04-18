using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services
{
   public interface ISimulationConfigurationFactory
   {
      SimulationConfiguration Create();
   }
   
   public class SimulationConfigurationFactory : ISimulationConfigurationFactory
   {
      private readonly ICoreCalculationMethodRepository _calculationMethodRepository;
      private readonly ICloneManagerForBuildingBlock _cloneManager;
      private readonly IMoBiContext _context;

      public SimulationConfigurationFactory(ICoreCalculationMethodRepository calculationMethodRepository, ICloneManagerForBuildingBlock cloneManager, IMoBiContext context)
      {
         _calculationMethodRepository = calculationMethodRepository;
         _cloneManager = cloneManager;
         _context = context;
      }

      public SimulationConfiguration Create()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = _cloneManager.Clone(_context.CurrentProject.SimulationSettings)
         };

         _calculationMethodRepository.All().Each(
            cm => simulationConfiguration.AddCalculationMethod(_cloneManager.Clone(cm, new FormulaCache())));

         return simulationConfiguration;
      }
   }
}
