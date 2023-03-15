using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSpatialStructurePresenter : ISingleStartPresenter<IMoBiSpatialStructure>,
      IDiagramBuildingBlockPresenter,
      IListener<RemovedEvent>
   {
      void LoadDiagram();
   }

   public class EditSpatialStructurePresenter : EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase<IEditSpatialStructureView, IEditSpatialStructurePresenter, IMoBiSpatialStructure, IContainer>,
      IEditSpatialStructurePresenter
   {
      private IMoBiSpatialStructure _spatialStructure;
      private readonly IHierarchicalSpatialStructurePresenter _hierarchicalSpatialStructurePresenter;
      private readonly ISpatialStructureDiagramPresenter _spatialStructureDiagramPresenter;
      private readonly IEditContainerPresenter _editPresenter;
      private bool _diagramLoaded;
      private readonly IHeavyWorkManager _heavyWorkManager;

      public EditSpatialStructurePresenter(
         IEditSpatialStructureView view,
         IHierarchicalSpatialStructurePresenter hierarchicalSpatialStructurePresenter,
         IFormulaCachePresenter formulaCachePresenter,
         IEditContainerPresenter editPresenter,
         ISpatialStructureDiagramPresenter spatialStructureDiagramPresenter,
         IHeavyWorkManager heavyWorkManager,
         IEditFavoritesInSpatialStructurePresenter favoritesPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter) :
         base(view, formulaCachePresenter, favoritesPresenter, userDefinedParametersPresenter)
      {
         _hierarchicalSpatialStructurePresenter = hierarchicalSpatialStructurePresenter;
         _spatialStructureDiagramPresenter = spatialStructureDiagramPresenter;
         _heavyWorkManager = heavyWorkManager;
         favoritesPresenter.ShouldHandleRemovedEvent = shouldHandleRemoved;
         _editPresenter = editPresenter;
         _view.SetEditView(_editPresenter.BaseView);
         _view.SetHierarchicalStructureView(_hierarchicalSpatialStructurePresenter.BaseView);
         _view.SetSpaceDiagramView(spatialStructureDiagramPresenter.View);
         AddSubPresenters(editPresenter, hierarchicalSpatialStructurePresenter, spatialStructureDiagramPresenter);
      }

      public override void Edit(IMoBiSpatialStructure spatialStructure)
      {
         _diagramLoaded = (_spatialStructure == spatialStructure) && _diagramLoaded;
         _spatialStructure = spatialStructure;
         EditFormulas(spatialStructure);
         _editPresenter.BuildingBlock = _spatialStructure;
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

      protected override (bool canHandle, IObjectBase objectBase) SpecificCanHandle(IObjectBase selectedObject)
      {
         return (shouldHandleSelection(selectedObject as IEntity), selectedObject);
      }

      internal override (bool canHandle, IObjectBase objectBase) CanHandle(IObjectBase selectedObject)
      {
         var specificCanHandle = SpecificCanHandle(selectedObject);
         if (specificCanHandle.Item1)
            return specificCanHandle;

         return base.CanHandle(selectedObject);
      }

      protected override void EnsureItemsVisibility(IObjectBase parentObject, IParameter parameter = null)
      {
         ShowView(_editPresenter.BaseView);
         _editPresenter.Edit(parentObject);
         _editPresenter.SelectParameter(parameter);
      }

      protected override void SelectBuilder(IContainer builder)
      {
         if (builder.IsAnImplementationOf<IDistributedParameter>())
         {
            EnsureItemsVisibility(builder.ParentContainer, (IDistributedParameter) builder);
         }
         else
         {
            _view.SetEditView(_editPresenter.BaseView);
            _editPresenter.Edit(builder);
         }
      }

      private bool shouldHandleSelection(IEntity entity)
      {
         return entity.IsAnImplementationOf<IContainer>() &&
                !entity.IsAnImplementationOf<IDistributedParameter>() &&
                IsInSubject((IContainer) entity);
      }

      public bool IsInSubject(IContainer container)
      {
         return _spatialStructure.IsInSpatialStructure(container);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.SpatialStructureBuildingBlockCaption(_spatialStructure.Name);
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

      private bool shouldHandleRemoved(IObjectBase objectBase)
      {
         return Equals(objectBase, _editPresenter.Subject);
      }

      public void LoadDiagram()
      {
         if (_diagramLoaded) return;
         _heavyWorkManager.Start(() => _spatialStructureDiagramPresenter.Edit(_spatialStructure), AppConstants.Captions.LoadingDiagram);
         _diagramLoaded = true;
      }

      protected override void ShowView(IView viewToShow) => _view.SetEditView(viewToShow);
   }
}