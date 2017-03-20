using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterToDummyParameterDTOMapper
   {
      DummyParameterDTO MapFrom(IParameter parameter, IContainer structureParent, IObjectBaseDTO modelParent);
   }

   internal class ParameterToDummyParameterDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IParameterToDummyParameterDTOMapper
   {
      public DummyParameterDTO MapFrom(IParameter parameter, IContainer structureParent, IObjectBaseDTO modelParent)
      {
         var dto = Map<DummyParameterDTO>(parameter);
         dto.ParameterToUse = Map<ObjectBaseDTO>(parameter);
         dto.Parent = structureParent;
         dto.ModelParentName = modelParent.Name;
         dto.Id = ShortGuid.NewGuid();
         return dto;
      }
   }
}