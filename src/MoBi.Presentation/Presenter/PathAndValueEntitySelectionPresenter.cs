using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Helper;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IPathAndValueEntitySelectionPresenter : IDisposablePresenter, IPresenter<IPathAndValueEntitySelectionView>
   {
      IReadOnlyList<TPathAndValueEntity> SelectReplacementEntities<TPathAndValueEntity>(IReadOnlyList<TPathAndValueEntity> newEntities, IBuildingBlock<TPathAndValueEntity> buildingBlock) where TPathAndValueEntity : PathAndValueEntity;
   }

   public class PathAndValueEntitySelectionPresenter : AbstractDisposablePresenter<IPathAndValueEntitySelectionView, IPathAndValueEntitySelectionPresenter>, IPathAndValueEntitySelectionPresenter
   {
      private readonly IPathAndValueEntityToSelectableDTOMapper _mapper;

      public PathAndValueEntitySelectionPresenter(IPathAndValueEntitySelectionView view, IPathAndValueEntityToSelectableDTOMapper mapper) : base(view)
      {
         _mapper = mapper;
      }

      public IReadOnlyList<TPathAndValueEntity> SelectReplacementEntities<TPathAndValueEntity>(IReadOnlyList<TPathAndValueEntity> newEntities, IBuildingBlock<TPathAndValueEntity> buildingBlock) where TPathAndValueEntity : PathAndValueEntity
      {
         var oldAndNewValues = newEntities.Select(newEntity => (replacementEntity: newEntity, oldEntity: collidingEntity(buildingBlock, newEntity))).ToList();
         var collidingEntities = oldAndNewValues.Where(tuple => tuple.oldEntity != null).ToList();
         if (!collidingEntities.Any())
            return newEntities;

         var selectedEntities = oldAndNewValues.Except(collidingEntities).Select(tuple => tuple.replacementEntity).ToList();

         return selectedEntities.Concat(selectFromCollidingEntities(buildingBlock, collidingEntities)).ToList();
      }

      private IEnumerable<TPathAndValueEntity> selectFromCollidingEntities<TPathAndValueEntity>(IBuildingBlock<TPathAndValueEntity> buildingBlock, List<(TPathAndValueEntity replacementEntity, TPathAndValueEntity oldEntity)> collidingEntities) where TPathAndValueEntity : PathAndValueEntity
      {
         var entityType = new ObjectTypeResolver().TypeFor<TPathAndValueEntity>();
         var selectableObjectPathDTOs = collidingEntities.Select(x => _mapper.MapFrom(x.replacementEntity, x.oldEntity)).ToList();
         _view.Caption = AppConstants.Captions.SelectEntitiesThatWillBeReplaced(entityType);
         _view.SetDescription(AppConstants.Captions.SelectEntitiesThatWillBeReplacedDescription(entityType, buildingBlock.DisplayName));
         _view.AddSelectableEntities(selectableObjectPathDTOs);
         _view.Display();

         return _view.Canceled ? new List<TPathAndValueEntity>() : selectableObjectPathDTOs.Where(x => x.Selected).Select(x => x.NewEntity);
      }

      private TPathAndValueEntity collidingEntity<TPathAndValueEntity>(IBuildingBlock<TPathAndValueEntity> buildingBlock, PathAndValueEntity newEntity) where TPathAndValueEntity : PathAndValueEntity
      {
         return buildingBlock.FirstOrDefault(x => x.Path.Equals(newEntity.Path));
      }
   }
}