using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper : IMapper<TransporterMoleculeContainer, TransporterMoleculeContainerDTO>
   {
   }

   internal class TransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper
   {
      private readonly ITransportBuilderToTransportBuilderDTOMapper _activeTransportToDTOActiveTransportBuilderMapper;
      private readonly IParameterToParameterDTOMapper _parameterBuilderToDTOParameterBuilderMapper;

      public TransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper(ITransportBuilderToTransportBuilderDTOMapper activeTransportToDTOActiveTransportBuilderMapper, IParameterToParameterDTOMapper parameterBuilderTodtoParameterBuilderMapper)
      {
         _activeTransportToDTOActiveTransportBuilderMapper = activeTransportToDTOActiveTransportBuilderMapper;
         _parameterBuilderToDTOParameterBuilderMapper = parameterBuilderTodtoParameterBuilderMapper;
      }

      public TransporterMoleculeContainerDTO MapFrom(TransporterMoleculeContainer transporterMoleculeContainer)
      {
         var dto = Map<TransporterMoleculeContainerDTO>(transporterMoleculeContainer);
         dto.TransportName = transporterMoleculeContainer.TransportName;
         dto.Realizations = transporterMoleculeContainer.ActiveTransportRealizations.MapAllUsing(_activeTransportToDTOActiveTransportBuilderMapper);
         dto.Parameters = transporterMoleculeContainer.Parameters.MapAllUsing(_parameterBuilderToDTOParameterBuilderMapper).Cast<ParameterDTO>();
         return dto;
      }
   }
}