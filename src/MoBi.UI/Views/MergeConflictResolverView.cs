using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class MergeConflictResolverView : BaseView, IMergeConflictResolverView
   {
      private IMergeConflictResolverPresenter _presenter;
      private int _remainingConflicts;
      private readonly Form _owner;

      public MergeConflictResolverView(IShell owner)
      {
         _owner = owner as Form;
         InitializeComponent();
      }

      public void AttachPresenter(IMergeConflictResolverPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnKeepMerge.Click += (o, e) => OnEvent(replace);
         btnKeepTarget.Click += (o, e) => OnEvent(skip);
         btnCancel.Click += (o, e) => OnEvent(cancel);
         btnClone.Click += (o, e) => OnEvent(clone);
         btnMerge.Click += (o, e) => OnEvent(merge);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnCancel.Text = AppConstants.Captions.Cancel;
         btnKeepTarget.Text = AppConstants.Captions.KeepExisting;
         btnKeepMerge.Text = AppConstants.Captions.ReplaceWithMerged;
         btnClone.Text = AppConstants.Captions.AddWithNewName;
         btnMerge.Text = AppConstants.Captions.Merge;
         Caption = AppConstants.Captions.ResolveMergeConflict;
         grpMerge.Text = AppConstants.Captions.MergeEntity;
         grpTarget.Text = AppConstants.Captions.TargetEntity;
         var layoutVisibility = LayoutVisibilityConvertor.FromBoolean(false);

         layoutControlItemCloneButton.Visibility = layoutVisibility;
         layoutControlItemCloneCheckBox.Visibility = layoutVisibility;
         layoutControlItemMergeButton.Visibility = layoutVisibility;

         _applyDefaultRenaming.Text = AppConstants.Dialog.ApplyDefaultNaming;
      }

      private void merge()
      {
         _presenter.SetConflictResolution(_chkApplyToRemaning.CheckState == CheckState.Checked ? MergeConflictOptions.MergeAll : MergeConflictOptions.MergeOnce);
         Close();
      }

      private void clone()
      {
         var option = _chkApplyToRemaning.CheckState == CheckState.Checked ? MergeConflictOptions.CloneAll : MergeConflictOptions.CloneOnce;
         if (_applyDefaultRenaming.CheckState == CheckState.Checked)
            option |= MergeConflictOptions.AutoRename;

         _presenter.SetConflictResolution(option);
      }

      private void cancel()
      {
         _presenter.SetConflictResolution(MergeConflictOptions.Cancel);
      }

      private void skip()
      {
         _presenter.SetConflictResolution(_chkApplyToRemaning.CheckState == CheckState.Checked ? MergeConflictOptions.SkipAll : MergeConflictOptions.SkipOnce);
      }

      private void replace()
      {
         _presenter.SetConflictResolution(_chkApplyToRemaning.CheckState == CheckState.Checked ? MergeConflictOptions.ReplaceAll : MergeConflictOptions.ReplaceOnce);
      }

      public void Display()
      {
         _chkApplyToRemaning.CheckState = CheckState.Unchecked;
         if (_remainingConflicts != 0)
         {
            _chkApplyToRemaning.Text = AppConstants.Captions.ApplyToRemaining(_remainingConflicts);
            checkEditLayoutControl.Visibility = LayoutVisibility.Always;
         }
         else
            checkEditLayoutControl.Visibility = LayoutVisibility.Never;

         ShowDialog(_owner);
      }

      public bool CloneButtonEnabled
      {
         get { return layoutControlItemCloneButton.Visible; }
         set
         {
            layoutControlItemCloneButton.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutControlItemCloneCheckBox.Visibility = layoutControlItemCloneButton.Visibility;
         }
      }

      public bool MergeOptionEnabled
      {
         get { return layoutControlItemMergeButton.Visible; }
         set { layoutControlItemMergeButton.Visibility = LayoutVisibilityConvertor.FromBoolean(value); }
      }

      public void EnableMergeOption()
      {
         layoutControlItemMergeButton.Visibility = LayoutVisibilityConvertor.FromBoolean(true);
      }

      public void AttachSummaryViews(IView mergeView, IView targetView, int i)
      {
         _targetPanel.FillWith(targetView);
         _mergePanel.FillWith(mergeView);
         _remainingConflicts = i;
      }

      public void SetFormLayout(Point location, Size size)
      {
         var desiredFormArea = new Rectangle(location, size);
         this.FitToScreen(desiredFormArea, getClosestWorkingArea(desiredFormArea));
      }

      protected override void OnFormClosing(FormClosingEventArgs e)
      {
         base.OnFormClosing(e);
         _presenter.FormClosing(Location, Size);
      }

      private static Rectangle getClosestWorkingArea(Rectangle desiredFormArea)
      {
         return Screen.GetWorkingArea(desiredFormArea);
      }

      public void CloseView()
      {
         Close();
      }

      public bool Canceled { get; private set; }
      public bool OkEnabled { get; set; }
      public bool ExtraEnabled { get; set; }
      public bool ExtraVisible { get; set; }
      public bool CancelVisible { get; set; }
   }
}