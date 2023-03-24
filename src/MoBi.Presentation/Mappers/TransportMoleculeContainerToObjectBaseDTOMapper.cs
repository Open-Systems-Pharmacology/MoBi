using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface ITransportMoleculeContainerToObjectBaseDTOMapper : IMapper<TransporterMoleculeContainer, ObjectBaseDTO>
   {
   }

   internal class TransportMoleculeContainerToObjectBaseDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ITransportMoleculeContainerToObjectBaseDTOMapper
   {
      public ObjectBaseDTO MapFrom(TransporterMoleculeContainer transporterMoleculeContainer)
      {
         var dto = Map(new ObjectBaseDTO(transporterMoleculeContainer));
         dto.Name = transporterMoleculeContainer.TransportName;
         return dto;
      }
   }
}