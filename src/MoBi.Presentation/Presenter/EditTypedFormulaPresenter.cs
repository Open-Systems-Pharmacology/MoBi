using OSPSuite.Utility;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter
{
   public interface IEditTypedFormulaPresenter : ISubjectPresenter, ICommandCollectorPresenter
   {
      IBuildingBlock BuildingBlock { set; }
      bool ReadOnly { set; get; }
      bool IsRHS { set; get; }

      /// <summary>
      ///    When calling this method, the presenter will try to display the absolute path used in the explicit formula instead
      ///    of relative path.
      ///    This is only useful when the paths are readonly (for instance in simulation)
      /// </summary>
      void Edit(IFormula formula, IEntity usingFormula = null);
   }

   public abstract class EditTypedFormulaPresenter<TView, TPresenter, TFormula> : AbstractEditPresenter<TView, TPresenter, TFormula>, IEditTypedFormulaPresenter, ILatchable
      where TView : IView<TPresenter>, IEditTypedFormulaView
      where TPresenter : IPresenter
      where TFormula : IFormula
   {
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      public virtual bool IsRHS { get; set; }

      public virtual IBuildingBlock BuildingBlock { set; get; }

      public bool IsLatched { get; set; }
      protected TFormula _formula;
      protected IEntity _formulaOwner;

      protected EditTypedFormulaPresenter(TView view, IDisplayUnitRetriever displayUnitRetriever) : base(view)
      {
         _displayUnitRetriever = displayUnitRetriever;
      }

      /// <summary>
      ///    Returns the display unit of the formula owner if defined. Otherwise, the preferred unit for the formula is returned
      /// </summary>
      protected Unit DisplayUnit
      {
         get
         {
            var withDisplayUnit = _formulaOwner as IWithDisplayUnit;
            return withDisplayUnit != null ? withDisplayUnit.DisplayUnit : _displayUnitRetriever.PreferredUnitFor(_formula.Dimension);
         }
      }

      public virtual void Edit(IFormula formula, IEntity formulaOwner = null)
      {
         _formulaOwner = formulaOwner;
         base.Edit(formula);
      }

      protected IUsingFormula UsingObject => _formulaOwner as IUsingFormula;

      public virtual bool ReadOnly
      {
         get => _view.ReadOnly;
         set => _view.ReadOnly = value;
      }

      public override object Subject => _formula;
   }
}