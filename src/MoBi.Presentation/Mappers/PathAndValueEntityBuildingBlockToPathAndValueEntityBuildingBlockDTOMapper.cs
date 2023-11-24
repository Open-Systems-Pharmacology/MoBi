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
         var parameterDTOs = buildingBlock.MapAllUsing(_builderToBuilderDTOMapper).ToList();

         // We need ToList because we are modifying the list while iterating over it
         parameterDTOs.Where(x => x.IsDistributed).ToList().Each(x => addDistributionParameters(x, parameterDTOs));
         return MapBuildingBlockDTO(buildingBlock, parameterDTOs);
      }

      private void addDistributionParameters(TBuilderDTO distributedParameter, List<TBuilderDTO> parameterDTOs)
      {
         // We need ToList because we are modifying the list while iterating over it
         subParametersFor(distributedParameter, parameterDTOs).ToList().Each(x =>
         {
            parameterDTOs.Remove(x);
            distributedParameter.AddSubParameter(x);
         });
      }

      protected abstract TBuildingBlockDTO MapBuildingBlockDTO(TBuildingBlock buildingBlock, List<TBuilderDTO> parameterDTOs);

      private IEnumerable<TBuilderDTO> subParametersFor(TBuilderDTO distributedParameter, IEnumerable<TBuilderDTO> parameterList)
      {
         return parameterList.Where(x => isDirectSubParameterOf(distributedParameter, x));
      }

      private bool isDirectSubParameterOf(TBuilderDTO distributedParameter, TBuilderDTO builderDTO)
      {
         return builderDTO.Path.StartsWith(distributedParameter.Path) && builderDTO.Path.Count - distributedParameter.Path.Count == 1;
      }
   }
}