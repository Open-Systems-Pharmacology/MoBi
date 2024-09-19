using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectFormulaUsablePathPresenter : IDisposablePresenter
   {
      void Init(Func<IObjectBase, bool> predicate, IEntity refObject, IReadOnlyList<IObjectBase> entities, string caption, ISelectReferencePresenter referencePresenter);
      FormulaUsablePath GetSelection();
   }

   internal class SelectFormulaUsablePathPresenter : AbstractDisposablePresenter<ISelectFormulaUsablePathView, ISelectFormulaUsablePathPresenter>, ISelectFormulaUsablePathPresenter
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private ISelectReferencePresenter _referencePresenter;

      public SelectFormulaUsablePathPresenter(ISelectFormulaUsablePathView view, IObjectPathFactory objectPathFactory) : base(view)
      {
         _objectPathFactory = objectPathFactory;
      }

      private void enableDisableButtons()
      {
         _view.OkEnabled = _referencePresenter.LegalObjectSelected;
      }

      public void Init(Func<IObjectBase, bool> predicate, IEntity refObject, IReadOnlyList<IObjectBase> entities, string caption, ISelectReferencePresenter referencePresenter)
      {
         _view.Text = caption;
         _referencePresenter = referencePresenter;
         _referencePresenter.SelectionChangedEvent += enableDisableButtons;
         _referencePresenter.SelectionPredicate = predicate;
         _referencePresenter.Init(refObject, entities, null);
         _view.AddSelectionView(_referencePresenter.View);
      }

      public FormulaUsablePath GetSelection()
      {
         _view.Display();
         if (_view.Canceled) return null;
         var selectedPath = getSelectedPath();
         return _objectPathFactory.CreateFormulaUsablePathFrom(selectedPath).WithAlias(selectedPath.Last());
      }

      private ObjectPath getSelectedPath()
      {
         return _referencePresenter.GetSelection();
      }
   }
}