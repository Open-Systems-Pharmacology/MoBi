using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ParameterStartValueDTO : StartValueDTO<ParameterValue>
   {
      public ParameterValue ParameterValue => PathWithValueObject;

      public ParameterStartValueDTO(ParameterValue parameterStartValue, IStartValuesBuildingBlock<ParameterValue> buildingBlock) : base(parameterStartValue, buildingBlock)
      {
      }

      public override void UpdateStartValueName(string newName)
      {
         ParameterValue.Name = newName;
      }
   }
}