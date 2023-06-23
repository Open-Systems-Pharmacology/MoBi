using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class IndividualParameterDTO : PathAndValueEntityDTO<IndividualParameter>, IWithFormulaDTO
   {
      public IndividualParameterDTO(IndividualParameter individualParameter) : base(individualParameter)
      {
      }
   }
}