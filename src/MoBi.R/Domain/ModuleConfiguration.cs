using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Domain;

public class ModuleConfiguration
{
   public Module Module { get; set; }
   public ParameterValuesBuildingBlock SelectedParameterValue { get; set; }
   public InitialConditionsBuildingBlock SelectedInitialCondition { get; set; }
}

public class MoBiSimulation : OSPSuite.R.Domain.Simulation
{
   public MoBiSimulation(IMoBiSimulation modelCoreSimulation) : base(modelCoreSimulation)
   {
   }
   
   public IMoBiSimulation Simulation => CoreSimulation as IMoBiSimulation;
}