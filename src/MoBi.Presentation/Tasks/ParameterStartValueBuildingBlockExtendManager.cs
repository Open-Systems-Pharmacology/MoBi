using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Tasks
{
   public interface IParameterStartValueBuildingBlockExtendManager : IExtendStartValuesManager<ParameterStartValue>
   {
   }

   public class ParameterStartValueBuildingBlockExtendManager : ExtendStartValuesManager<ParameterStartValue>, IParameterStartValueBuildingBlockExtendManager
   {
      public ParameterStartValueBuildingBlockExtendManager(
         IApplicationController applicationController,
         IParameterStartValueToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }
}