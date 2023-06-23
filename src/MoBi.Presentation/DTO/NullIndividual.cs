using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public static class NullIndividual
   {
      public static IndividualBuildingBlock NullIndividualBuildingBlock = new IndividualBuildingBlock().WithName(AppConstants.Captions.IndividualNotSelected);
   }
}