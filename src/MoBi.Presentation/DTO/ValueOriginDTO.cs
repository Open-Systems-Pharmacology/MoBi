using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;

namespace MoBi.Presentation.DTO
{
   //TODO MOVE TO CORE
   public class ValueOriginDTO : IWithValueOrigin
   {
      public string Caption { get; set; } = Captions.ValueOrigin.FormatForLabel(addColon: false);

      private readonly IWithValueOrigin _withValueOrigin;

      public ValueOriginDTO(IWithValueOrigin withValueOrigin)
      {
         _withValueOrigin = withValueOrigin;
      }

      public ValueOrigin ValueOrigin => _withValueOrigin.ValueOrigin;
   }
}