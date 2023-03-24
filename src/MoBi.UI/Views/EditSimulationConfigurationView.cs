﻿using System.Windows.Forms;
using DevExpress.XtraLayout;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditSimulationConfigurationView : BaseUserControl, IEditSimulationConfigurationView
   {
      private IEditSimulationConfigurationPresenter _presenter;

      public EditSimulationConfigurationView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         layoutControl.Images = imageListRetriever.AllImages16x16;
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

      public void AttachPresenter(IEditSimulationConfigurationPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.SimulationConfiguration;
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.Simulation; }
      }
   }
}