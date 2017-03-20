using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter.SpaceDiagram;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditSpatialStructurePresenter : ISingleStartPresenter<IMoBiSpatialStructure>,
      IDiagramBuildingBlockPresenter,
      IListener<EntitySelectedEvent>,
      IListener<RemovedEvent>,
      IListener<FavoritesSelectedEvent>
   {
      void LoadDiagram();
   }

   public class EditSpatialStructurePresenter : EditBuildingBlockPresenterBase<IEditSpatialStructureView, IEditSpatialStructurePresenter, IMoBiSpatialStructure, IContainer>,
      IEditSpatialStructurePresenter
   {
      private IMoBiSpatialStructure _spatialStructure;
      private readonly IHierarchicalSpatialStructurePresenter _hierarchicalSpatialStructurePresenter;
      private readonly ISpatialStructureDiagramPresenter _spatialStructureDiagramPresenter;
      private readonly IEditContainerPresenter _editPresenter;
      private bool _diagramLoaded;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IEditFavoritesInSpatialStructurePresenter _favoritesPresenter;

      public EditSpatialStructurePresenter(IEditSpatialStructureView view,
         IHierarchicalSpatialStructurePresenter hierarchicalSpatialStructurePresenter,
         IFormulaCachePresenter formulaCachePresenter, IEditContainerPresenter editPresenter,
         ISpatialStructureDiagramPresenter spatialStructureDiagramPresenter, IHeavyWorkManager heavyWorkManager,
         IEditFavoritesInSpatialStructurePresenter favoritesPresenter) :
            base(view, formulaCachePresenter)
      {
         _hierarchicalSpatialStructurePresenter = hierarchicalSpatialStructurePresenter;
         _spatialStructureDiagramPresenter = spatialStructureDiagramPresenter;
         _heavyWorkManager = heavyWorkManager;
         _favoritesPresenter = favoritesPresenter;
         _favoritesPresenter.ShouldHandleRemovedEvent = shouldHandleRemoved;
         _editPresenter = editPresenter;
         _view.SetEditView(_editPresenter.BaseView);
         _view.SetHierarchicalStructureView(_hierarchicalSpatialStructurePresenter.BaseView);
         _view.SetSpaceDiagramView(spatialStructureDiagramPresenter.View);
         AddSubPresenters(editPresenter, hierarchicalSpatialStructurePresenter, spatialStructureDiagramPresenter, _favoritesPresenter);
      }

      public override void Edit(IMoBiSpatialStructure spatialStructure)
      {
         _diagramLoaded = (_spatialStructure == spatialStructure) && _diagramLoaded;
         _spatialStructure = spatialStructure;
         EditFormulas(spatialStructure);
         _editPresenter.BuildingBlock = _spatialStructure;
         _favoritesPresenter.BuildingBlock = _spatialStructure;
         _hierarchicalSpatialStructurePresenter.Edit(spatialStructure);
         _view.SetEditView(_favoritesPresenter.BaseView);
         _favoritesPresenter.Edit(spatialStructure);
         setInitalView();
         UpdateCaption();
         _view.Display();
      }

      private void setInitalView()
      {
         _view.SetEditView(_favoritesPresenter.BaseView);
      }

      public override object Subject
      {
         get { return _spatialStructure; }
      }

      protected override Tuple<bool, IObjectBase> SpecificCanHandle(IObjectBase selectedObject)
      {
         return new Tuple<bool, IObjectBase>(shoudHandleSelection(selectedObject as IEntity), selectedObject);
      }

      internal override Tuple<bool, IObjectBase> CanHandle(IObjectBase selectedObject)
      {
         var specificCanHandle = SpecificCanHandle(selectedObject);
         if (specificCanHandle.Item1)
            return specificCanHandle;
         return base.CanHandle(selectedObject);
      }

      protected override void EnsureItemsVisibility(IObjectBase parentObject, IParameter parameter = null)
      {
         _view.SetEditView(_editPresenter.BaseView);
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

      private bool shoudHandleSelection(IEntity entity)
      {
         return entity.IsAnImplementationOf<IContainer>() && !entity.IsAnImplementationOf<IDistributedParameter>() &&
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
         if (eventToHandle.RemovedObjects.Any(shouldHandleRemoved))
         {
            setInitalView();
         }
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

      public void Handle(FavoritesSelectedEvent eventToHandle)
      {
         if (Equals(eventToHandle.ObjectBase, _spatialStructure))
            _view.SetEditView(_favoritesPresenter.BaseView);
      }
   }
}