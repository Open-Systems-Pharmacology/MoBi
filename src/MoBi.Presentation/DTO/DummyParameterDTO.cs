using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.DTO
{
   public class DummyParameterDTO : ObjectBaseDTO
   {
      public IParameter Parameter { get; }
      public string ModelParentName { get; set; }
      public IContainer Parent { get; set; }

      public DummyParameterDTO(IParameter parameter) : base(parameter)
      {
         Parameter = parameter;
         if (Id == null)
            Id = ShortGuid.NewGuid();
      }
   }
}