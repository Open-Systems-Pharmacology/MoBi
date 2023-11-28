using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectContainerInTreePresenter : ISelectEntityInTreePresenter
   {
      bool ContainerSelected { get; }
   }

   public class SelectContainerInTreePresenter : SelectEntityInTreePresenter, ISelectContainerInTreePresenter
   {
      private readonly IContainerToContainerDTOMapper _containerDTOMapper;

      public SelectContainerInTreePresenter(ISelectEntityInTreeView view,
         IObjectPathFactory objectPathFactory,
         IMoBiContext context,
         IContainerToContainerDTOMapper containerDTOMapper,
         IObjectBaseDTOToSpatialStructureNodeMapper spatialStructureNodeMapper) : base(view, objectPathFactory, context, spatialStructureNodeMapper)
      {
         _containerDTOMapper = containerDTOMapper;
         GetChildren = getChildren;
      }

      private IReadOnlyList<ObjectBaseDTO> getChildren(ObjectBaseDTO parentDTO)
      {
         if (parentDTO is SpatialStructureDTO spatialStructureDTO)
            return spatialStructureDTO.TopContainers.ToList();

         var parent = parentDTO.ObjectBase;
         if (parent.IsAnImplementationOf<IDistributedParameter>())
            return Array.Empty<ObjectBaseDTO>();

         if (!(parent is IContainer container))
            return Array.Empty<ObjectBaseDTO>();

         //Add sub containers removing molecule properties and parameters
         var subContainers = subContainersFor(container);
         return subContainers.MapAllUsing(_containerDTOMapper);
      }

      private static IEnumerable<IContainer> subContainersFor(IContainer container)
      {
         var subContainers = container.GetChildrenSortedByName<IContainer>(x =>
            !x.IsNamed(Constants.MOLECULE_PROPERTIES) && !x.IsAnImplementationOf<IParameter>()
         );
         return subContainers;
      }

      public bool ContainerSelected => SelectedEntity != null;
   }
}