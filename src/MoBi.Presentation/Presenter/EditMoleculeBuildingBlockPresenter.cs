using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Events;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditMoleculeBuildingBlockPresenter :
      ISingleStartPresenter<IMoleculeBuildingBlock>,
      IListener<EntitySelectedEvent>,
      IListener<RemovedEvent>,
      IListener<BulkUpdateFinishedEvent>,
      IListener<BulkUpdateStartedEvent>,
      IListener<FavoritesSelectedEvent>,
      IListener<UserDefinedSelectedEvent>
   {
   }

   public class EditMoleculeBuildingBlockPresenter : EditBuildingBlockWithFavoriteAndUserDefinedPresenterBase<IEditMoleculesBuildingBlockView, IEditMoleculeBuildingBlockPresenter, IMoleculeBuildingBlock, IMoleculeBuilder>,
      IEditMoleculeBuildingBlockPresenter
   {
      private readonly IMoleculeListPresenter _moleculeListPresenter;
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private readonly IEditMoleculeBuilderPresenter _editMoleculeBuilderPresenter;
      private readonly IEditTransporterMoleculeContainerPresenter _editTransporterMoleculeContainerPresenter;
      private readonly IEditTransportBuilderPresenter _editTransportBuilderPresenter;
      private bool _disableEventsForHeavyWork;
      private readonly IEditContainerPresenter _editInteractionContainerPresenter;

      public EditMoleculeBuildingBlockPresenter(IEditMoleculesBuildingBlockView view,
         IMoleculeListPresenter moleculeListPresenter, IFormulaCachePresenter formulaCachePresenter,
         IEditMoleculeBuilderPresenter editMoleculeBuilderPresenter,
         IEditTransporterMoleculeContainerPresenter editTransporterMoleculeContainerPresenter,
         IEditTransportBuilderPresenter editTransportBuilderPresenter,
         IEditContainerPresenter editInteractionContainerPresenter,
         IEditFavoritesInMoleculesPresenter favoritesPresenter,
         IUserDefinedParametersPresenter userDefinedParametersPresenter
      )
         : base(view, formulaCachePresenter, favoritesPresenter, userDefinedParametersPresenter)
      {
         _editTransportBuilderPresenter = editTransportBuilderPresenter;
         _editTransporterMoleculeContainerPresenter = editTransporterMoleculeContainerPresenter;
         _editMoleculeBuilderPresenter = editMoleculeBuilderPresenter;
         _moleculeListPresenter = moleculeListPresenter;
         _editInteractionContainerPresenter = editInteractionContainerPresenter;
         _favoritesPresenter.ShouldHandleRemovedEvent = shouldHandleType;
         _view.SetListView(_moleculeListPresenter.BaseView);

         AddSubPresenters(_editTransportBuilderPresenter, _editTransporterMoleculeContainerPresenter,
            _editMoleculeBuilderPresenter, _moleculeListPresenter, _editInteractionContainerPresenter);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.MoleculesBuildingBlockCaption(_moleculeBuildingBlock.Name);
      }

      public override object Subject => _moleculeBuildingBlock;

      public override void Edit(IMoleculeBuildingBlock moleculeBuildingBlockToEdit)
      {
         _moleculeBuildingBlock = moleculeBuildingBlockToEdit;
         _moleculeListPresenter.Edit(moleculeBuildingBlockToEdit);
         _editTransportBuilderPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editTransporterMoleculeContainerPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editInteractionContainerPresenter.BuildingBlock = _moleculeBuildingBlock;
         _editMoleculeBuilderPresenter.BuildingBlock = _moleculeBuildingBlock;
         _favoritesPresenter.Edit(moleculeBuildingBlockToEdit);
         EditFormulas(moleculeBuildingBlockToEdit);
         _view.SetEditView(_favoritesPresenter.BaseView);
         UpdateCaption();
         _view.Display();
      }

      private void refresh(IMoleculeBuildingBlock moleculeBuildingBlockToEdit)
      {
         setupEditPresenterFor(moleculeBuildingBlockToEdit.FirstOrDefault());
         UpdateCaption();
      }

      private void setupEditPresenterFor(IObjectBase objectBase, IParameter parameter = null)
      {
         switch (objectBase)
         {
            case IMoleculeBuilder moleculeBuilder:
               editPresenter(moleculeBuilder, _editMoleculeBuilderPresenter, parameter);
               return;
            case TransporterMoleculeContainer transporterMoleculeContainer:
               editPresenter(transporterMoleculeContainer, _editTransporterMoleculeContainerPresenter, parameter);
               return;

            case ITransportBuilder transportBuilder:
               editPresenter(transportBuilder, _editTransportBuilderPresenter, parameter);
               return;

            case InteractionContainer interactionContainer:
               editPresenter(interactionContainer, _editInteractionContainerPresenter, parameter);
               return;
         }
      }

      private void editPresenter<T>(T objectBase, IEditPresenterWithParameters<T> editPresenter, IParameter parameter)
      {
         _view.SetEditView(editPresenter.BaseView);
         editPresenter.Edit(objectBase);
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
         setupEditPresenterFor(parentObject, parameter);
      }

      protected override void SelectBuilder(IMoleculeBuilder builder)
      {
         setupEditPresenterFor(builder);
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

      protected override void ShowView(IView viewToShow) => _view.SetEditView(viewToShow);

      protected override Action ColumnConfiguration(IUserDefinedParametersPresenter userDefinedParametersPresenter) => userDefinedParametersPresenter.ConfigureForMolecule;
   }
}