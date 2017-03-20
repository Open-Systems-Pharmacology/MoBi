using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class NewMoleculeBuildingBlockDescription
   {
      public IEnumerable<IMoleculeBuilder> Molecules { set; get; }
      public string Name { set; get; }
   }
}