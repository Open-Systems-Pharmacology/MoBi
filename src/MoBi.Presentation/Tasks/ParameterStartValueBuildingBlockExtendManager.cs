using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Tasks
{
   public interface IParameterStartValueBuildingBlockExtendManager : IExtendStartValuesManager<ParameterValue>
   {
   }

   public class ParameterStartValueBuildingBlockExtendManager : ExtendStartValuesManager<ParameterValue>, IParameterStartValueBuildingBlockExtendManager
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