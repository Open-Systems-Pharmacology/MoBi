using System;
using System.Collections.Generic;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
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
      private readonly IInteractionTasksForTopContainer _tasksForTopContainer;

      public SelectContainerInTreePresenter(ISelectEntityInTreeView view,
         IObjectPathFactory objectPathFactory,
         IMoBiContext context,
         IContainerToContainerDTOMapper containerDTOMapper,
         IObjectBaseDTOToSpatialStructureNodeMapper spatialStructureNodeMapper,
         IInteractionTasksForTopContainer tasksForTopContainer) : base(view, objectPathFactory, context, spatialStructureNodeMapper)
      {
         _containerDTOMapper = containerDTOMapper;
         _tasksForTopContainer = tasksForTopContainer;
         GetChildren = getChildren;
      }

      public override ObjectPath SelectedEntityPath => _tasksForTopContainer.BuildObjectPath(SelectedEntity as IContainer);

      private IReadOnlyList<ObjectBaseDTO> getChildren(ObjectBaseDTO parentDTO)
      {
         if (parentDTO is ModuleAndSpatialStructureDTO moduleAndSpatialStructureDTO)
            return moduleAndSpatialStructureDTO.SpatialStructure.TopContainers;

         var parent = EntityFrom(parentDTO);
         if (parent.IsAnImplementationOf<IDistributedParameter>())
            return Array.Empty<ObjectBaseDTO>();

         if (!(parent is IContainer container))
            return Array.Empty<ObjectBaseDTO>();

         //Add sub containers removing molecule properties and parameters
         return container.GetChildrenSortedByName<IContainer>(x =>
            !x.IsNamed(Constants.MOLECULE_PROPERTIES) && !x.IsAnImplementationOf<IParameter>()).MapAllUsing(_containerDTOMapper);
      }

      public override void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs)
      {
         base.InitTreeStructure(entityDTOs);
         _view.ExpandRootNodes();
      }

      public bool ContainerSelected => SelectedEntity != null;
   }
}