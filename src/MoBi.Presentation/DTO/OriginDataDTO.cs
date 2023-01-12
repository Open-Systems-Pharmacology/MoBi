using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.DTO
{
   public class OriginDataDTO
   {
      private readonly OriginDataItems _originData;
      private readonly List<OriginDataItemDTO> _originDataItems;

      public IEnumerable<OriginDataItemDTO> AllDataItems => _originDataItems;
      public ValueOriginDTO ValueOrigin { get; }

      public OriginDataDTO(OriginDataItems originData)
      {
         _originData = originData;
         _originDataItems = new List<OriginDataItemDTO>();
         _originData.AllDataItems.Each(x => _originDataItems.Add(new OriginDataItemDTO(x)));
         ValueOrigin = new ValueOriginDTO(originData);

      }
   }
}