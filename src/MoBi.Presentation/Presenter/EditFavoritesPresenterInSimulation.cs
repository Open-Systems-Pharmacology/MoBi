﻿using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesInSimulationPresenter : IEditFavoritesPresenter<IMoBiSimulation>, IEditInSimulationPresenter
   {
      IEnumerable<IParameter> Favorites();
   }

   public class EditFavoritesInSimulationPresenter : EditFavoritesPresenter<IMoBiSimulation>, IEditFavoritesInSimulationPresenter
   {
      public EditFavoritesInSimulationPresenter(
         IEditFavoritesView view,
         IFavoriteRepository favoriteRepository,
         IEntityPathResolver entityPathResolver,
         IEditParameterListPresenter editParameterListPresenter,
         IFavoriteTask favoriteTask)
         : base(view, favoriteRepository, entityPathResolver, editParameterListPresenter, favoriteTask)
      {
         ShouldHandleRemovedEvent = x => false; //Can not remove in Simulation
      }

      protected override void CacheParameters(IMoBiSimulation projectItem)
      {
         var allParameters = projectItem.Model.Root.GetAllChildren<IParameter>();
         _parameterCache.AddRange(allParameters);
      }

      protected override bool IsAddedToParent(IObjectBase parent)
      {
         var parentContainer = parent as IContainer;
         return parentContainer != null && Simulation.Model.Root.GetAllContainersAndSelf<IContainer>().Contains(parentContainer);
      }

      public IMoBiSimulation Simulation
      {
         get => _projectItem;
         set => _projectItem = value;
      }

      public IEnumerable<IParameter> Favorites()
      {
         return _editParameterListPresenter.EditedParameters;
      }

      public override void Edit(IMoBiSimulation projectItem)
      {
         base.Edit(projectItem);
         _editParameterListPresenter.SetVisibility(PathElementId.Simulation, isVisible: false);
      }
   }
}