using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditTableFormulaWithOffsetFormulaView : BaseUserControl, IEditTableFormulaWithOffsetFormulaView
   {
      private IEditTableFormulaWithOffsetFormulaPresenter _presenter;
      public bool ReadOnly { get; set; }

      public EditTableFormulaWithOffsetFormulaView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemOffsetObjectPath.Text = AppConstants.Captions.OffsetObjectPath.FormatForLabel();
         layoutControlItemTableObjectPath.Text = AppConstants.Captions.TableObjectPath.FormatForLabel();
         btEditOffsetObjectPath.Properties.ReadOnly = true;
         btEditTableObjectPath.Properties.ReadOnly = true;
      }

      public void AttachPresenter(IEditTableFormulaWithOffsetFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btEditOffsetObjectPath.ButtonClick += (o, e) => OnEvent(_presenter.SetOffsetFormulaPath);
         btEditTableObjectPath.ButtonClick += (o, e) => OnEvent(_presenter.SetTableObjectPath);
      }

      public void BindTo(TableFormulaWithOffsetDTO tableFormulaWithOffsetDTO)
      {
         showOffsetObjectPath(tableFormulaWithOffsetDTO.OffsetObjectPath);
         showTableObjectPath(tableFormulaWithOffsetDTO.TableObjectPath);
      }

      private void showOffsetObjectPath(FormulaUsablePathDTO forrmulaUsablePathDTO)
      {
         if (forrmulaUsablePathDTO != null)
            btEditOffsetObjectPath.Text = forrmulaUsablePathDTO.Path;
      }

      private void showTableObjectPath(FormulaUsablePathDTO forrmulaUsablePathDTO)
      {
         if (forrmulaUsablePathDTO != null)
            btEditTableObjectPath.Text = forrmulaUsablePathDTO.Path;
      }
   }
}