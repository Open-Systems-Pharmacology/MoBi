using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
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
      private readonly IMoBiProjectRetriever _projectRetriever;

      public SimulationConfigurationFactory(ICoreCalculationMethodRepository calculationMethodRepository, ICloneManagerForBuildingBlock cloneManager, IMoBiProjectRetriever projectRetriever)
      {
         _calculationMethodRepository = calculationMethodRepository;
         _cloneManager = cloneManager;
         _projectRetriever = projectRetriever;
      }

      public SimulationConfiguration Create()
      {
         var simulationConfiguration = new SimulationConfiguration
         {
            SimulationSettings = _cloneManager.Clone(_projectRetriever.Current.SimulationSettings)
         };

         _calculationMethodRepository.All().Each(simulationConfiguration.AddCalculationMethod);

         return simulationConfiguration;
      }
   }
}
