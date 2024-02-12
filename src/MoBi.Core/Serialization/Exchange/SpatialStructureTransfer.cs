using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Serialization.Exchange
{
   public class SpatialStructureTransfer
   {
      public string Id { get; set; } = ShortGuid.NewGuid();
      public MoBiSpatialStructure SpatialStructure { get; set; }
      public ParameterValuesBuildingBlock ParameterValues { set; get; }
   }
}
