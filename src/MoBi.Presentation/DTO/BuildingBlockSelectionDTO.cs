using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class BuildingBlockSelectionDTO : ValidatableDTO
   {
      public BuildingBlockSelectionDTO()
      {
         Rules.AddRange(AllRules.All());
      }

      private IBuildingBlock _buildingBlock;

      public IBuildingBlock BuildingBlock
      {
         get { return _buildingBlock; }
         set
         {
            _buildingBlock = value;
            OnPropertyChanged(() => BuildingBlock);
         }
      }

      private static class AllRules
      {
         private static IBusinessRule buildingBlockNotNull
         {
            get
            {
               return CreateRule.For<BuildingBlockSelectionDTO>()
                   .Property(item => item.BuildingBlock)
                   .WithRule((dto, block) => block != null)
                   .WithError((dto, block) => AppConstants.Exceptions.NoBuildingBlockAvailable);
            }
         }

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return buildingBlockNotNull;
         }
      }
   }
}