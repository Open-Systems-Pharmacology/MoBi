﻿using System;
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
         var parent = EntityFrom(parentDTO);
         if (parent.IsAnImplementationOf<IDistributedParameter>())
            return Array.Empty<ObjectBaseDTO>();

         if (!(parent is IContainer container))
            return Array.Empty<ObjectBaseDTO>();

         //Add sub containers removing molecule properties and parameters
         return container.GetChildrenSortedByName<IContainer>(x =>
            !x.IsNamed(Constants.MOLECULE_PROPERTIES) && !x.IsAnImplementationOf<IParameter>()).MapAllUsing(_containerDTOMapper);
      }

      public bool ContainerSelected => SelectedEntity != null;
   }
}