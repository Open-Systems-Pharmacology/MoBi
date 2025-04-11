using MoBi.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Services
{
   public interface IEntitySourceMapper : IMapper<EntitySource, EntitySourceReference>
   {
   }

   public class EntitySourceMapper : IEntitySourceMapper
   {
      private readonly IWithIdRepository _repository;

      public EntitySourceMapper(IWithIdRepository repository)
      {
         _repository = repository;
      }

      public EntitySourceReference MapFrom(EntitySource entitySource)
      {
         var buildingBlock = _repository.Get<IBuildingBlock>(entitySource.BuildingBlockId);
         var module = buildingBlock?.Module;
         var source = _repository.Get<IEntity>(entitySource.SourceId);
         return new EntitySourceReference(source, buildingBlock, module);
      }
   }
};