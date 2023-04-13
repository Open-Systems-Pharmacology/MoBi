using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class IndividualSelectionDTO
   {
      public IndividualSelectionDTO(IReadOnlyList<IndividualBuildingBlock> allIndividuals)
      {
         _allIndividuals.AddRange(allIndividuals);
      }
      
      public static IndividualBuildingBlock NullIndividualBuildingBlock = new IndividualBuildingBlock().WithName(AppConstants.Captions.IndividualNotSelected);
      private readonly List<IndividualBuildingBlock> _allIndividuals = new List<IndividualBuildingBlock> {NullIndividualBuildingBlock};
      public IndividualBuildingBlock SelectedIndividualBuildingBlock { set; get; } = NullIndividualBuildingBlock;

      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals => _allIndividuals;

      public bool IsNull()
      {
         return SelectedIndividualBuildingBlock.Equals(NullIndividualBuildingBlock);
      }
   }
}