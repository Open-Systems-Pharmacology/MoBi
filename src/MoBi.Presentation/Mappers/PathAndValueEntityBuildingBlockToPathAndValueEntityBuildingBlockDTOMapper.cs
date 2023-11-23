using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public abstract class PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper<TBuildingBlock,TBuilder, TBuildingBlockDTO, TBuilderDTO> : 
      IMapper<TBuildingBlock, TBuildingBlockDTO> 
      where TBuilder : PathAndValueEntity 
      where TBuildingBlock : PathAndValueEntityBuildingBlock<TBuilder>
      where TBuilderDTO : PathAndValueEntityDTO<TBuilder, TBuilderDTO>
   {
      private readonly IMapper<TBuilder, TBuilderDTO> _builderToBuilderDTOMapper;

      protected PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper(IMapper<TBuilder, TBuilderDTO> builderToBuilderDTOMapper)
      {
         _builderToBuilderDTOMapper = builderToBuilderDTOMapper;
      }

      public TBuildingBlockDTO MapFrom(TBuildingBlock buildingBlock)
      {
         var distributedParameters = buildingBlock.Where(p => p.DistributionType != null).ToList();
         var nonDistributedParameters = new PathAndValueEntityCache<TBuilder>();
         nonDistributedParameters.AddRange(buildingBlock.Where(p => p.DistributionType == null));

         var parameterDTOs = distributedParameters.Select(x => mapDistributedParameters(x, nonDistributedParameters)).ToList();
         parameterDTOs.AddRange(nonDistributedParameters.MapAllUsing(_builderToBuilderDTOMapper));

         return MapBuildingBlockDTO(buildingBlock, parameterDTOs);
      }

      protected abstract TBuildingBlockDTO MapBuildingBlockDTO(TBuildingBlock buildingBlock, List<TBuilderDTO> parameterDTOs);

      private TBuilderDTO mapDistributedParameters(TBuilder distributedParameter, PathAndValueEntityCache<TBuilder> nonDistributedParameters)
      {
         var dto = _builderToBuilderDTOMapper.MapFrom(distributedParameter);
         var subParameters = subParametersFor(distributedParameter, nonDistributedParameters).ToList();
         subParameters.MapAllUsing(_builderToBuilderDTOMapper).Each(x =>
         {
            // although the sub-parameter is not distributed, it's already been mapped once as a sub-parameter of a distributed parameter
            nonDistributedParameters.Remove(x.PathWithValueObject.Path);
            dto.AddSubParameter(x);
         });

         return dto;
      }

      private IEnumerable<TBuilder> subParametersFor(TBuilder distributedParameter, IEnumerable<TBuilder> nonDistributedParameters)
      {
         return nonDistributedParameters.Where(x => isDirectSubParameterOf(distributedParameter, x));
      }

      private bool isDirectSubParameterOf(TBuilder distributedParameter, TBuilder individualParameter)
      {
         return individualParameter.Path.StartsWith(distributedParameter.Path) && individualParameter.Path.Count - distributedParameter.Path.Count == 1;
      }
   }
}