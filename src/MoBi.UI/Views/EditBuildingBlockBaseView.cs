using OSPSuite.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditBuildingBlockBaseView : BaseMdiChildView, IEditBuildingBlockBaseView
   {
      //only for design time
      public EditBuildingBlockBaseView() : this(null)
      {
      }

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
         set
         {
            //Required because this might be set before the component is actually fully initialized
            if(tabEditBuildingBlock != null)
               tabEditBuildingBlock.Image = value.ToImage(IconSizes.Size16x16);
         }
      }

      public void ShowFormulas()
      {
         tabPagesControl.SelectedTabPage = tabFormulaCache;
      }

      public void ShowDefault()
      {
         tabPagesControl.SelectedTabPage = tabEditBuildingBlock;
      }

      public override ApplicationIcon ApplicationIcon
      {
         set
         {
            base.ApplicationIcon = value;
            EditIcon = value;
         }
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
         tabFormulaCache.Image = ApplicationIcons.Formula.ToImage(IconSizes.Size16x16);
      }
   }
}