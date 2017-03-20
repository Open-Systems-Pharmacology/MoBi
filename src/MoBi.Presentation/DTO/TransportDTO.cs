using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class TransportDTO : ObjectBaseDTO
   {
      public string Molecule { get; set; }
      public string Source { get; set; }
      public string Target { get; set; }
      public string Rate { get; set; }
      public IDimension Dimension { get; set; }
   }
}