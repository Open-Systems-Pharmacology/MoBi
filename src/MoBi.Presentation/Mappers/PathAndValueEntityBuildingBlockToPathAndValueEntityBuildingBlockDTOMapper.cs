using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Extensions;

namespace MoBi.Presentation.Mappers
{
   public abstract class PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper<TBuildingBlock,TBuilder, TBuildingBlockDTO, TBuilderDTO> : 
      IMapper<TBuildingBlock, TBuildingBlockDTO> 
      where TBuilder : PathAndValueEntity 
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder>
      where TBuilderDTO : PathAndValueEntityDTO<TBuilder, TBuilderDTO>
   {
      private readonly IPathAndValueEntityToDistributedParameterMapper _pathAndValueEntityToDistributedParameterMapper;

      protected PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper(IPathAndValueEntityToDistributedParameterMapper pathAndValueEntityToDistributedParameterMapper)
      { 
         _pathAndValueEntityToDistributedParameterMapper = pathAndValueEntityToDistributedParameterMapper;
      }

      public TBuildingBlockDTO MapFrom(TBuildingBlock buildingBlock)
      {
         var parameterDTOs = buildingBlock.Select(x => BuilderDTOFor(x, buildingBlock)).ToList();

         // We need ToList because we are modifying the list while iterating over it
         parameterDTOs.Where(x => x.IsDistributed).ToList().Each(x => addDistributionParameters(x, parameterDTOs));
         return MapBuildingBlockDTO(buildingBlock, parameterDTOs);
      }

      protected abstract TBuilderDTO BuilderDTOFor(TBuilder pathAndValueEntity, TBuildingBlock buildingBlock);

      private void addDistributionParameters(TBuilderDTO distributedParameter, List<TBuilderDTO> parameterDTOs)
      {
         // We need ToList because we are modifying the list while iterating over it
         subParametersFor(distributedParameter, parameterDTOs).ToList().Each(x =>
         {
            parameterDTOs.Remove(x);
            distributedParameter.AddSubParameter(x);
         });

         var pathAndValueEntity = builderFrom(distributedParameter);
         var temporaryParameter = _pathAndValueEntityToDistributedParameterMapper.MapFrom(pathAndValueEntity, distributionType(pathAndValueEntity), distributedParameter.SubParameters.Select(x => x.PathWithValueObject).ToList());
         distributedParameter.DistributionValue = temporaryParameter.ConvertToDisplayUnit(temporaryParameter.Value);
      }

      private static DistributionType distributionType(TBuilder pathAndValueEntity)
      {
         return pathAndValueEntity.DistributionType.Value;
      }

      private static TBuilder builderFrom(TBuilderDTO distributedParameter)
      {
         return distributedParameter.PathWithValueObject;
      }

      protected abstract TBuildingBlockDTO MapBuildingBlockDTO(TBuildingBlock buildingBlock, List<TBuilderDTO> parameterDTOs);

      private IEnumerable<TBuilderDTO> subParametersFor(TBuilderDTO distributedParameter, IEnumerable<TBuilderDTO> parameterList) => 
         parameterList.Where(x => isDirectSubParameterOf(distributedParameter, x));

      private bool isDirectSubParameterOf(TBuilderDTO distributedParameter, TBuilderDTO builderDTO) => 
         builderDTO.PathWithValueObject.IsDirectSubParameterOf(distributedParameter.PathWithValueObject);
   }
}