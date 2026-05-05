using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation.DTO;

public class ConditionGroupDTO : DescriptorConditionDTO
{
   public ConditionGroup ConditionGroup { get; }
   public IReadOnlyList<DescriptorConditionDTO> ConditionDTOs { get; }

   public ConditionGroupDTO(ConditionGroup conditionGroup, IReadOnlyList<DescriptorConditionDTO> conditionDTOs)
      : base(conditionGroup?.Condition ?? string.Empty, TagType.ConditionGroup, AppConstants.ConditionGroup)
   {
      ConditionGroup = conditionGroup;
      ConditionDTOs = conditionDTOs ?? new List<DescriptorConditionDTO>();
   }

   public CriteriaOperator InnerOperator => ConditionGroup?.Operator ?? CriteriaOperator.And;
}