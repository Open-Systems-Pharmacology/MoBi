using System.Collections.Generic;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterPresenter : IPresenterWithFormulaCache, ICommandCollectorPresenter
   {
      void SetParamterUnit(IParameterDTO parameterDTO, Unit displayUnit);
      bool IsFixedValue(IParameterDTO parameterDTO);
      void OnParameterValueSet(IParameterDTO parameterDTO, double valueInGuiUnit);
      void OnParameterValueDescriptionSet(IParameterDTO parameterDTO, string valueDescription);
      void SetDimensionFor(IParameterDTO parameterDTO, IDimension newDimension);
      IEnumerable<IDimension> GetDimensions();
      void SetIsFavorite(IParameterDTO parameterDTO, bool isFavorite);
      void ResetValueFor(IParameterDTO parameterDTO);
   }
}