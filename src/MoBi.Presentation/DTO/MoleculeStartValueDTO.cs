using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class MoleculeStartValueDTO : StartValueDTO<InitialCondition>
   {
      public bool IsPresent
      {
         get { return PathWithValueObject.IsPresent; }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public bool NegativeValuesAllowed
      {
         get { return PathWithValueObject.NegativeValuesAllowed; }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public double ScaleDivisor
      {
         get { return PathWithValueObject.ScaleDivisor; }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public InitialCondition MoleculeStartValue
      {
         get { return PathWithValueObject; }
      }

      public MoleculeStartValueDTO(InitialCondition moleculeStartValue, IStartValuesBuildingBlock<InitialCondition> buildingBlock) : base(moleculeStartValue, buildingBlock)
      {
      }

      public override void UpdateStartValueName(string newName)
      {
         MoleculeStartValue.Name = newName;
      }
   }
}