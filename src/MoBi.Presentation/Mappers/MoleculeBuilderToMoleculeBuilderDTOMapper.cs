using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderToMoleculeBuilderDTOMapper : IMapper<IMoleculeBuilder, MoleculeBuilderDTO>
   {
   }

   public class MoleculeBuilderToMoleculeBuilderDTOMapper : ObjectBaseToObjectBaseDTOMapperBase,
      IMoleculeBuilderToMoleculeBuilderDTOMapper
   {
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaBuilderMapper;
      private readonly ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper _transporterMoleculeContainerMapper;
      private readonly IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper _usedCalculationMethodMapper;
      private readonly IInteractionContainerToInteractionConatainerDTOMapper _interactionContainerMapper;

      public MoleculeBuilderToMoleculeBuilderDTOMapper(IFormulaToFormulaBuilderDTOMapper formulaBuilderMapper, ITransporterMoleculeContainerToTranpsorterMoleculeContainerDTOMapper transporterMoleculeContainerMapper,
         IUsedCalcualtionMethodToUsedCalcualtionMethodDTOMapper usedCalculationMethodMapper, IInteractionContainerToInteractionConatainerDTOMapper interactionContainerMapper)
      {
         _formulaBuilderMapper = formulaBuilderMapper;
         _usedCalculationMethodMapper = usedCalculationMethodMapper;
         _interactionContainerMapper = interactionContainerMapper;
         _transporterMoleculeContainerMapper = transporterMoleculeContainerMapper;
      }

      public MoleculeBuilderDTO MapFrom(IMoleculeBuilder moleculeBuilder)
      {
         var dto = Map<MoleculeBuilderDTO>(moleculeBuilder);
         dto.TransporterMolecules = allTransporterMoleculesDTOFrom(moleculeBuilder);
         dto.InteractionContainerCollection = allInteractionContainersDTOFrom(moleculeBuilder);
         dto.DefaultStartFormula = _formulaBuilderMapper.MapFrom(moleculeBuilder.DefaultStartFormula);
         dto.Stationary = !moleculeBuilder.IsFloating;
         dto.UsedCalculationMethods = moleculeBuilder.UsedCalculationMethods.MapAllUsing(_usedCalculationMethodMapper);
         dto.MoleculeType = moleculeBuilder.QuantityType;
         return dto;
      }

      private IReadOnlyList<InteractionContainerDTO> allInteractionContainersDTOFrom(IMoleculeBuilder moleculeBuilder)
      {
         return moleculeBuilder.InteractionContainerCollection.MapAllUsing(_interactionContainerMapper);
      }

      private IReadOnlyList<TransporterMoleculeContainerDTO> allTransporterMoleculesDTOFrom(IMoleculeBuilder moleculeBuilder)
      {
         return moleculeBuilder.TransporterMoleculeContainerCollection.MapAllUsing(_transporterMoleculeContainerMapper);
      }
   }
}