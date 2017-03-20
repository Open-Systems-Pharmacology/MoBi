using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public interface IDummyContainer
   {
      IContainer StructureParent { get; set; }
   }

   public class DummyMoleculeDTO : ObjectBaseDTO, IDummyContainer
   {
      public IMoleculeBuilder MoleculeBuilder { set; get; }
      public IContainer StructureParent { get; set; }
   }  
}