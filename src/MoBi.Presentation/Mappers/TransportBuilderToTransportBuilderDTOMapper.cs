using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface ITransportBuilderToTransportBuilderDTOMapper : IMapper<ITransportBuilder, TransportBuilderDTO>
   {
   }

   public class TransportBuilderToTransportBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ITransportBuilderToTransportBuilderDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaDTOMapper;
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;

      public TransportBuilderToTransportBuilderDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaDTOMapper, IParameterToParameterDTOMapper parameterDTOMapper)
      {
         _formulaDTOMapper = formulaDTOMapper;
         _parameterDTOMapper = parameterDTOMapper;
      }

      private T mapTransport<T>(ITransportBuilder transportBuilder, T dto) where T : TransportBuilderDTO
      {
         MapProperties(transportBuilder, dto);
         dto.Formula = _formulaDTOMapper.MapFrom(transportBuilder.Formula);
         dto.Parameters = transportBuilder.Parameters.MapAllUsing(_parameterDTOMapper).Cast<ParameterDTO>();
         dto.TransportType = transportBuilder.TransportType;
         return dto;
      }

      public TransportBuilderDTO MapFrom(ITransportBuilder transportBuilder)
      {
         return mapTransport(transportBuilder, new TransportBuilderDTO(transportBuilder));
      }
   }
}