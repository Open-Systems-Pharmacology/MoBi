using MoBi.Core.Domain;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IStringChangeToSelectDTOStringChangeMapper : IMapper<IStringChange, SelectStringChangeDTO>
   {
      void Initialize(bool defaultIsSelected);
   }
   class StringChangeToSelectDTOStringChangeMapper : IStringChangeToSelectDTOStringChangeMapper
   {
      private bool _defaultSelected;
      private readonly IObjectTypeResolver _objectTypeResolver;

      public StringChangeToSelectDTOStringChangeMapper(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public void Initialize(bool defaultIsSelected)
      {
         _defaultSelected = defaultIsSelected;
      }

      public SelectStringChangeDTO MapFrom(IStringChange input)
      {
         var dto = new SelectStringChangeDTO();
         dto.Selected = _defaultSelected;
         dto.Description = input.ChangeDescription;
         dto.Change = input;
         dto.BuildingBlock = $"{_objectTypeResolver.TypeFor(input.BuildingBlock)}:{input.BuildingBlock.DisplayName}";
         return dto;
      }
   }
}