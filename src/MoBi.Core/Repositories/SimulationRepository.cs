using System.Collections.Generic;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;

namespace MoBi.Core.Repositories
{
   public class SimulationRepository : ISimulationRepository
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public SimulationRepository(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IEnumerable<ISimulation> All()
      {
         return _projectRetriever.Current.Simulations;
      }
   }
}