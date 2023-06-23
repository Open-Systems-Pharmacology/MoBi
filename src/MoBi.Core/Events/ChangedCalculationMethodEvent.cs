using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class ChangedCalculationMethodEvent
   {
      public MoleculeBuilder MoleculeBuilder { get; set; }

      public ChangedCalculationMethodEvent(MoleculeBuilder moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
      }
   }
}