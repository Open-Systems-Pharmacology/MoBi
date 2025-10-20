using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public interface IInitialConditionsBuildingBlockExtendManager : IExtendPathAndValuesManager<InitialCondition>
   {
   }

   public class InitialConditionsBuildingBlockExtendManager : ExtendPathAndValuesManager<InitialCondition>, IInitialConditionsBuildingBlockExtendManager
   {
   }
}