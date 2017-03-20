using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   /// <summary>
   ///    interface defining a view containing a control that need to get the focus
   /// </summary>
   public interface IActivatableView : IView
   {
      void Activate();
   }

   public interface ISimpleEditView<TData> : IView<ISimpleEditPresenter<TData>>
   {
      void Show(TData data);
   }

   public interface IViewWithFormula
   {
      void SetFormulaView(IView view);
   }
}