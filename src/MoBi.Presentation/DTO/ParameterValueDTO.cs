using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ParameterValueDTO : ExtendablePathAndValueEntityDTO<ParameterValue, ParameterValueDTO>
   {
      public ParameterValue ParameterValue => PathWithValueObject;

      public ParameterValueDTO(ParameterValue parameterValues, IBuildingBlock<ParameterValue> buildingBlock) : base(parameterValues, buildingBlock)
      {
      }

      public override void UpdateName(string newName)
      {
         ParameterValue.Name = newName;
      }
   }
}