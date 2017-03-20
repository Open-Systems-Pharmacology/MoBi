using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Presenter
{
   public interface IEditConstantFormulaPresenter : IEditTypedFormulaPresenter
   {
      /// <summary>
      /// Sets the display unit of the DTO and issues the command to update the underlying object.
      /// </summary>
      /// <param name="valueEditDTO">The DTO being modified</param>
      /// <param name="newDisplayUnit">The new unit being applied to the DTO. Note that setting a display unit will issue a call to <see cref="SetDisplayValue"/></param>
      void SetDisplayUnit(ValueEditDTO valueEditDTO, Unit newDisplayUnit);

      /// <summary>
      /// Sets the value on the DTO and issues the command to update the underlying object
      /// </summary>
      /// <param name="valueEditDTO">The DTO being modified</param>
      /// <param name="newValueInDisplayUnit">The new value in display units</param>
      void SetDisplayValue(ValueEditDTO valueEditDTO, double newValueInDisplayUnit);
   }

   public class EditConstantFormulaPresenter : EditTypedFormulaPresenter<IEditConstantFormulaView, IEditConstantFormulaPresenter, ConstantFormula>,
      IEditConstantFormulaPresenter,
      IListener<QuantityValueChangedEvent>
   {
      private readonly IConstantFormulaToDTOConstantFormulaMapper _formulaMapper;
      private readonly IMoBiFormulaTask _moBiFormulaTask;

      public EditConstantFormulaPresenter(IEditConstantFormulaView view, IConstantFormulaToDTOConstantFormulaMapper formulaMapper, IMoBiFormulaTask moBiFormulaTask, IDisplayUnitRetriever displayUnitRetriever) 
         : base(view,displayUnitRetriever)
      {
         _formulaMapper = formulaMapper;
         _moBiFormulaTask = moBiFormulaTask;
      }

      public override void Edit(ConstantFormula objectToEdit)
      {
         _formula = objectToEdit;
         refreshView();
      }

      private void refreshView()
      {
         _view.BindTo(_formulaMapper.MapFrom(_formula, DisplayUnit));
      }

      public void SetDisplayUnit(ValueEditDTO valueEditDTO, Unit newDisplayUnit)
      {
         setDisplayValue(valueEditDTO, valueEditDTO.Value, newDisplayUnit);

         var withDisplayUnit = _formulaOwner as IWithDisplayUnit;
         if (withDisplayUnit != null)
            withDisplayUnit.DisplayUnit = newDisplayUnit;
      }

      public void SetDisplayValue(ValueEditDTO valueEditDTO, double newValueInDisplayUnit)
      {
         setDisplayValue(valueEditDTO, newValueInDisplayUnit, valueEditDTO.DisplayUnit);
      }

      private void setDisplayValue(ValueEditDTO valueEditDTO, double newValueInDisplayUnit, Unit displayUnit)
      {
         var oldDisplayUnit = valueEditDTO.DisplayUnit;
         valueEditDTO.DisplayUnit = displayUnit;
         valueEditDTO.Value = newValueInDisplayUnit;
         this.DoWithinLatch(() => AddCommand(_moBiFormulaTask.SetConstantFormulaValue(_formula, valueEditDTO.KernelValue, displayUnit, oldDisplayUnit, BuildingBlock, _formulaOwner)));
      }

      public void Handle(QuantityValueChangedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         refreshView();
      }

      private bool canHandle(QuantityValueChangedEvent quantityValueChangedEvent)
      {
         if (IsLatched) return false;
         return Equals(quantityValueChangedEvent.Quantity, _formulaOwner);
      }
   }
}