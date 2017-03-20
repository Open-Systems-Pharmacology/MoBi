using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseToDummyMoleculeDTOMapper : IMapper<IObjectBase, DummyMoleculeContainerDTO>
   {
      void Initialise(IContainer moleculePropertiesContainer);
   }

   public class ObjectBaseToDummyMoleculeDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IObjectBaseToDummyMoleculeDTOMapper
   {
      private IContainer _moleculePropertiesContainer;
      private readonly IObjectBaseToObjectBaseDTOMapper _objectBaseToDTOObjectBaseMapper;

      public ObjectBaseToDummyMoleculeDTOMapper(IObjectBaseToObjectBaseDTOMapper objectBaseToDTOObjectBaseMapper)
      {
         _objectBaseToDTOObjectBaseMapper = objectBaseToDTOObjectBaseMapper;
      }

      public void Initialise(IContainer moleculePropertiesContainer)
      {
         _moleculePropertiesContainer = moleculePropertiesContainer;
      }

      public DummyMoleculeContainerDTO MapFrom(IObjectBase objectBase)
      {
         var dto = Map<DummyMoleculeContainerDTO>(objectBase);
         dto.Id = ShortGuid.NewGuid();
         dto.MoleculePropertiesContainer = _objectBaseToDTOObjectBaseMapper.MapFrom(_moleculePropertiesContainer);
         return dto;
      }
   }
}