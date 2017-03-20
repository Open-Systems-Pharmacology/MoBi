using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface ITransportBuilderToDTOTransportBuilderMapper : IMapper<ITransportBuilder, TransportBuilderDTO>
   {
   }

   public class TransportBuilderToDTOTransportBuilderMapper : ObjectBaseToObjectBaseDTOMapperBase, ITransportBuilderToDTOTransportBuilderMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToDTOFormulaMapper;
      private readonly IParameterToParameterDTOMapper _parameterBuilderToDTOParameterBuilderMapper;

      public TransportBuilderToDTOTransportBuilderMapper(IFormulaToFormulaBuilderDTOMapper formulaToDTOFormulaMapper, IParameterToParameterDTOMapper parameterBuilderToDTOParameterBuilderMapper)
      {
         _formulaToDTOFormulaMapper = formulaToDTOFormulaMapper;
         _parameterBuilderToDTOParameterBuilderMapper = parameterBuilderToDTOParameterBuilderMapper;
      }

      private T mapTransport<T>(ITransportBuilder transportBuilder, T dto) where T : TransportBuilderDTO
      {
         MapProperties(transportBuilder, dto);
         dto.Formula = _formulaToDTOFormulaMapper.MapFrom(transportBuilder.Formula);
         dto.Parameters = transportBuilder.Parameters.MapAllUsing(_parameterBuilderToDTOParameterBuilderMapper).Cast<ParameterDTO>();
         dto.TransportType = transportBuilder.TransportType;
         return dto;
      }

      public TransportBuilderDTO MapFrom(ITransportBuilder transportBuilder)
      {
         return mapTransport(transportBuilder, new TransportBuilderDTO(transportBuilder));
      }
   }
}