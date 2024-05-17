using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalStructurePresenter : IPresenterWithContextMenu<IViewItem>
   {
      IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate);
      void Select(ObjectBaseDTO objectBaseDTO);
      void Clear();
   }

   public abstract class HierarchicalStructurePresenter : AbstractCommandCollectorPresenter<IHierarchicalStructureView, IHierarchicalStructurePresenter>
   {
      protected readonly IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseMapper;

      protected HierarchicalStructurePresenter(
         IHierarchicalStructureView view,
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper)
         : base(view)
      {
         _context = context;
         _objectBaseMapper = objectBaseMapper;
      }

      public virtual IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         return (dto.ObjectBase is IContainer container) ? GetChildrenSorted(container, predicate) : Array.Empty<ObjectBaseDTO>();
      }

      protected virtual IReadOnlyList<ObjectBaseDTO> GetChildrenSorted(IContainer container, Func<IEntity, bool> predicate)
      {
         IReadOnlyList<ObjectBaseDTO> allChildrenDTO()
         {
            return container.GetChildren(predicate)
               .OrderBy(groupingTypeFor)
               .ThenBy(x => x.Name)
               .MapAllUsing(_objectBaseMapper);
         }

         switch (container)
         {
            case NeighborhoodBuilder neighborhood:
               return neighborsOf(neighborhood).Union(allChildrenDTO()).ToList();
            default:
               return allChildrenDTO();
         }
      }

      private IEnumerable<ObjectBaseDTO> neighborsOf(NeighborhoodBuilder neighborhoodBuilder)
      {
         if (neighborhoodBuilder.FirstNeighborPath != null)
            yield return new NeighborDTO(neighborhoodBuilder.FirstNeighborPath) { Icon = ApplicationIcons.Neighbor, Id = createNeighborhoodId(neighborhoodBuilder, neighborhoodBuilder.FirstNeighborPath) };

         if (neighborhoodBuilder.SecondNeighborPath != null)
            yield return new NeighborDTO(neighborhoodBuilder.SecondNeighborPath) { Icon = ApplicationIcons.Neighbor, Id = createNeighborhoodId(neighborhoodBuilder, neighborhoodBuilder.SecondNeighborPath) };
      }

      /// <summary>
      ///    Creates an Id for neighbors in a neighborhood.
      ///    The Id must be distinct for each neighborhood and neighbor, so it cannot be just the
      ///    <paramref name="neighborPath" />, but must
      ///    include the id of the <paramref name="neighborhood" />
      /// </summary>
      /// <returns>An Id that combines the two Ids of the neighborhood and neighbor</returns>
      private string createNeighborhoodId(NeighborhoodBuilder neighborhood, ObjectPath neighborPath)
      {
         return $"{neighborhood.Id}-{neighborPath}";
      }

      private ContainerType groupingTypeFor(IEntity entity)
      {
         var container = entity as IContainer;
         return container?.ContainerType ?? ContainerType.Other;
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         Clear();
      }

      public void Clear()
      {
         _view.Clear();
      }

      public abstract void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation);
   }
}