using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderToDummyMoleculeDTOMapper
   {
      DummyMoleculeDTO MapFrom(IMoleculeBuilder moleculeBuilder, IContainer container);
   }

   internal class MoleculeBuilderToDummyMoleculeDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IMoleculeBuilderToDummyMoleculeDTOMapper
   {
      public DummyMoleculeDTO MapFrom(IMoleculeBuilder moleculeBuilder, IContainer container)
      {
         var dto = Map(new DummyMoleculeDTO(moleculeBuilder));
         dto.StructureParent = container;
         return dto;
      }
   }
}