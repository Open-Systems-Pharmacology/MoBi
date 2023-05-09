using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class InitialConditionDTO : StartValueDTO<InitialCondition>
   {
      public bool IsPresent
      {
         get => PathWithValueObject.IsPresent;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public bool NegativeValuesAllowed
      {
         get => PathWithValueObject.NegativeValuesAllowed;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public double ScaleDivisor
      {
         get => PathWithValueObject.ScaleDivisor;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public InitialCondition InitialCondition => PathWithValueObject;

      public InitialConditionDTO(InitialCondition initialCondition, PathAndValueEntityBuildingBlock<InitialCondition> buildingBlock) : base(initialCondition, buildingBlock)
      {
      }

      public override void UpdateName(string newName)
      {
         InitialCondition.Name = newName;
      }
   }
}