using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class ParameterStartValueDTO : StartValueDTO<IParameterStartValue>
   {
      public IParameterStartValue ParameterStartValue => StartValueObject;

      public ParameterStartValueDTO(IParameterStartValue parameterStartValue, IStartValuesBuildingBlock<IParameterStartValue> buildingBlock) : base(parameterStartValue, buildingBlock)
      {
      }

      public override void UpdateStartValueName(string newName)
      {
         ParameterStartValue.Name = newName;
      }
   }

}