using MoBi.Core.Domain.Model;

namespace MoBi.R.Domain;

public class MoBiSimulation : OSPSuite.R.Domain.Simulation
{
   public MoBiSimulation(IMoBiSimulation modelCoreSimulation) : base(modelCoreSimulation)
   {
   }
   
   public IMoBiSimulation Simulation => CoreSimulation as IMoBiSimulation;
}