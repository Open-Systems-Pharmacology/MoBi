using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface ISolverSettingsToDTOSolverSettingsMapper : IMapper<SolverSettings, SolverSettingsDTO>
   {
   }

   public class SolverSettingsToDTOSolverSettingsMapper : ISolverSettingsToDTOSolverSettingsMapper
   {
      public SolverSettingsDTO MapFrom(SolverSettings solverSettings)
      {
         var dto = new SolverSettingsDTO
         {
            Name = solverSettings.Name,
            SolverOptions = dtoBaseOptionsFrom(solverSettings)
         };
         return dto;
      }

      private IEnumerable<ISolverOptionDTO> dtoBaseOptionsFrom(SolverSettings input)
      {
         return new List<ISolverOptionDTO>
         {
            new SolverOptionDTO<double>("AbsTol", input.AbsTol, ToolTips.SolverOptions.AbsTol),
            new SolverOptionDTO<double>("RelTol", input.RelTol, ToolTips.SolverOptions.RelTol),
            new SolverOptionDTO<double>("H0", input.H0, ToolTips.SolverOptions.H0),
            new SolverOptionDTO<double>("HMin", input.HMin, ToolTips.SolverOptions.HMin),
            new SolverOptionDTO<double>("HMax", input.HMax, ToolTips.SolverOptions.HMax),
            new SolverOptionDTO<int>("MxStep", input.MxStep, ToolTips.SolverOptions.MxStep),
            new SolverOptionDTO<bool>("UseJacobian", input.UseJacobian, ToolTips.SolverOptions.UseJacobian)
         };
      }
   }
}