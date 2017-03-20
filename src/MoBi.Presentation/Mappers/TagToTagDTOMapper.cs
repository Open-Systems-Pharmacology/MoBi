using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Presentation.Mappers
{
   public interface ITagToTagDTOMapper : IMapper<Tag, TagDTO>
   {
   }

   internal class TagToTagDTOMapper : ITagToTagDTOMapper
   {
      public TagDTO MapFrom(Tag tag)
      {
         return new TagDTO(tag.Value);
      }
   }
}