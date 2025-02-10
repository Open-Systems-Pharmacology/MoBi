using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.DTO
{
   public abstract class ContainsMultiSelectDTO<TSelectableDTO, TParentDTO> : DxValidatableDTO where TSelectableDTO : SelectableDTO<TSelectableDTO, TParentDTO> where TParentDTO : ContainsMultiSelectDTO<TSelectableDTO, TParentDTO>
   {
      protected readonly List<TSelectableDTO> _selectedDTOs = new List<TSelectableDTO>();
      protected readonly List<TSelectableDTO> _selectableDTOs = new List<TSelectableDTO>();

      public void SelectionUpdated()
      {
         _selectableDTOs.Each(updateSelectionFor);
      }

      private void updateSelectionFor(TSelectableDTO x)
      {
         if (x.Selected)
            AddSelectedDTO(x);
         else
            removeSelectedDTO(x);
      }

      protected void AddSelectedDTO(TSelectableDTO selectableDTO)
      {
         if (_selectedDTOs.Contains(selectableDTO))
            return;

         _selectedDTOs.Add(selectableDTO);
      }

      protected void AddSelectableDTO(TSelectableDTO selectableDTO)
      {
         _selectableDTOs.Add(selectableDTO);

         if (selectableDTO.Selected)
            AddSelectedDTO(selectableDTO);
      }

      private void removeSelectedDTO(TSelectableDTO selectableDTO) => _selectedDTOs.Remove(selectableDTO);
   }
}