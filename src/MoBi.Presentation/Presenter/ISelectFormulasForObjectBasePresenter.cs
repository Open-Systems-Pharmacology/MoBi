using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectFormulasForObjectBasePresenter : IDisposablePresenter
   {
      bool SelectFrom(ICache<IObjectBase, IList<IFormula>> multipleDefinitionsToResolve);
      ICache<IObjectBase, IFormula> Selction { get; }
      IEnumerable<IFormula> GetFormulasFor(IObjectBase objectBase);
   }

   internal class SelectFormulasForObjectBasePresenter : AbstractDisposablePresenter<ISelectFormulasForObjectBaseView, ISelectFormulasForObjectBasePresenter>, ISelectFormulasForObjectBasePresenter
   {
      private ICache<IObjectBase, IFormula> _selction;
      private ICache<IObjectBase, IList<IFormula>> _multipleDefinitionsToResolve;

      public SelectFormulasForObjectBasePresenter(ISelectFormulasForObjectBaseView view): base(view)
      {
      }

      public bool SelectFrom(ICache<IObjectBase, IList<IFormula>> multipleDefinitionsToResolve)
      {
         _selction = new Cache<IObjectBase, IFormula>();
         _multipleDefinitionsToResolve = multipleDefinitionsToResolve;
         var dtos = createDTOs(_multipleDefinitionsToResolve).ToList();

         _view.Show(dtos);
         _view.Display();
         var accepted = !_view.Canceled;
         if (accepted)
         {
            foreach (var dtoObjectFormula in dtos)
            {
               _selction.Add(dtoObjectFormula.ObjectBase, dtoObjectFormula.Formula);
            }
         }
         return accepted;
      }

      private IEnumerable<ObjectFormulaDTO> createDTOs(ICache<IObjectBase, IList<IFormula>> multipleDefinitionsToResolve)
      {
         return multipleDefinitionsToResolve.KeyValues.
            Select(definition => new ObjectFormulaDTO {ObjectBase = definition.Key, Formula = definition.Value.First()}).ToList();
      }

      public ICache<IObjectBase, IFormula> Selction
      {
         get { return _selction; }
      }

      public IEnumerable<IFormula> GetFormulasFor(IObjectBase objectBase)
      {
         return _multipleDefinitionsToResolve[objectBase];
      }
   }
}