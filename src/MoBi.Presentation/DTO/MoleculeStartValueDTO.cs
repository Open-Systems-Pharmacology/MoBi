using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class MoleculeStartValueDTO : StartValueDTO<MoleculeStartValue>
   {
      public bool IsPresent
      {
         get { return StartValueObject.IsPresent; }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public bool NegativeValuesAllowed
      {
         get { return StartValueObject.NegativeValuesAllowed; }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public double ScaleDivisor
      {
         get { return StartValueObject.ScaleDivisor; }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public MoleculeStartValue MoleculeStartValue
      {
         get { return StartValueObject; }
      }

      public MoleculeStartValueDTO(MoleculeStartValue moleculeStartValue, IStartValuesBuildingBlock<MoleculeStartValue> buildingBlock) : base(moleculeStartValue, buildingBlock)
      {
      }

      public override void UpdateStartValueName(string newName)
      {
         MoleculeStartValue.Name = newName;
      }
   }
}