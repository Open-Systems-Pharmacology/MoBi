using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter.BasePresenter
{
   public abstract class AbstractSubPresenterWithFormula<TView, TPresenter> : AbstractCommandCollectorPresenter<TView, TPresenter> where TPresenter : IPresenter
      where TView : IView<TPresenter>, IViewWithFormula
   {
      protected readonly IEditFormulaInContainerPresenter _editFormulaPresenter;
      protected ISelectReferencePresenter _referencePresenter;

      protected AbstractSubPresenterWithFormula(TView view, IEditFormulaInContainerPresenter editFormulaPresenter, ISelectReferencePresenter referencePresenter) : base(view)
      {
         _editFormulaPresenter = editFormulaPresenter;
         _referencePresenter = referencePresenter;
         _editFormulaPresenter.ReferencePresenter = _referencePresenter;
         _editFormulaPresenter.StatusChanged += (o, e) => FormulaChanged();
         _view.SetFormulaView(_editFormulaPresenter.BaseView);
         AddSubPresenters(_editFormulaPresenter, _referencePresenter);
      }

      protected virtual void FormulaChanged()
      {
         //should be overriden when required
      }
   }
}