using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Presentation.Mappers
{

   public interface IStartValueToStartValueDTOMapper<TStartValue, out TStartValueDTO> where TStartValue : class, IStartValue
   {
      TStartValueDTO MapFrom(TStartValue startValue, IStartValuesBuildingBlock<TStartValue> buildingBlock);
   }

   public interface IMoleculeStartValueToMoleculeStartValueDTOMapper : IStartValueToStartValueDTOMapper<IMoleculeStartValue, MoleculeStartValueDTO>
   {
      
   }

   public class MoleculeStartValueToMoleculeStartValueDTOMapper : IMoleculeStartValueToMoleculeStartValueDTOMapper
   {
      public MoleculeStartValueDTO MapFrom(IMoleculeStartValue moleculeStartValue, IStartValuesBuildingBlock<IMoleculeStartValue> buildingBlock)
      {
         var dto = new MoleculeStartValueDTO(moleculeStartValue, buildingBlock)
         {
            ContainerPath = moleculeStartValue.ContainerPath,
         };

         var formula = moleculeStartValue.Formula as ExplicitFormula;
         dto.Formula = formula != null ? new StartValueFormulaDTO(formula) : new EmptyFormulaDTO();
         return dto;
      }
   }
}