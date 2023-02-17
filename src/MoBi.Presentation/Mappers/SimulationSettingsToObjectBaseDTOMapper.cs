using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface ISimulationSettingsToObjectBaseDTOMapper : 
      IMapper<SolverSettings, ObjectBaseDTO>,
      IMapper<OutputSchema, ObjectBaseDTO>,
      IMapper<ISimulationSettings, ObjectBaseDTO>
   {
   }

   public class SimulationSettingsToObjectBaseDTOMapper : ISimulationSettingsToObjectBaseDTOMapper
   {
      public ObjectBaseDTO MapFrom(SolverSettings solverSettings)
      {
         return new ObjectBaseDTO
         {
            Id = AppConstants.SolverSettingsId,
            Icon = ApplicationIcons.Solver.IconName,
            Name = AppConstants.Captions.SolverSettings,
         };
      }

      public ObjectBaseDTO MapFrom(OutputSchema outputSchema)
      {
         return new ObjectBaseDTO
         {
            Id = AppConstants.OutputIntervalId,
            Icon = ApplicationIcons.OutputInterval.IconName,
            Name = AppConstants.Captions.OutputIntervals,
         };
      }

      public ObjectBaseDTO MapFrom(ISimulationSettings input)
      {
         return new ObjectBaseDTO
         {
            Id = AppConstants.SimulationSettingsId,
            Icon = ApplicationIcons.Settings.IconName,
            Name = AppConstants.Captions.SimulationSettings,
         };
      }
   }
}