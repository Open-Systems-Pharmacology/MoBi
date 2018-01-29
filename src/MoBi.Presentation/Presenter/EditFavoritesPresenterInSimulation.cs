using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IEditFavoritesInSimulationPresenter : IEditFavoritesPresenter<IMoBiSimulation>, IEditInSimulationPresenter
   {
      IEnumerable<IParameter> Favorites();
   }

   public class EditFavoritesInSimulationPresenter : EditFavoritesPresenter<IMoBiSimulation>, IEditFavoritesInSimulationPresenter
   {
      public EditFavoritesInSimulationPresenter(
         IEditParameterListView view, 
         IQuantityTask quantityTask,
         IInteractionTaskContext interactionTaskContext, 
         IFormulaToFormulaBuilderDTOMapper formulaMapper,
         IParameterToParameterDTOMapper parameterDTOMapper, 
         IFavoriteRepository favoriteRepository,
         IInteractionTasksForParameter parameterTask, 
         IFavoriteTask favoriteTask, 
         IEntityPathResolver entityPathResolver, 
         IViewItemContextMenuFactory contextMenusFactory)
         : base(view, quantityTask, interactionTaskContext, formulaMapper, parameterDTOMapper,
            favoriteRepository, parameterTask, favoriteTask, entityPathResolver, contextMenusFactory)
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
         return _favorites.Select(x => x.Parameter);
      }
   }
}