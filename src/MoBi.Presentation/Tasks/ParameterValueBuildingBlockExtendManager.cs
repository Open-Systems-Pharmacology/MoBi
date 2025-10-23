using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks
{
   public interface IParameterValueBuildingBlockExtendManager : IExtendPathAndValuesManager<ParameterValue>
   {
   }

   public class ParameterValueBuildingBlockExtendManager : ExtendPathAndValuesManager<ParameterValue>, IParameterValueBuildingBlockExtendManager
   {
      
   }
}