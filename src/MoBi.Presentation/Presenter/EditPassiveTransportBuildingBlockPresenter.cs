using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
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

namespace MoBi.Presentation.Presenter
{
   public interface IEditPassiveTransportBuildingBlockPresenter : ISingleStartPresenter<IPassiveTransportBuildingBlock>,
      IPresenterWithContextMenu<IViewItem>,
      IListener<RemovedEvent>, IListener<AddedEvent>, IListener<EntitySelectedEvent>

   {
      IEnumerable<FormulaBuilderDTO> GetFormulas();
      void Select(TransportBuilderDTO transportBuilder);
   }

   public class EditPassiveTransportBuildingBlockPresenter : EditBuildingBlockPresenterBase<IEditPassiveTransportBuildingBlockView, IEditPassiveTransportBuildingBlockPresenter, IPassiveTransportBuildingBlock, ITransportBuilder>, IEditPassiveTransportBuildingBlockPresenter
   {
      private readonly ITransportBuilderToTransportBuilderDTOMapper _transportBuilderToDTOTransportBuilderMapper;
      private readonly IEditTransportBuilderPresenter _editTransportBuilderPresenter;
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IFormulaToFormulaBuilderDTOMapper _formulaToFormulaDTOBuilderMapper;
      private IPassiveTransportBuildingBlock _passiveTransports;

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

      public override void Edit(IPassiveTransportBuildingBlock passiveTransports)
      {
         _passiveTransports = passiveTransports;
         _editTransportBuilderPresenter.BuildingBlock = _passiveTransports;
         refresh(passiveTransports);
         _view.Display();
      }

      private void refresh(IPassiveTransportBuildingBlock passiveTransports)
      {
         EditFormulas(_passiveTransports);
         _view.Show(passiveTransports.MapAllUsing(_transportBuilderToDTOTransportBuilderMapper));
         editChild(passiveTransports.FirstOrDefault());
         UpdateCaption();
      }

      private void editChild(ITransportBuilder passiveTransports)
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
         _view.Caption = AppConstants.Captions.PassiveTransportCaption(_passiveTransports.Name);
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

      private ITransportBuilder builderFrom(TransportBuilderDTO transportBuilder)
      {
         if (transportBuilder == null)
            return null;
         return _passiveTransports.FindById(transportBuilder.Id);
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_passiveTransports == null) return;
         if (eventToHandle.RemovedObjects.Any(removedObject => removedObject.IsAnImplementationOf<ITransportBuilder>()))
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
         editChild(addedObject as ITransportBuilder);
      }

      private bool shouldHandleAdd(IObjectBase addedObject, IObjectBase parent)
      {
         return addedObject.IsAnImplementationOf<ITransportBuilder>() && parent.Equals(_passiveTransports);
      }

      private class RootViewItemPassiveTransport : IRootViewItem<ITransportBuilder>
      {
      }

      protected override Tuple<bool, IObjectBase> SpecificCanHandle(IObjectBase selectedObject)
      {
         var transportBuilder = selectedObject as ITransportBuilder;
         if (transportBuilder != null)
            return new Tuple<bool, IObjectBase>(_passiveTransports.Contains(transportBuilder), transportBuilder);

         return new Tuple<bool, IObjectBase>(false, selectedObject);
      }

      protected override void SelectBuilder(ITransportBuilder builder)
      {
         editChild(builder);
      }

      protected override void EnsureItemsVisibility(IObjectBase parentObject, IParameter parameter = null)
      {
         SelectBuilder(parentObject as ITransportBuilder);
         _editTransportBuilderPresenter.SelectParameter(parameter);
      }
   }
}