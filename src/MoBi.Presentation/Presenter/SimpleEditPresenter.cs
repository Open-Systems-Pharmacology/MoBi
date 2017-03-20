using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISimpleEditPresenter<TData> : IEditPresenter<TData>, IPresenter<ISimpleEditView<TData>>
   {
   }

   public class SimpleEditPresenter<TData> : AbstractEditPresenter<ISimpleEditView<TData>, ISimpleEditPresenter<TData>,TData>, ISimpleEditPresenter<TData>
   {
      private TData _data;

      public SimpleEditPresenter(ISimpleEditView<TData> view) : base(view)
      {
      }

      public override void Edit(TData objectToEdit)
      {
         _data = objectToEdit;
         _view.Show(_data);
      }

      public override object Subject
      {
         get { return _data; }
      }
   }
}