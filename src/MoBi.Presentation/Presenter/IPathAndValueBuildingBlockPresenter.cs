using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.Presenter
{
   public interface IPathAndValueBuildingBlockPresenter<in TParameterDTO> : IBreadCrumbsPresenter
   {
      void SetParameterValue(TParameterDTO parameterDTO, double? newValue);
      void SetUnit(TParameterDTO parameterDTO, Unit unit);
      void SetFormula(TParameterDTO parameterDTO, IFormula newValueFormula);
      IEnumerable<ValueFormulaDTO> AllFormulas();
      void AddNewFormula(TParameterDTO parameterDTO);
      IEnumerable<IDimension> DimensionsSortedByName();
      void SetValueOrigin(TParameterDTO parameterDTO, ValueOrigin valueOrigin);
      void EditDistributedParameter(TParameterDTO distributedParameter);
   }
}