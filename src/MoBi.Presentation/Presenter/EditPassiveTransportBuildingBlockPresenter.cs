using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IEditPassiveTransportBuildingBlockPresenter : ISingleStartPresenter<PassiveTransportBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<RemovedEvent>, IListener<AddedEvent>, IListener<EntitySelectedEvent>

   {
      IEnumerable<FormulaBuilderDTO> GetFormulas();
      void Select(TransportBuilderDTO transportBuilder);
   }

   public class EditPassiveTransportBuildingBlockPresenter : EditBuildingBlockPresenterBase<IEditPassiveTransportBuildingBlockView, IEditPassiveTransportBuildingBlockPresenter, PassiveTransportBuildingBlock, TransportBuilder>, IEditPassiveTransportBuildingBlockPresenter
   {
      private readonly ITransportBuilderToTransportBuilderDTOMapper _transportBuilderToDTOTransportBuilderMapper;
      private readonly IEditTransportBuilderPresenter _editTransportBuilderPresenter;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToFormulaDTOBuilderMapper;
      private PassiveTransportBuildingBlock _passiveTransports;

      public EditPassiveTransportBuildingBlockPresenter(IEditPassiveTransportBuildingBlockView view,
         ITransportBuilderToTransportBuilderDTOMapper transportBuilderToDTOTransportBuilderMapper,
         IEditTransportBuilderPresenter editTransportBuilderPresenter, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IFormulaToFormulaBuilderDTOMapper formulaToFormulaDTOBuilderMapper, IFormulaCachePresenter formulaCachePresenter)
         : base(view, formulaCachePresenter)
      {
         _formulaToFormulaDTOBuilderMapper = formulaToFormulaDTOBuilderMapper;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _editTransportBuilderPresenter = editTransportBuilderPresenter;
         _transportBuilderToDTOTransportBuilderMapper = transportBuilderToDTOTransportBuilderMapper;
         AddSubPresenters(_editTransportBuilderPresenter);
      }

      public override void Edit(PassiveTransportBuildingBlock passiveTransports)
      {
         _passiveTransports = passiveTransports;
         _editTransportBuilderPresenter.BuildingBlock = _passiveTransports;
         refresh(passiveTransports);
         _view.Display();
      }

      private void refresh(PassiveTransportBuildingBlock passiveTransports)
      {
         EditFormulas(_passiveTransports);
         _view.Show(passiveTransports.MapAllUsing(_transportBuilderToDTOTransportBuilderMapper));
         editChild(passiveTransports.FirstOrDefault());
         UpdateCaption();
      }

      private void editChild(TransportBuilder passiveTransports)
      {
         if (passiveTransports == null)
         {
            _view.ClearEditView();
            return;
         }

         _view.SetEditView(_editTransportBuilderPresenter.BaseView);
         _editTransportBuilderPresenter.Edit(passiveTransports);
      }

      public override object Subject => _passiveTransports;

      protected override void UpdateCaption()
      {
         _view.Caption = AppConstants.Captions.PassiveTransportCaption(_passiveTransports.DisplayName);
      }

      public void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var viewItem = objectRequestingPopup ?? new RootViewItemPassiveTransport();
         var contextMenu = _viewItemContextMenuFactory.CreateFor(viewItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Select(TransportBuilderDTO transportBuilder)
      {
         editChild(builderFrom(transportBuilder));
      }

      public IEnumerable<FormulaBuilderDTO> GetFormulas()
      {
         return FormulaCache.MapAllUsing(_formulaToFormulaDTOBuilderMapper);
      }

      public IFormulaCache FormulaCache => BuildingBlock.FormulaCache;

      private TransportBuilder builderFrom(TransportBuilderDTO transportBuilder)
      {
         if (transportBuilder == null)
            return null;
         return _passiveTransports.FindById(transportBuilder.Id);
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_passiveTransports == null) return;
         if (eventToHandle.RemovedObjects.Any(removedObject => removedObject.IsAnImplementationOf<TransportBuilder>()))
         {
            refresh(_passiveTransports);
         }
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_passiveTransports == null) return;
         var addedObject = eventToHandle.AddedObject;
         if (!shouldHandleAdd(addedObject, eventToHandle.Parent))
            return;

         refresh(_passiveTransports);
         editChild(addedObject as TransportBuilder);
      }

      private bool shouldHandleAdd(IObjectBase addedObject, IObjectBase parent)
      {
         return addedObject.IsAnImplementationOf<TransportBuilder>() && parent.Equals(_passiveTransports);
      }

      private class RootViewItemPassiveTransport : IRootViewItem<TransportBuilder>
      {
      }

      protected override (bool canHandle, IContainer parentObject) SpecificCanHandle(IObjectBase selectedObject)
      {
         if (selectedObject is TransportBuilder transportBuilder)
            return (_passiveTransports.Contains(transportBuilder), transportBuilder);

         return (false, null);
      }

      protected override void SelectBuilder(TransportBuilder builder)
      {
         editChild(builder);
      }

      protected override void EnsureItemsVisibility(IContainer parentObject, IParameter parameter = null)
      {
         SelectBuilder(parentObject as TransportBuilder);
         _editTransportBuilderPresenter.SelectParameter(parameter);
      }
   }
}