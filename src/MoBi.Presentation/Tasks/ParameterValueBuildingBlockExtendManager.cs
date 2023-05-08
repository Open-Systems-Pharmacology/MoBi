using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Tasks
{
   public interface IParameterValueBuildingBlockExtendManager : IExtendStartValuesManager<ParameterValue>
   {
   }

   public class ParameterValueBuildingBlockExtendManager : ExtendStartValuesManager<ParameterValue>, IParameterValueBuildingBlockExtendManager
   {
      public ParameterValueBuildingBlockExtendManager(
         IApplicationController applicationController,
         IParameterValueToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }
}