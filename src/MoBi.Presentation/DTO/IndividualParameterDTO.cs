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
      private readonly ValueOriginDTO _valueOrigin;

      public IEnumerable<OriginDataItemDTO> AllDataItems => _originDataItems;

      public OriginDataDTO(OriginDataItems originData)
      {
         _originData = originData;
         _originDataItems = new List<OriginDataItemDTO>();
         _originData.AllDataItems.Each(x => _originDataItems.Add(new OriginDataItemDTO(x)));
         // _valueOrigin = new ValueOriginDTO(originData);

      }
   }

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

   public class IndividualParameterDTO : PathWithValueEntityDTO<IndividualParameter>, IWithFormulaDTO
   {
      public IndividualParameterDTO(IndividualParameter individualParameter) : base(individualParameter)
      {

      }
   }
}