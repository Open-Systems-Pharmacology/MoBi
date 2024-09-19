using System;
using System.Collections;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IFormulaPresenterCache : IReleasable, IEnumerable<IEditTypedFormulaPresenter>
   {
      IEditTypedFormulaPresenter PresenterFor(IFormula formula);
      IEditTypedFormulaPresenter PresenterFor(Type formulaType);
      bool HasPresenterFor(Type formulaType);
   }

   internal class FormulaPresenterCache : IFormulaPresenterCache
   {
      private readonly IContainer _container;
      private readonly ICache<Type, IEditTypedFormulaPresenter> _cache;

      public FormulaPresenterCache(IContainer container)
      {
         _container = container;
         _cache = new Cache<Type, IEditTypedFormulaPresenter>();
      }

      public IEditTypedFormulaPresenter PresenterFor(IFormula formula)
      {
         return PresenterFor(formula.GetType());
      }

      public IEditTypedFormulaPresenter PresenterFor(Type formulaType)
      {
         if (formulaType == typeof(ExplicitFormula))
            return presenterFor<IEditExplicitFormulaPresenter>(formulaType);

         if (formulaType == typeof(BlackBoxFormula))
            return presenterFor<IEditBlackBoxFormulaPresenter>(formulaType);

         if (formulaType == typeof(ConstantFormula))
            return presenterFor<IEditConstantFormulaPresenter>(formulaType);

         if (formulaType == typeof(TableFormula) || formulaType == typeof(DistributedTableFormula))
            return presenterFor<IEditTableFormulaPresenter>(formulaType);

         if (formulaType == typeof(TableFormulaWithXArgument))
            return presenterFor<IEditTableFormulaWithXArgumentFormulaPresenter>(formulaType);

         if (formulaType == typeof(TableFormulaWithOffset))
            return presenterFor<IEditTableFormulaWithOffsetFormulaPresenter>(formulaType);

         if (formulaType == typeof(SumFormula))
            return presenterFor<IEditSumFormulaPresenter>(formulaType);

         throw new NotSupportedException($"Formula '{formulaType.Name}' not supported at the moment");
      }

      public bool HasPresenterFor(Type formulaType)
      {
         return _cache.Contains(formulaType);
      }

      public void ReleaseFrom(IEventPublisher eventPublisher)
      {
         _cache.Each(x => x.ReleaseFrom(eventPublisher));
         _cache.Each(x => x.BaseView.Dispose());
         _cache.Clear();
      }

      private IEditTypedFormulaPresenter presenterFor<T>(Type formulaType) where T : IEditTypedFormulaPresenter
      {
         if (!HasPresenterFor(formulaType))
         {
            _cache.Add(formulaType, _container.Resolve<T>());
         }

         return _cache[formulaType];
      }

      public IEnumerator<IEditTypedFormulaPresenter> GetEnumerator()
      {
         return _cache.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}