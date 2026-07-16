using System.Collections.Generic;
using System.ComponentModel;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation.DTO;

public class EditConditionGroupDTO
{
   //the number of empty Match rows the dialog opens with so the user always has a usable starting state.
   private const int DEFAULT_OPERAND_COUNT = 2;

   public EditConditionGroupDTO(IReadOnlyList<string> availableTags)
   {
      AvailableTags = availableTags ?? new List<string>();
      for (var i = 0; i < DEFAULT_OPERAND_COUNT; i++)
         Operands.Add(new EditConditionGroupOperandDTO());
   }

   public CriteriaOperator Operator { get; set; } = CriteriaOperator.And;

   public BindingList<EditConditionGroupOperandDTO> Operands { get; } = new();

   public IReadOnlyList<string> AvailableTags { get; }
}