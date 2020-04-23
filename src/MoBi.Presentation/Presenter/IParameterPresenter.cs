using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IParameterPresenter : IPresenterWithFormulaCache, ICommandCollectorPresenter
   {
      void SetParameterUnit(IParameterDTO parameterDTO, Unit displayUnit);
      bool IsFixedValue(IParameterDTO parameterDTO);
      void OnParameterValueSet(IParameterDTO parameterDTO, double valueInGuiUnit);
      void OnParameterValueOriginSet(IParameterDTO parameterDTO, ValueOrigin valueOrigin);
      IReadOnlyList<IDimension> GetDimensions();
      void SetIsFavorite(IParameterDTO parameterDTO, bool isFavorite);
      void ResetValueFor(IParameterDTO parameterDTO);
   }
}