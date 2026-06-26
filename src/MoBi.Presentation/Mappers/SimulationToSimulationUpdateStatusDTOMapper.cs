using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers;

public interface ISimulationToSimulationUpdateStatusDTOMapper : IMapper<IMoBiSimulation, SimulationUpdateStatusDTO>
{
}

public class SimulationToSimulationUpdateStatusDTOMapper : ISimulationToSimulationUpdateStatusDTOMapper
{
   public SimulationUpdateStatusDTO MapFrom(IMoBiSimulation simulation) =>
      new SimulationUpdateStatusDTO { SimulationId = simulation.Id, Name = simulation.Name, Status = RunStatus.WaitingToRun };
}