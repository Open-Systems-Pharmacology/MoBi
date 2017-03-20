using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public interface ITaggedEntityDTO
   {
      IList<TagDTO> Tags { set; get; }
   }

   public class ContainerDTO : ObjectBaseDTO, ITaggedEntityDTO
   {
      public IList<TagDTO> Tags { set; get; }
      public ContainerMode Mode { set; get; }
      public ContainerType ContainerType { get; set; }
   }
}