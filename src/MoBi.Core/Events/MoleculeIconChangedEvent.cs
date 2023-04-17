using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class MoleculeIconChangedEvent
   {
      public MoleculeBuilder MoleculeBuilder { get; set; }

      public MoleculeIconChangedEvent(MoleculeBuilder moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
      }
   }
}