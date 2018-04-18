using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditTableFormulaWithXArgumentFormulaView : BaseUserControl, IEditTableFormulaWithXArgumentFormulaView
   {
      private IEditTableFormulaWithXArgumentFormulaPresenter _presenter;
      public bool ReadOnly { get; set; }

      public EditTableFormulaWithXArgumentFormulaView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemXArgumentObjectPath.Text = AppConstants.Captions.XArgumentObjectPath.FormatForLabel();
         layoutControlItemTableObjectPath.Text = AppConstants.Captions.TableObjectPath.FormatForLabel();
         btEditXArgumentObjectPath.Properties.ReadOnly = true;
         btEditTableObjectPath.Properties.ReadOnly = true;
      }

      public void AttachPresenter(IEditTableFormulaWithXArgumentFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btEditXArgumentObjectPath.ButtonClick += (o, e) => OnEvent(_presenter.SetXArgumentFormulaPath);
         btEditTableObjectPath.ButtonClick += (o, e) => OnEvent(_presenter.SetTableObjectPath);
      }

      public void BindTo(TableFormulaWithXArgumentDTO tableFormulaWithXArgumentDTO)
      {
         showXArgumentObjectPath(tableFormulaWithXArgumentDTO.XArgumentObjectPath);
         showTableObjectPath(tableFormulaWithXArgumentDTO.TableObjectPath);
      }

      private void showXArgumentObjectPath(FormulaUsablePathDTO forrmulaUsablePathDTO)
      {
         if (forrmulaUsablePathDTO != null)
            btEditXArgumentObjectPath.Text = forrmulaUsablePathDTO.Path;
      }

      private void showTableObjectPath(FormulaUsablePathDTO forrmulaUsablePathDTO)
      {
         if (forrmulaUsablePathDTO != null)
            btEditTableObjectPath.Text = forrmulaUsablePathDTO.Path;
      }
   }
}