using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface ISelectedIndividualToIndividualSelectionDTOMapper : IMapper<IndividualBuildingBlock, IndividualSelectionDTO>
   {
   }

   public class SelectedIndividualToIndividualSelectionDTOMapper : ISelectedIndividualToIndividualSelectionDTOMapper
   {
      private readonly IMoBiProjectRetriever _projectRetriever;

      public SelectedIndividualToIndividualSelectionDTOMapper(IMoBiProjectRetriever projectRetriever)
      {
         _projectRetriever = projectRetriever;
      }

      public IndividualSelectionDTO MapFrom(IndividualBuildingBlock individualBuildingBlock)
      {
         var individualSelectionDTO = new IndividualSelectionDTO(_projectRetriever.Current.IndividualsCollection);
         if (individualBuildingBlock != null)
            individualSelectionDTO.SelectedIndividualBuildingBlock = _projectRetriever.Current.IndividualByName(individualBuildingBlock.Name);

         return individualSelectionDTO;
      }
   }
}