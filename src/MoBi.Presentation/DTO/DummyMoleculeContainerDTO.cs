using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.DTO
{
   public class DummyMoleculeContainerDTO : ObjectBaseDTO
   {
      public ObjectBaseDTO MoleculePropertiesContainer { get; set; }

      public DummyMoleculeContainerDTO(IObjectBase objectBase) : base(objectBase)
      {
         Id = ShortGuid.NewGuid();
      }
   }
}