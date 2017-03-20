using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class MoleculeIconChangedEvent
   {
      public IMoleculeBuilder MoleculeBuilder { get; set; }

      public MoleculeIconChangedEvent(IMoleculeBuilder moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
      }
   }
}