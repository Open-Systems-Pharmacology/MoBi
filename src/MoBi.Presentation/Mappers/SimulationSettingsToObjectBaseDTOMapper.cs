using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Presentation.Mappers
{
   public interface ISimulationSettingsToObjectBaseDTOMapper : 
      IMapper<SolverSettings, IObjectBaseDTO>,
      IMapper<OutputSchema, IObjectBaseDTO>,
      IMapper<ISimulationSettings, IObjectBaseDTO>
   {
   }

   public class SimulationSettingsToObjectBaseDTOMapper : ISimulationSettingsToObjectBaseDTOMapper
   {
      public IObjectBaseDTO MapFrom(SolverSettings solverSettings)
      {
         return new ObjectBaseDTO
         {
            Id = AppConstants.SolverSettingsId,
            Icon = ApplicationIcons.Solver.IconName,
            Name = AppConstants.Captions.SolverSettings,
         };
      }

      public IObjectBaseDTO MapFrom(OutputSchema outputSchema)
      {
         return new ObjectBaseDTO
         {
            Id = AppConstants.OutputIntervalId,
            Icon = ApplicationIcons.OutputInterval.IconName,
            Name = AppConstants.Captions.OutputIntervals,
         };
      }

      public IObjectBaseDTO MapFrom(ISimulationSettings input)
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