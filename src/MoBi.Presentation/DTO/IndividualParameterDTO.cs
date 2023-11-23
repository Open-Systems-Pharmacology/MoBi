using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class IndividualParameterDTO : PathAndValueEntityDTO<IndividualParameter, IndividualParameterDTO>
   {
      public IndividualParameterDTO(IndividualParameter individualParameter) : base(individualParameter)
      {
      }
   }
}