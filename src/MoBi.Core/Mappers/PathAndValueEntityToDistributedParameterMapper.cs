using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IPathAndValueEntityToDistributedParameterMapper
   {
      IDistributedParameter MapFrom<TBuilder>(TBuilder pathAndValueEntity, DistributionType distributionType, IReadOnlyList<TBuilder> subParameters) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit;
   }

   public class PathAndValueEntityToDistributedParameterMapper : IPathAndValueEntityToDistributedParameterMapper
   {
      private readonly IParameterFactory _parameterFactory;

      public PathAndValueEntityToDistributedParameterMapper(IParameterFactory parameterFactory)
      {
         _parameterFactory = parameterFactory;
      }
      public IDistributedParameter MapFrom<TBuilder>(TBuilder pathAndValueEntity, DistributionType distributionType, IReadOnlyList<TBuilder> subParameters) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
      {
         var distributedParameter =  createParameter(pathAndValueEntity, distributionType);
         addSubParameters(subParameters, distributedParameter);
         if (pathAndValueEntity.Value.HasValue)
            distributedParameter.Value = pathAndValueEntity.Value.Value;
         return distributedParameter;
      }

      private void addSubParameters<TBuilder>(IReadOnlyList<TBuilder> subParameters, IDistributedParameter distributedParameter) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
      {
         subParameters.Each(subParameter =>
         {
            distributedParameter.Add(_parameterFactory.CreateParameter(subParameter.Name, subParameter.Value, subParameter.Dimension, formula: subParameter.Formula, displayUnit: subParameter.DisplayUnit));
         });
      }

      private IDistributedParameter createParameter<TBuilder>(TBuilder pathAndValueEntity, DistributionType distributionType) where TBuilder : PathAndValueEntity, IUsingFormula, IWithDisplayUnit
      {
         return _parameterFactory.CreateDistributedParameter(pathAndValueEntity.Name, distributionType, dimension: pathAndValueEntity.Dimension, displayUnit: pathAndValueEntity.DisplayUnit) as IDistributedParameter;
      }
   }
}
