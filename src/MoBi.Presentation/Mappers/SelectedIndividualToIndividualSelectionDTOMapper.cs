using MoBi.Core.Domain.Model;
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
      private readonly IMoBiContext _moBiContext;

      public SelectedIndividualToIndividualSelectionDTOMapper(IMoBiContext moBiContext)
      {
         _moBiContext = moBiContext;
      }

      public IndividualSelectionDTO MapFrom(IndividualBuildingBlock individualBuildingBlock)
      {
         var individualSelectionDTO = new IndividualSelectionDTO(_moBiContext.CurrentProject.IndividualsCollection);
         if (individualBuildingBlock != null)
            individualSelectionDTO.SelectedIndividualBuildingBlock = _moBiContext.CurrentProject.IndividualsCollection.FindByName(individualBuildingBlock.Name);

         return individualSelectionDTO;
      }
   }
}