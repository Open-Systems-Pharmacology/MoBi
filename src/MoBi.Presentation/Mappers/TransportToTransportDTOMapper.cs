using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   internal interface ITransportToTransportDTOMapper : IMapper<ITransport, TransportDTO>
   {
   }

   internal class TransportToTransportDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ITransportToTransportDTOMapper
   {
      private readonly IObjectPathFactory _pathFactory;

      public TransportToTransportDTOMapper(IObjectPathFactory pathFactory)
      {
         _pathFactory = pathFactory;
      }

      public TransportDTO MapFrom(ITransport transport)
      {
         var dto = Map(new TransportDTO(transport));
         dto.Molecule = transport.SourceAmount.Name;
         dto.Source = _pathFactory.CreateAbsoluteObjectPath(transport.SourceAmount.ParentContainer).PathAsString;
         dto.Target = _pathFactory.CreateAbsoluteObjectPath(transport.TargetAmount.ParentContainer).PathAsString;
         dto.Rate = transport.Formula.ToString();
         dto.Dimension = transport.Dimension;
         return dto;
      }
   }
}