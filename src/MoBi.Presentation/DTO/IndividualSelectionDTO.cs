using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class IndividualSelectionDTO
   {
      public IndividualSelectionDTO(IReadOnlyList<IndividualBuildingBlock> allIndividuals)
      {
         _allIndividuals.AddRange(allIndividuals);
         SelectedIndividualBuildingBlock = NullIndividual.NullIndividualBuildingBlock;
      }

      private readonly List<IndividualBuildingBlock> _allIndividuals = new List<IndividualBuildingBlock> { NullIndividual.NullIndividualBuildingBlock };
      private IndividualBuildingBlock _selectedIndividualBuildingBlock;

      public IndividualBuildingBlock SelectedIndividualBuildingBlock
      {
         set => _selectedIndividualBuildingBlock = value ?? NullIndividual.NullIndividualBuildingBlock;
         get => _selectedIndividualBuildingBlock;
      }

      public IReadOnlyList<IndividualBuildingBlock> AllIndividuals => _allIndividuals;

      public bool IsNull()
      {
         return SelectedIndividualBuildingBlock.Equals(NullIndividual.NullIndividualBuildingBlock);
      }
   }
}