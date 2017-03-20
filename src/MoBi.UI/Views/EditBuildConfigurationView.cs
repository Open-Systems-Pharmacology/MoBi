using System.Windows.Forms;
using OSPSuite.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraLayout;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class EditBuildConfigurationView : BaseUserControl, IEditBuildConfigurationView
   {
      public EditBuildConfigurationView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         layoutControl.Images = imageListRetriever.AllImages16x16;
      }

      protected override int TopicId => HelpId.MoBi_SettingUpSimulation;

      public void AttachPresenter(IEditBuildConfigurationPresenter presenter)
      {
      }

      public void AddSelectionView(IResizableView view, string caption, ApplicationIcon icon)
      {
         var group = layoutMainGroup.AddGroup();
         var layoutControlItem = group.AddItem();
         layoutControlItem.Control = view.DowncastTo<Control>();
         layoutControlItem.TextVisible = false;
         layoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         group.Text = caption;
         group.CaptionImageIndex = ApplicationIcons.IconIndex(icon);
         view.HeightChanged += (o, e) => adjustLayoutItemSize(layoutControlItem, view, e.Height);
      }

      private void adjustLayoutItemSize(LayoutControlItem layoutControlItem, IResizableView view, int height)
      {
         layoutControlItem.AdjustControlHeight(height);
         view.Repaint();
      }

      public void AddEmptyPlaceHolder()
      {
         var dummyGroup = layoutMainGroup.AddGroup();
         dummyGroup.Add(new EmptySpaceItem());
         dummyGroup.TextVisible = false;
         dummyGroup.GroupBordersVisible = false;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.BuildConfiguration;
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.Simulation; }
      }
   }
}