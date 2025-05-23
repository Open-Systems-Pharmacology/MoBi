using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public class TrackableSimulation
   {
      public IMoBiSimulation Simulation { get; }
      public SimulationEntitySourceReferenceCache ReferenceCache { get; }

      public TrackableSimulation(IMoBiSimulation simulation, SimulationEntitySourceReferenceCache referenceCache)
      {
         Simulation = simulation;
         ReferenceCache = referenceCache;
      }

      public SimulationEntitySourceReference SourceFor(IEntity parameter) => ReferenceCache[parameter];
   }
}