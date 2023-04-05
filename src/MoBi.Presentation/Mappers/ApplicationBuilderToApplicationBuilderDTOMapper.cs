using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IApplicationBuilderToApplicationBuilderDTOMapper : IMapper<IApplicationBuilder, ApplicationBuilderDTO>
   {
   }

   internal class ApplicationBuilderToApplicationBuilderDTOMapper : EventGroupBuilderToEventGroupBuilderDTOMapper, IApplicationBuilderToApplicationBuilderDTOMapper
   {
      private readonly ITransportBuilderToTransportBuilderDTOMapper _transportBuilderDTOMapper;
      private readonly IApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper _applicationMoleculeBuilderDTOMapper;

      public ApplicationBuilderToApplicationBuilderDTOMapper(IParameterToParameterDTOMapper parameterDTOMapper, IEventBuilderToEventBuilderDTOMapper eventBuilderDTOMapper, ITransportBuilderToTransportBuilderDTOMapper transportBuilderDTOMapper, IApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper applicationMoleculeBuilderDTOMapper, IContainerToContainerDTOMapper containerDTOMapper)
         : base(parameterDTOMapper, eventBuilderDTOMapper, containerDTOMapper)
      {
         _transportBuilderDTOMapper = transportBuilderDTOMapper;
         _applicationMoleculeBuilderDTOMapper = applicationMoleculeBuilderDTOMapper;
      }

      public ApplicationBuilderDTO MapFrom(IApplicationBuilder applicationBuilder)
      {
         _applicationBuilderToDTOApplicationBuilderMapper = this;
         var dto = MapEventGroupProperties(applicationBuilder, new ApplicationBuilderDTO(applicationBuilder));
         dto.MoleculeName = applicationBuilder.MoleculeName;
         dto.Transports = applicationBuilder.Transports.MapAllUsing(_transportBuilderDTOMapper);
         dto.Molecules = applicationBuilder.Molecules.MapAllUsing(_applicationMoleculeBuilderDTOMapper);
         return dto;
      }
   }
}