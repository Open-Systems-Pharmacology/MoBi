using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class DummyParameterDTO : ObjectBaseDTO
   {
      public ObjectBaseDTO ParameterToUse { get; set; }
      public string ModelParentName { get; set; }
      public IContainer Parent { get; set; }
   }
}