using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class ObserverBuilderDTO : ObjectBaseDTO
   {
      public ObserverBuilderDTO(ObserverBuilder observerBuilder) : base(observerBuilder)
      {
      }

      public IDimension Dimension { get; set; }
      public FormulaBuilderDTO Monitor { get; set; }
      public string MonitorString { get; set; }
      public bool ForAll { get; set; }
      public IEnumerable<string> MoleculeNames { get; set; }
   }
}