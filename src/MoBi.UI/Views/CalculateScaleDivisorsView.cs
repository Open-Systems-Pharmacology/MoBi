using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class CalculateScaleDivisorsView : BaseModalView, ICalculateScaleDivisorsView
   {
      private ICalculateScaleDivisorsPresenter _presenter;
      private readonly GridViewBinder<ScaleDivisorDTO> _gridViewBinder;
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>(AppConstants.MAX_PATH_DEPTH);

      public CalculateScaleDivisorsView(IShell shell) : base(shell)
      {
         InitializeComponent();
         gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         _gridViewBinder = new GridViewBinder<ScaleDivisorDTO>(gridView);
      }

      public void AttachPresenter(ICalculateScaleDivisorsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<ScaleDivisorDTO> scaleDivisors)
      {
         _gridViewBinder.BindToSource(scaleDivisors);
         initColumnVisibility();
      }

      public bool Calculating
      {
         get => ExtraEnabled;
         set
         {
            ExtraEnabled = !value;
            OkEnabled = ExtraEnabled;
            btnCalculateScaleDivisors.Enabled = ExtraEnabled;
         }
      }

      public void RefreshData()
      {
         gridControl.RefreshDataSource();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         initPathElementColumn(dto => dto.PathElement0, Captions.PathElement(0));
         initPathElementColumn(dto => dto.PathElement1, Captions.PathElement(1));
         initPathElementColumn(dto => dto.PathElement2, Captions.PathElement(2));
         initPathElementColumn(dto => dto.PathElement3, Captions.PathElement(3));
         initPathElementColumn(dto => dto.PathElement4, Captions.PathElement(4));
         initPathElementColumn(dto => dto.PathElement5, Captions.PathElement(5));
         initPathElementColumn(dto => dto.PathElement6, Captions.PathElement(6));
         initPathElementColumn(dto => dto.PathElement7, Captions.PathElement(7));
         initPathElementColumn(dto => dto.PathElement8, Captions.PathElement(8));
         initPathElementColumn(dto => dto.PathElement9, Captions.PathElement(9));

         _gridViewBinder.AutoBind(x => x.ScaleDivisor)
            .WithOnValueUpdating((o, e) => _presenter.UpdateScaleFactorValue(o, e.NewValue));

         _gridViewBinder.Changed += NotifyViewChanged;

         btnCalculateScaleDivisors.Click += (o, e) => OnEvent(async () => await _presenter.StartScaleDivisorsCalculation());
      }

      protected override void ExtraClicked()
      {
         _presenter.ResetScaleDivisors();
      }

      private void initPathElementColumn(Expression<Func<ScaleDivisorDTO, string>> expression, string caption)
      {
         _pathElementsColumns.Add(_gridViewBinder.Bind(expression).WithCaption(caption).AsReadOnly());
      }

      public override bool HasError => _gridViewBinder.HasError;

      private void initColumnVisibility()
      {
         for (int i = 0; i < _pathElementsColumns.Count; i++)
         {
            _pathElementsColumns[i].Visible = _presenter.HasAtLeastTwoDistinctValues(i);
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ExtraVisible = true;
         layoutItemCalculateScaleDivisors.AdjustButtonSize();
         btnCalculateScaleDivisors.InitWithImage(ApplicationIcons.Run, AppConstants.Captions.Calculate);
         ButtonExtra.InitWithImage(ApplicationIcons.Reset, AppConstants.Captions.Reset);
         ApplicationIcon = ApplicationIcons.ScaleFactor;
         Caption = AppConstants.Captions.CalculateScaleDivisor;
      }
   }
}