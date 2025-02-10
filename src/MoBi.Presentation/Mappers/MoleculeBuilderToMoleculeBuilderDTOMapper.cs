using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderToMoleculeBuilderDTOMapper : IMapper<MoleculeBuilder, MoleculeBuilderDTO>
   {
   }

   public class MoleculeBuilderToMoleculeBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase,
      IMoleculeBuilderToMoleculeBuilderDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaBuilderMapper;
      private readonly ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper _transporterMoleculeContainerMapper;
      private readonly IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper _usedCalculationMethodMapper;
      private readonly IInteractionContainerToInteractionContainerDTOMapper _interactionContainerMapper;

      public MoleculeBuilderToMoleculeBuilderDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaBuilderMapper, ITransporterMoleculeContainerToTransporterMoleculeContainerDTOMapper transporterMoleculeContainerMapper,
         IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper usedCalculationMethodMapper, IInteractionContainerToInteractionContainerDTOMapper interactionContainerMapper)
      {
         _formulaBuilderMapper = formulaBuilderMapper;
         _usedCalculationMethodMapper = usedCalculationMethodMapper;
         _interactionContainerMapper = interactionContainerMapper;
         _transporterMoleculeContainerMapper = transporterMoleculeContainerMapper;
      }

      public MoleculeBuilderDTO MapFrom(MoleculeBuilder moleculeBuilder)
      {
         var dto = Map(new MoleculeBuilderDTO(moleculeBuilder));
         dto.TransporterMolecules = allTransporterMoleculesDTOFrom(moleculeBuilder);
         dto.InteractionContainerCollection = allInteractionContainersDTOFrom(moleculeBuilder);
         dto.DefaultStartFormula = _formulaBuilderMapper.MapFrom(moleculeBuilder.DefaultStartFormula);
         dto.Stationary = !moleculeBuilder.IsFloating;
         dto.UsedCalculationMethods = moleculeBuilder.UsedCalculationMethods.MapAllUsing(_usedCalculationMethodMapper);
         dto.MoleculeType = moleculeBuilder.QuantityType;
         return dto;
      }

      private IReadOnlyList<InteractionContainerDTO> allInteractionContainersDTOFrom(MoleculeBuilder moleculeBuilder)
      {
         return moleculeBuilder.InteractionContainerCollection.MapAllUsing(_interactionContainerMapper);
      }

      private IReadOnlyList<TransporterMoleculeContainerDTO> allTransporterMoleculesDTOFrom(MoleculeBuilder moleculeBuilder)
      {
         return moleculeBuilder.TransporterMoleculeContainerCollection.MapAllUsing(_transporterMoleculeContainerMapper);
      }
   }
}