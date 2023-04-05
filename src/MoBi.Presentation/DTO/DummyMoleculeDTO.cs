using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.DTO
{
   public interface IDummyContainer
   {
      IContainer StructureParent { get; set; }
   }

   public class DummyMoleculeDTO : ObjectBaseDTO, IDummyContainer
   {
      public IMoleculeBuilder MoleculeBuilder { get; }
      public IContainer StructureParent { get; set; }

      public DummyMoleculeDTO(IMoleculeBuilder moleculeBuilder) : base(moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
         Id = ShortGuid.NewGuid();
      }
   }
}