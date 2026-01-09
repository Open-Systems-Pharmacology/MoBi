using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO;

public class ParameterValueWithInitialStateDTO<TParameter, TParameterDTO> : PathAndValueEntityDTO<TParameter, TParameterDTO> where TParameterDTO : PathAndValueEntityDTO<TParameter> where TParameter : ParameterValueWithInitialState
{
   protected ParameterValueWithInitialStateDTO(TParameter pathAndValueEntity) : base(pathAndValueEntity)
   {
   }

   public bool HasInitialState => PathWithValueObject.HasInitialState;
}