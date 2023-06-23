using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Presenter
{
   public interface IPathAndValueBuildingBlockPresenter<in TDTO> : IBreadCrumbsPresenter
   {
      void SetParameterValue(TDTO expressionParameterDTO, double? newValue);
      void SetUnit(TDTO expressionParameter, Unit unit);
      void SetFormula(TDTO expressionParameterDTO, IFormula newValueFormula);
      IEnumerable<ValueFormulaDTO> AllFormulas();
      void AddNewFormula(TDTO expressionParameterDTO);
      IEnumerable<IDimension> DimensionsSortedByName();
   }
}