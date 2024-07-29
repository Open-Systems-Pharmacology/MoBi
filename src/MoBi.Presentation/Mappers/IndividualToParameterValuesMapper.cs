using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IIndividualToParameterValuesMapper : IMapper<IndividualBuildingBlock, ParameterValuesBuildingBlock>
   {
   }

   public class IndividualToParameterValuesMapper : IIndividualToParameterValuesMapper
   {
      private readonly IPathAndValueEntityToParameterValueMapper _individualParameterToParameterValueMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public IndividualToParameterValuesMapper(IPathAndValueEntityToParameterValueMapper individualParameterToParameterValueMapper, IObjectBaseFactory objectBaseFactory)
      {
         _individualParameterToParameterValueMapper = individualParameterToParameterValueMapper;
         _objectBaseFactory = objectBaseFactory;
      }

      public ParameterValuesBuildingBlock MapFrom(IndividualBuildingBlock individualBuildingBlock)
      {
         var buildingBlock = _objectBaseFactory.Create<ParameterValuesBuildingBlock>();
         buildingBlock.Name = individualBuildingBlock.Name;
         individualBuildingBlock.MapAllUsing(_individualParameterToParameterValueMapper).Each(x => buildingBlock.Add(x));
         buildingBlock.FormulaCache.AddRange(buildingBlock.UniqueFormulasByName());

         return buildingBlock;
      }
   }
}