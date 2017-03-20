using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditTableFormulaWithOffSetFormulaView : BaseUserControl, IEditTableFormulaWithOffSetFormulaView
   {
      private IEditTableFormulaWithOffSetFormulaPresenter _presenter;
      private TableFormulaWithOffsetDTO _dto;
      public bool ReadOnly { get; set; }

      public EditTableFormulaWithOffSetFormulaView()
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

      public void AttachPresenter(IEditTableFormulaWithOffSetFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btEditOffsetObjectPath.ButtonClick += (o, e) => OnEvent(btEditOffsetObjectPathOnButtonClick);
         btEditTableObjectPath.ButtonClick += (o, e) => OnEvent(btEditTableObjectPathOnButtonClick);
      }

      private void btEditOffsetObjectPathOnButtonClick()
      {
         _presenter.SetOffsetFormulaPath(_dto.OffsetObjectPath);
      }

      private void btEditTableObjectPathOnButtonClick()
      {
         _presenter.SetTableObjectPath(_dto.OffsetObjectPath);
      }

      public void Show(TableFormulaWithOffsetDTO dtoTableFormulaWithOffset)
      {
         ShowOffsetObjectPath(dtoTableFormulaWithOffset.OffsetObjectPath);
         ShowTableObjectPath(dtoTableFormulaWithOffset.TableObjectPath);
         _dto = dtoTableFormulaWithOffset;
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_ParametersTableFormulaOffset;

      public void ShowOffsetObjectPath(FormulaUsablePathDTO dtoFormulaUsablePath)
      {
         if (dtoFormulaUsablePath != null) btEditOffsetObjectPath.Text = dtoFormulaUsablePath.Path;
      }

      public void ShowTableObjectPath(FormulaUsablePathDTO dtoFormulaUsablePath)
      {
         if (dtoFormulaUsablePath != null) btEditTableObjectPath.Text = dtoFormulaUsablePath.Path;
      }
   }
}