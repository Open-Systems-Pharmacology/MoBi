using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Serialization.Exchange
{
   public class SpatialStructureTransfer
   {
      public MoBiSpatialStructure SpatialStructure { get; set; }
      public ParameterValuesBuildingBlock ParameterValues { set; get; }
      public InitialConditionsBuildingBlock InitialConditions { get; set; }
   }
}
