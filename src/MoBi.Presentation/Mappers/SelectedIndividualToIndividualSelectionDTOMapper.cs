using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation.Mappers
{
   public interface ISelectedIndividualToIndividualSelectionDTOMapper : IMapper<IndividualBuildingBlock, IndividualSelectionDTO>
   {
   }

   public class SelectedIndividualToIndividualSelectionDTOMapper : ISelectedIndividualToIndividualSelectionDTOMapper
   {
      private readonly IBuildingBlockRepository _buildingBlockRepository;

      public SelectedIndividualToIndividualSelectionDTOMapper(IBuildingBlockRepository buildingBlockRepository)
      {
         _buildingBlockRepository = buildingBlockRepository;
      }

      public IndividualSelectionDTO MapFrom(IndividualBuildingBlock individualBuildingBlock)
      {
         var individualSelectionDTO = new IndividualSelectionDTO(_buildingBlockRepository.IndividualsCollection);
         if (individualBuildingBlock != null)
            individualSelectionDTO.SelectedIndividualBuildingBlock = _buildingBlockRepository.IndividualByName(individualBuildingBlock.Name);

         return individualSelectionDTO;
      }
   }
}