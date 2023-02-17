using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class ContainerDTO : ObjectBaseDTO
   {
      public ContainerMode Mode { set; get; }

      public ContainerType ContainerType { get; set; }

      //Path to parent container. It should always be defined, even if the underlying container does not have it set
      //as it is inferred from the container structure. However only editable if there are no parent
      public string ParentPath { get; set; }
      public bool ParentPathEditable { get; set; }
   }
}