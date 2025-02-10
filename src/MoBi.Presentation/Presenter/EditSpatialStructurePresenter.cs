using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSpatialStructurePresenter : ISingleStartPresenter<MoBiSpatialStructure>,
      IDiagramBuildingBlockPresenter,
      IListener<RemovedEvent>,
      IListener<NeighborhoodChangedEvent>
   {
      void LoadDiagram();
   }

   public class EditSpatialStructurePresenter : EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase<IEditSpatialStructureView, IEditSpatialStructurePresenter, MoBiSpatialStructure, IContainer>,
      IEditSpatialStructurePresenter
   {
      private MoBiSpatialStructure _spatialStructure;
      private readonly IHierarchicalSpatialStructurePresenter _hierarchicalSpatialStructurePresenter;
      private readonly ISpatialStructureDiagramPresenter _spatialStructureDiagramPresenter;
      private readonly IEditContainerPresenter _editContainerPresenter;
      private bool _diagramLoaded;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IEditNeighborhoodBuilderPresenter _neighborhoodBuilderPresenter;

      public EditSpatialStructurePresenter(IEditSpatialStructureView view,
         IFormulaCachePresenter formulaCachePresenter,
         IEditFavoritesInSpatialStructurePresenter favoritesPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter,
         IHierarchicalSpatialStructurePresenter hierarchicalSpatialStructurePresenter,
         ISpatialStructureDiagramPresenter spatialStructureDiagramPresenter,
         IEditContainerPresenter editContainerPresenter,
         IHeavyWorkManager heavyWorkManager,
         IEditNeighborhoodBuilderPresenter neighborhoodBuilderPresenter) :
         base(view, formulaCachePresenter, favoritesPresenter, userDefinedParametersPresenter)
      {
         _hierarchicalSpatialStructurePresenter = hierarchicalSpatialStructurePresenter;
         _spatialStructureDiagramPresenter = spatialStructureDiagramPresenter;
         _heavyWorkManager = heavyWorkManager;
         _neighborhoodBuilderPresenter = neighborhoodBuilderPresenter;
         favoritesPresenter.ShouldHandleRemovedEvent = shouldHandleRemoved;
         _editContainerPresenter = editContainerPresenter;
         ShowView(_editContainerPresenter.BaseView);
         _view.SetHierarchicalStructureView(_hierarchicalSpatialStructurePresenter.BaseView);
         _view.SetSpaceDiagramView(spatialStructureDiagramPresenter.View);
         AddSubPresenters(_editContainerPresenter, hierarchicalSpatialStructurePresenter, spatialStructureDiagramPresenter, _neighborhoodBuilderPresenter);
      }

      public override void Edit(MoBiSpatialStructure spatialStructure)
      {
         _diagramLoaded = (_spatialStructure == spatialStructure) && _diagramLoaded;
         _spatialStructure = spatialStructure;
         EditFormulas(spatialStructure);
         _editContainerPresenter.BuildingBlock = _spatialStructure;
         _neighborhoodBuilderPresenter.BuildingBlock = _spatialStructure;
         _hierarchicalSpatialStructurePresenter.Edit(spatialStructure);
         _favoritesPresenter.Edit(spatialStructure);
         setInitialView();
         UpdateCaption();
         _view.Display();
      }

      private void setInitialView()
      {
         ShowView(_favoritesPresenter.BaseView);
      }

      public override object Subject => _spatialStructure;

      protected override (bool canHandle, IContainer containerObject) SpecificCanHandle(EntitySelectedEvent entitySelectedEvent)
      {
         if(entitySelectedEvent.ObjectBase == null && entitySelectedEvent.Sender == _hierarchicalSpatialStructurePresenter)
            return (canHandle:true, containerObject: null);

         return (shouldHandleSelection(entitySelectedEvent.ObjectBase as IEntity), null);
      }

      internal override (bool canHandle, IContainer containerObject) CanHandle(EntitySelectedEvent entitySelectedEvent)
      {
         var specificCanHandle = SpecificCanHandle(entitySelectedEvent);
         if (specificCanHandle.Item1)
            return specificCanHandle;

         return base.CanHandle(entitySelectedEvent);
      }

      protected override void EnsureItemsVisibility(IContainer parentObject, IParameter parameter = null)
      {
         setupEditPresenterFor(parentObject, parameter);
      }

      protected override void SelectBuilder(IContainer builder)
      {
         setupEditPresenterFor(builder);
      }

      private void showPresenter<T>(IEditPresenterWithParameters<T> presenter, T objectToEdit, IParameter parameter)
      {
         presenter.Edit(objectToEdit);
         ShowView(presenter.BaseView);

         if (parameter != null)
            presenter.SelectParameter(parameter);
      }

      private void setupEditPresenterFor(IContainer container, IParameter parameter = null)
      {
         if (container == null)
         {
            _view.SetEditView(null);
            return;
         }

         switch (container)
         {
            case IDistributedParameter distributedParameter:
               setupEditPresenterFor(distributedParameter.ParentContainer, distributedParameter);
               return;
            case NeighborhoodBuilder neighborhoodBuilder:
               showPresenter(_neighborhoodBuilderPresenter, neighborhoodBuilder, parameter);
               return;
            default:
               showPresenter(_editContainerPresenter, container, parameter);
               return;
         }
      }

      private bool shouldHandleSelection(IEntity entity)
      {
         if (!(entity is IContainer container))
            return false;

         return !container.IsAnImplementationOf<IDistributedParameter>() && _spatialStructure.IsInSpatialStructure(container);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.SpatialStructureBuildingBlockCaption(_spatialStructure.DisplayName);
      }

      public void ZoomIn()
      {
         _spatialStructureDiagramPresenter.Zoom(AppConstants.Diagram.Space.ZoomInFactor);
      }

      public void ZoomOut()
      {
         _spatialStructureDiagramPresenter.Zoom(1 / AppConstants.Diagram.Space.ZoomInFactor);
      }

      public void FitToPage()
      {
         _spatialStructureDiagramPresenter.Zoom(AppConstants.Diagram.Base.ZoomFitToPageFactor);
      }

      public void LayoutByForces()
      {
         _spatialStructureDiagramPresenter.Layout(null, AppConstants.Diagram.Base.LayoutDepthChildren, null);
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (!eventToHandle.RemovedObjects.Any(shouldHandleRemoved))
            return;

         setInitialView();
      }

      private bool shouldHandleRemoved(IObjectBase objectBase) => Equals(objectBase, _editContainerPresenter.Subject);

      public void LoadDiagram()
      {
         if (_diagramLoaded) return;
         _heavyWorkManager.Start(() => _spatialStructureDiagramPresenter.Edit(_spatialStructure), AppConstants.Captions.LoadingDiagram);
         _diagramLoaded = true;
      }

      protected override void ShowView(IView viewToShow) => _view.SetEditView(viewToShow);

      public void Handle(NeighborhoodChangedEvent neighborhoodChangedEvent)
      {
         if (!canHandleNeighborhoodChange(neighborhoodChangedEvent))
            return;

         //The neighborhood has changed. We need to refresh the presenter to reflect changes 
         _hierarchicalSpatialStructurePresenter.Refresh(neighborhoodChangedEvent.NeighborhoodBuilder);
      }

      private bool canHandleNeighborhoodChange(NeighborhoodChangedEvent neighborhoodChangedEvent)
      {
         return _spatialStructure.Neighborhoods.Contains(neighborhoodChangedEvent.NeighborhoodBuilder);
      }
   }
}