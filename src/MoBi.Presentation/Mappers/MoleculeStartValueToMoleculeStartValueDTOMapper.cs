using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{

   public interface IStartValueToStartValueDTOMapper<TStartValue, out TStartValueDTO> where TStartValue : class, IStartValue
   {
      TStartValueDTO MapFrom(TStartValue startValue, IStartValuesBuildingBlock<TStartValue> buildingBlock);
   }

   public interface IMoleculeStartValueToMoleculeStartValueDTOMapper : IStartValueToStartValueDTOMapper<InitialCondition, MoleculeStartValueDTO>
   {
      
   }

   public class MoleculeStartValueToMoleculeStartValueDTOMapper : IMoleculeStartValueToMoleculeStartValueDTOMapper
   {
      private readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      public MoleculeStartValueToMoleculeStartValueDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }
      public MoleculeStartValueDTO MapFrom(InitialCondition moleculeStartValue, IStartValuesBuildingBlock<InitialCondition> buildingBlock)
      {
         var dto = new MoleculeStartValueDTO(moleculeStartValue, buildingBlock)
         {
            ContainerPath = moleculeStartValue.ContainerPath,
            Formula = _formulaMapper.MapFrom(moleculeStartValue.Formula)
         };
         return dto;
      }
   }
}