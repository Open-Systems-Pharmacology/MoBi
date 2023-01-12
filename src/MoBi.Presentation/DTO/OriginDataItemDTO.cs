using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class OriginDataItemDTO
   {
      private readonly OriginDataItem _originDataItem;

      public OriginDataItemDTO(OriginDataItem originDataItem)
      {
         _originDataItem = originDataItem;
      }

      public string Name => _originDataItem.Name;
      public string Value => _originDataItem.Value;
   }
}