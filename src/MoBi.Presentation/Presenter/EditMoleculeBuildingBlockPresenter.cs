using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditMoleculeBuildingBlockPresenter : ISingleStartPresenter<IMoleculeBuildingBlock>,
      IListener<EntitySelectedEvent>, IListener<RemovedEvent>, IListener<BulkUpdateFinishedEvent>,
      IListener<BulkUpdateStartedEvent>, IListener<FavoritesSelectedEvent>
   {
   }

   public class EditMoleculeBuildingBlockPresenter :
      EditBuildingBlockPresenterBase
         <IEditMoleculesBuildingBlockView, IEditMoleculeBuildingBlockPresenter, IMoleculeBuildingBlock, IMoleculeBuilder
            >, IEditMoleculeBuildingBlockPresenter
   {
      private readonly IMoleculeListPresenter _moleculeListPresenter;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private readonly IEditMoleculeBuilderPresenter _editMoleculeBuilderPresenter;
      private readonly IEditTransporterMoleculeContainerPresenter _editTransporterMoleculeContainerPresenter;
      private readonly IEditTransportBuilderPresenter _editTransportBuilderPresenter;
      private bool _disableEventsForHeavyWork;
      private readonly IEditContainerPresenter _editInteractionContainerPresenter;
      private readonly IEditFavoritesInMoleculesPresenter _editFavoritesPresenter;

      public EditMoleculeBuildingBlockPresenter(IEditMoleculesBuildingBlockView view,
         IMoleculeListPresenter moleculeListPresenter, IFormulaCachePresenter formulaCachePresenter,
         IEditMoleculeBuilderPresenter editMoleculeBuilderPresenter,
         IEditTransporterMoleculeContainerPresenter editTransporterMoleculeContainerPresenter,
         IEditTransportBuilderPresenter editTransportBuilderPresenter,
         IEditContainerPresenter editInteractionContainerPresenter,
         IEditFavoritesInMoleculesPresenter editFavoritesPresenter)
         : base(view, formulaCachePresenter)
      {
         _editTransportBuilderPresenter = editTransportBuilderPresenter;
         _editTransporterMoleculeContainerPresenter = editTransporterMoleculeContainerPresenter;
         _editMoleculeBuilderPresenter = editMoleculeBuilderPresenter;
         _moleculeListPresenter = moleculeListPresenter;
         _editInteractionContainerPresenter = editInteractionContainerPresenter;
         _editFavoritesPresenter = editFavoritesPresenter;
         _editFavoritesPresenter.ShouldHandleRemovedEvent = shouldHandleType;
         _view.SetListView(_moleculeListPresenter.BaseView);
         AddSubPresenters(_editTransportBuilderPresenter, _editTransporterMoleculeContainerPresenter,
            _editMoleculeBuilderPresenter, _moleculeListPresenter, _editInteractionContainerPresenter, _editFavoritesPresenter);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.MoleculesBuildingBlockCaption(_moleculeBuildingBlock.Name);
      }

      public override object Subject
      {
         get { return _moleculeBuildingBlock; }
      }

      public override void Edit(IMoleculeBuildingBlock moleculeBuildingBlockToEdit)
      {
         _moleculeBuildingBlock = moleculeBuildingBlockToEdit;
         _moleculeListPresenter.Edit(moleculeBuildingBlockToEdit);
         _editTransportBuilderPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editTransporterMoleculeContainerPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editInteractionContainerPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editMoleculeBuilderPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editFavoritesPresenter.Edit(moleculeBuildingBlockToEdit);
         EditFormulas(moleculeBuildingBlockToEdit);
         _view.SetEditView(_editFavoritesPresenter.BaseView);
         UpdateCaption();
         _view.Display();
      }

      private void refresh(IMoleculeBuildingBlock moleculeBuildingBlockToEdit)
      {
         setUpEditPresenterFor(moleculeBuildingBlockToEdit.FirstOrDefault());
         UpdateCaption();
      }

      private void setUpEditPresenterFor(IObjectBase objectBase)
      {
         setUpEditPresenterFor(objectBase, null);
      }

      private void setUpEditPresenterFor(IObjectBase objectBase, IParameter parameter)
      {
         if (objectBase.IsAnImplementationOf<IMoleculeBuilder>())
            editPresenter(objectBase, _editMoleculeBuilderPresenter, parameter);

         else if (objectBase.IsAnImplementationOf<TransporterMoleculeContainer>())
            editPresenter(objectBase, _editTransporterMoleculeContainerPresenter, parameter);

         else if (objectBase.IsAnImplementationOf<ITransportBuilder>())
            editPresenter(objectBase, _editTransportBuilderPresenter, parameter);

         else if (objectBase.IsAnImplementationOf<InteractionContainer>())
            editPresenter(objectBase, _editInteractionContainerPresenter, parameter);
      }

      private void editPresenter<T>(IObjectBase objectBase, IEditPresenterWithParameters<T> editPresenter,
         IParameter parameter)
      {
         _view.SetEditView(editPresenter.BaseView);
         editPresenter.Edit(objectBase.DowncastTo<T>());
         if (parameter != null)
            editPresenter.SelectParameter(parameter);
      }

      protected override Tuple<bool, IObjectBase> SpecificCanHandle(IObjectBase selectedObject)
      {
         if (shouldHandleType(selectedObject))
            return new Tuple<bool, IObjectBase>(_moleculeBuildingBlock.ContainsBuilder(selectedObject), selectedObject);

         return new Tuple<bool, IObjectBase>(false, selectedObject);
      }

      protected override void EnsureItemsVisibility(IObjectBase parentObject, IParameter parameter = null)
      {
         setUpEditPresenterFor(parentObject, parameter);
      }

      protected override void SelectBuilder(IMoleculeBuilder builder)
      {
         setUpEditPresenterFor(builder);
      }

      private bool shouldHandleType(IObjectBase selectedEntity)
      {
         return selectedEntity.IsAnImplementationOf<IMoleculeBuilder>()
                || selectedEntity.IsAnImplementationOf<ITransportBuilder>()
                || selectedEntity.IsAnImplementationOf<TransporterMoleculeContainer>()
                || selectedEntity.IsAnImplementationOf<InteractionContainer>();
      }

      public void Handle(BulkUpdateFinishedEvent eventToHandle)
      {
         _disableEventsForHeavyWork = false;
         refresh(_moleculeBuildingBlock);
      }

      public void Handle(BulkUpdateStartedEvent eventToHandle)
      {
         _disableEventsForHeavyWork = true;
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_disableEventsForHeavyWork)
            return;
         if (eventToHandle.RemovedObjects.Any(shouldHandleType))
         {
            refresh(_moleculeBuildingBlock);
         }
      }

      public void Handle(FavoritesSelectedEvent eventToHandle)
      {
         if (eventToHandle.ObjectBase.Equals(_moleculeBuildingBlock))
            _view.SetEditView(_editFavoritesPresenter.BaseView);
      }
   }
}