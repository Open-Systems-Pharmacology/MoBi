using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Tasks
{
   public interface IInitialConditionsBuildingBlockExtendManager : IExtendStartValuesManager<InitialCondition>
   {
   }

   public class InitialConditionsBuildingBlockExtendManager : ExtendStartValuesManager<InitialCondition>, IInitialConditionsBuildingBlockExtendManager
   {
      public InitialConditionsBuildingBlockExtendManager(
         IApplicationController applicationController,
         IInitialConditionToObjectBaseSummaryDTOMapper dtoMapper,
         IMoBiContext context)
         : base(applicationController, dtoMapper, context)
      {
      }
   }
}