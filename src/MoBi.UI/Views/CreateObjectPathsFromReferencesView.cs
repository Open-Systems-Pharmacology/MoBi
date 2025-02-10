using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.XtraEditors;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class CreateObjectPathsFromReferencesView : BaseUserControl, ICreateObjectPathsFromReferencesView
   {
      private ICreateObjectPathsFromReferencesPresenter _presenter;

      public CreateObjectPathsFromReferencesView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemBtnAdd.AsAddButton();
         memoEditObjectPaths.Properties.WordWrap = false;
         btnAdd.Click += (sender, args) => _presenter.AddSelection();

         pathsToBeAddedGroup.Text = AppConstants.Captions.ParameterPathsToAdd;
         lblDescription.AutoSizeMode = LabelAutoSizeMode.Vertical;
         lblDescription.Text = AppConstants.Captions.AddAndEditPathsDescription;
         pathsToBeAddedGroup.Size = new Size(layoutControlItem4.Width, pathsToBeAddedGroup.Height);
      }

      protected override void OnLoad(EventArgs e)
      {
         base.OnLoad(e);
         memoEditObjectPaths.TextChanged += (o, ev) => OnEvent(memoEditObjectPaths.AutoScrollBars);
         memoEditObjectPaths.SizeChanged += (o, ev) => OnEvent(memoEditObjectPaths.AutoScrollBars);
      }

      public void AttachPresenter(ICreateObjectPathsFromReferencesPresenter presenter) => _presenter = presenter;

      public void AddReferenceSelectionView(IView view) => referenceSelectionPanel.FillWith(view);

      public IReadOnlyList<string> AllPaths => memoEditObjectPaths.Lines;
      public Size? ModalSize => UIConstants.UI.PARAMETER_SELECTION_SIZE;

      public void CanAdd(bool canAdd) => btnAdd.Enabled = canAdd;

      public void AddSelectedPaths(IReadOnlyList<string> pathsToAdd)
      {
         var allLines = pathsToAdd.Where(x => !memoEditObjectPaths.Lines.Contains(x));

         if (!string.IsNullOrEmpty(memoEditObjectPaths.Text))
            allLines = allLines.Prepend(memoEditObjectPaths.Text);

         memoEditObjectPaths.Text = string.Join(Environment.NewLine, allLines);
      }
   }
}