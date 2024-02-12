using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IPathAndValueEntityToParameterValueMapper : IMapper<PathAndValueEntity, ParameterValue>
   {

   }


   public class PathAndValueEntityToParameterValueMapper : IPathAndValueEntityToParameterValueMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManager;

      public PathAndValueEntityToParameterValueMapper(IObjectBaseFactory objectBaseFactory, ICloneManagerForModel cloneManager)
      {
         _objectBaseFactory = objectBaseFactory;
         _cloneManager = cloneManager;
      }

      public ParameterValue MapFrom(PathAndValueEntity pathAndValueEntity)
      {
         var parameterValue = _objectBaseFactory.Create<ParameterValue>();

         parameterValue.UpdatePropertiesFrom(pathAndValueEntity, _cloneManager);

         return parameterValue;
      }
   }
}