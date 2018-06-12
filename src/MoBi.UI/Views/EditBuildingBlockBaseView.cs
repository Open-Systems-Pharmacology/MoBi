using OSPSuite.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditBuildingBlockBaseView : BaseMdiChildView, IEditBuildingBlockBaseView
   {
      public EditBuildingBlockBaseView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void SetFormulaCacheView(IView view)
      {
         tabFormulaCache.FillWith(view);
      }

      public string EditCaption
      {
         set => tabEditBuildingBlock.Text = value;
      }

      public ApplicationIcon EditIcon
      {
         set => tabEditBuildingBlock.Image = value.ToImage(IconSizes.Size16x16);
      }

      public void ShowFormulas()
      {
         tabPagesControl.SelectedTabPage = tabFormulaCache;
      }

      public void ShowDefault()
      {
         tabPagesControl.SelectedTabPage = tabEditBuildingBlock;
      }

      public override void SaveChanges()
      {
         base.SaveChanges();
         if (ActiveControl != null)
            SelectNextControl(ActiveControl, forward: true, tabStopOnly: true, nested: true, wrap: true);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         EditIcon = ApplicationIcon;
         tabFormulaCache.Image = ApplicationIcons.Formula.ToImage(IconSizes.Size16x16);
      }
   }
}