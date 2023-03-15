using OSPSuite.Core.Domain;

namespace MoBi.Presentation.DTO
{
   public class DummyParameterDTO : ObjectBaseDTO
   {
      public DummyParameterDTO() : base()
      {
      }
      public DummyParameterDTO(IParameter parameter) : base(parameter)
      {
      }
      public ObjectBaseDTO ParameterToUse { get; set; }
      public string ModelParentName { get; set; }
      public IContainer Parent { get; set; }
   }
}