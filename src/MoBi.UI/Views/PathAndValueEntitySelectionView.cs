using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevExpress.Utils;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Extensions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class PathAndValueEntitySelectionView : BaseModalView, IPathAndValueEntitySelectionView
   {
      private IPathAndValueEntitySelectionPresenter _presenter;
      private readonly GridViewBinder<SelectableReplacePathAndValueDTO> _gridViewBinder;
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();

      public PathAndValueEntitySelectionView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<SelectableReplacePathAndValueDTO>(gridView);
         Load += (o, e) => OnEvent(formLoad);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         gridView.ConfigureGridForCheckBoxSelect(nameof(SelectableReplacePathAndValueDTO.Selected));
         gridLayoutControlItem.TextLocation = Locations.Top;
         gridLayoutControlItem.TextVisible = true;
      }

      private void formLoad()
      {
         gridControl.ForceInitialize();
         gridView.Appearance.SelectedRow.Assign(gridView.PaintAppearance.Row);
      }

      public void SetDescription(string description)
      {
         gridLayoutControlItem.AllowHtmlStringInCaption = true;
         gridLayoutControlItem.Text = description;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.AutoBind(dto => dto.Name).AsReadOnly();
         initializePathColumns();

         _gridViewBinder.Bind(dto => dto.OldValue)
            .WithCaption(nameof(SelectableReplacePathAndValueDTO.OldValue).SplitToUpperCase())
            .WithFormat(dto => dto.OldValueFormatter())
            .AsReadOnly();
         _gridViewBinder.Bind(dto => dto.NewValue)
            .WithFormat(dto => dto.NewValueFormatter())
            .WithCaption(nameof(SelectableReplacePathAndValueDTO.NewValue).SplitToUpperCase())
            .AsReadOnly();

         _gridViewBinder.Bind(dto => dto.OldFormula)
            .WithCaption(nameof(SelectableReplacePathAndValueDTO.OldFormula).SplitToUpperCase())
            .AsReadOnly();
         _gridViewBinder.Bind(dto => dto.NewFormula)
            .WithCaption(nameof(SelectableReplacePathAndValueDTO.NewFormula).SplitToUpperCase())
            .AsReadOnly();
      }

      private void initializePathColumns()
      {
         initializePathElementColumn(dto => dto.PathElement0, Captions.PathElement(0));
         initializePathElementColumn(dto => dto.PathElement1, Captions.PathElement(1));
         initializePathElementColumn(dto => dto.PathElement2, Captions.PathElement(2));
         initializePathElementColumn(dto => dto.PathElement3, Captions.PathElement(3));
         initializePathElementColumn(dto => dto.PathElement4, Captions.PathElement(4));
         initializePathElementColumn(dto => dto.PathElement5, Captions.PathElement(5));
         initializePathElementColumn(dto => dto.PathElement6, Captions.PathElement(6));
         initializePathElementColumn(dto => dto.PathElement7, Captions.PathElement(7));
         initializePathElementColumn(dto => dto.PathElement8, Captions.PathElement(8));
         initializePathElementColumn(dto => dto.PathElement9, Captions.PathElement(9));
      }

      private void initializePathElementColumn(Expression<Func<SelectableReplacePathAndValueDTO, string>> expression, string caption)
      {
         _pathElementsColumns.Add(_gridViewBinder.Bind(expression).WithCaption(caption).AsReadOnly());
      }

      private void initColumnVisibility(IReadOnlyList<SelectableReplacePathAndValueDTO> selectableReplacePathAndValueDTOs)
      {
         _pathElementsColumns.Each(column => column.Visible = HasAtLeastOneValue(_pathElementsColumns.IndexOf(column), selectableReplacePathAndValueDTOs));
      }

      public bool HasAtLeastOneValue(int pathElementIndex, IReadOnlyList<SelectableReplacePathAndValueDTO> selectableReplacePathAndValueDTOs)
      {
         return selectableReplacePathAndValueDTOs.HasAtLeastOneValue(pathElementIndex);
      }

      public void AttachPresenter(IPathAndValueEntitySelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSelectableEntities(IReadOnlyList<SelectableReplacePathAndValueDTO> selectableObjectPaths)
      {
         _gridViewBinder.BindToSource(selectableObjectPaths);
         initColumnVisibility(selectableObjectPaths);
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
      }
   }
}
