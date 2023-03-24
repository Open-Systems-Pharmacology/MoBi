using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper : IMapper<IApplicationMoleculeBuilder, ApplicationMoleculeBuilderDTO>
   {
   }

   internal class ApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaBuilderToDTOFormulaMapper;

      public ApplicationMoleculeBuilderToApplicationMoleculeBuilderDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaBuilderToDTOFormulaMapper)
      {
         _formulaBuilderToDTOFormulaMapper = formulaBuilderToDTOFormulaMapper;
      }

      public ApplicationMoleculeBuilderDTO MapFrom(IApplicationMoleculeBuilder applicationMoleculeBuilder)
      {
         var dto = Map(new ApplicationMoleculeBuilderDTO(applicationMoleculeBuilder));
         dto.RelativeContainerPath = applicationMoleculeBuilder.RelativeContainerPath == null ? string.Empty : applicationMoleculeBuilder.RelativeContainerPath.PathAsString;
         dto.Formula = _formulaBuilderToDTOFormulaMapper.MapFrom(applicationMoleculeBuilder.Formula);
         return dto;
      }
   }
}