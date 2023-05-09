using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation.Tasks
{
   public interface IInitialConditionsBuildingBlockExtendManager : IExtendPathAndValuesManager<InitialCondition>
   {
   }

   public class InitialConditionsBuildingBlockExtendManager : ExtendPathAndValuesManager<InitialCondition>, IInitialConditionsBuildingBlockExtendManager
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