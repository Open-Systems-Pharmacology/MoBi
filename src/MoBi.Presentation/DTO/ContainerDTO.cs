using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ContainerDTO : ObjectBaseDTO
   {
      public ContainerMode Mode { set; get; }
      public ContainerType ContainerType { get; set; }

   }
}