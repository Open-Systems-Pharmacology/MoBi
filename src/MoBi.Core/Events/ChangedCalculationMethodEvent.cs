using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Events
{
   public class ChangedCalculationMethodEvent
   {
      public IMoleculeBuilder MoleculeBuilder { get; set; }

      public ChangedCalculationMethodEvent(IMoleculeBuilder moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
      }
   }
}