using System.Collections.Generic;
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
      private TrackableSimulation _trackableSimulation;

      public EditFavoritesInSimulationPresenter(
         IEditFavoritesView view,
         IFavoriteRepository favoriteRepository,
         IEntityPathResolver entityPathResolver,
         IEditParameterListPresenter editParameterListPresenter,
         IFavoriteTask favoriteTask)
         : base(view, favoriteRepository, entityPathResolver, editParameterListPresenter, favoriteTask)
      {
         ShouldHandleRemovedEvent = x => false; //Can not remove in Simulation
         UpdateSpecialColumnsVisibility = _editParameterListPresenter.ConfigureForSimulation;
      }

      protected override void CacheParameters(IMoBiSimulation projectItem)
      {
         var allParameters = projectItem.Model.Root.GetAllChildren<IParameter>();
         _parameterCache.AddRange(allParameters);
      }

      protected override bool IsAddedToParent(IObjectBase parent)
      {
         var parentContainer = parent as IContainer;
         return parentContainer != null && _trackableSimulation.Simulation.Model.Root.GetAllContainersAndSelf<IContainer>().Contains(parentContainer);
      }

      public TrackableSimulation TrackableSimulation
      {
         get => _trackableSimulation;
         set
         {
            _trackableSimulation = value;
            _projectItem = value.Simulation;
         }
      }

      public IEnumerable<IParameter> Favorites()
      {
         return _editParameterListPresenter.EditedParameters;
      }
   }
}