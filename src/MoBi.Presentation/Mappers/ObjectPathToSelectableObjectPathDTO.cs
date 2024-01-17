using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IPathAndValueEntityToSelectableDTOMapper
   {
      SelectableReplacePathAndValueDTO<TPathAndValueEntity> MapFrom<TPathAndValueEntity>(TPathAndValueEntity newEntity, TPathAndValueEntity oldEntity) where TPathAndValueEntity : PathAndValueEntity;
   }

   public class PathAndValueEntityToSelectableDTOMapper : IPathAndValueEntityToSelectableDTOMapper
   {
      public SelectableReplacePathAndValueDTO<TPathAndValueEntity> MapFrom<TPathAndValueEntity>(TPathAndValueEntity newEntity, TPathAndValueEntity oldEntity) where TPathAndValueEntity : PathAndValueEntity
      {
         return new SelectableReplacePathAndValueDTO<TPathAndValueEntity>(newEntity, oldEntity);
      }
   }
}
