using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ParameterStartValueDTO : StartValueDTO<ParameterStartValue>
   {
      public ParameterStartValue ParameterStartValue => PathWithValueObject;

      public ParameterStartValueDTO(ParameterStartValue parameterStartValue, IStartValuesBuildingBlock<ParameterStartValue> buildingBlock) : base(parameterStartValue, buildingBlock)
      {
      }

      public override void UpdateStartValueName(string newName)
      {
         ParameterStartValue.Name = newName;
      }
   }
}