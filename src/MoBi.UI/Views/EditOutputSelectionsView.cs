using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Core.Domain;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Exceptions;
using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using static OSPSuite.UI.UIConstants;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.DataBinding;
using static OSPSuite.UI.UIConstants.Size;

namespace MoBi.UI.Views
{
   public partial class EditOutputSelectionsView : BaseUserControl, IEditOutputSelectionsView
   {
      private readonly GridViewBinder<QuantitySelection> _gridViewBinder;
      private readonly RepositoryItemButtonEdit _buttonRepository = new UxAddRemoveAndEditButtonRepository();
      private IEditOutputSelectionsPresenter _presenter;

      public EditOutputSelectionsView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<QuantitySelection>(gridView);
         gridView.AllowsFiltering = false;
      }

      public void AttachPresenter(IEditOutputSelectionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.Path).WithOnValueUpdating((o,e) => OnEvent(() => updateOutputSelectionPath(e)));

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(x => _buttonRepository)
            .WithFixedWidth(EMBEDDED_BUTTON_WIDTH * 3);

         _buttonRepository.ButtonClick += (o, e) => OnEvent(() => onButtonClick(e));
      }

      private void updateOutputSelectionPath(PropertyValueSetEventArgs<string> e)
      {
         _presenter.UpdateOutputSelection(_gridViewBinder.FocusedElement, e.NewValue);
         _gridViewBinder.Rebind();
      }

      private void onButtonClick(ButtonPressedEventArgs eventArgs)
      {
         switch (eventArgs.Button.Kind)
         {
            case ButtonPredefines.Delete:
               _presenter.RemoveOutputSelection(_gridViewBinder.FocusedElement);
               break;
            case ButtonPredefines.Plus:
               _presenter.AddOutputSelection(_gridViewBinder.FocusedElement);
               break;
            case ButtonPredefines.Ellipsis:
               _presenter.EditOutputSelection(_gridViewBinder.FocusedElement);
               break;
            default:
               throw new OSPSuiteException("No action associated with that button type");
         }
         _gridViewBinder.Rebind();
      }

      public void BindTo(IEnumerable<QuantitySelection> allOutputs)
      {
         _gridViewBinder.BindToSource(allOutputs);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.OutputSelections;
      }
   }
}
