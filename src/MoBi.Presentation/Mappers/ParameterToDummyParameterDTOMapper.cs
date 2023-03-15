using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IParameterToDummyParameterDTOMapper
   {
      DummyParameterDTO MapFrom(IParameter parameter, IContainer structureParent, ObjectBaseDTO modelParent);
   }

   internal class ParameterToDummyParameterDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IParameterToDummyParameterDTOMapper
   {
      public DummyParameterDTO MapFrom(IParameter parameter, IContainer structureParent, ObjectBaseDTO modelParent)
      {
         var dto = Map(new DummyParameterDTO(parameter));
         dto.Parent = structureParent;
         dto.ModelParentName = modelParent.Name;
         return dto;
      }
   }
}