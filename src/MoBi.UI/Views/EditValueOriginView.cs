using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditValueOriginView : BaseResizableUserControl, IEditValueOriginView
   {
      private readonly ValueOriginBinder<ValueOriginDTO> _valueOriginBinder;
      private IEditValueOriginPresenter _presenter;
      private readonly GridViewBinder<ValueOriginDTO> _gridViewBinder;
      private IGridViewColumn _captionColumn;

      public EditValueOriginView(ValueOriginBinder<ValueOriginDTO> valueOriginBinder)
      {
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ValueOriginDTO>(gridView);
         gridView.ShowRowIndicator = false;
         gridView.ShowColumnHeaders = false;
         gridView.AllowsFiltering = false;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _captionColumn = _gridViewBinder.Bind(x => x.Caption)
            .AsReadOnly();

         _valueOriginBinder.InitializeBinding(_gridViewBinder,
            (dto, valueOrigin) => _presenter.ValueOriginUpdated(valueOrigin),
            dto => _presenter.ValueOriginEditable(),
            defaultColumnWidth: null);
      }

      public void AttachPresenter(IEditValueOriginPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ValueOriginDTO valueOriginDTO)
      {
         _gridViewBinder.BindToSource(new[] {valueOriginDTO});
         AdjustHeight();
      }

      public bool ShowCaption
      {
         get => _captionColumn.Visible;
         set => _captionColumn.Visible = value;
      }

      public override void Repaint()
      {
         gridView.LayoutChanged();
      }

      public override int OptimalHeight => gridView.OptimalHeight;
   }
}