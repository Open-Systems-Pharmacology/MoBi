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
      public MoleculeBuilder MoleculeBuilder { get; }
      public IContainer StructureParent { get; set; }

      public DummyMoleculeDTO(MoleculeBuilder moleculeBuilder) : base(moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
         Id = ShortGuid.NewGuid();
      }
   }
}