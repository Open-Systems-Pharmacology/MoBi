using System;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class ValueEditDTO : Notifier
   {
      private IDimension _dimension;
      private Unit _displayUnit;
      private double _kernelValue;

      public virtual double KernelValue
      {
         get => _kernelValue;
         set
         {
            _kernelValue = value;
            OnPropertyChanged(() => Value);
         }
      }

      public double Value
      {
         get => valueToDisplayValue(KernelValue);
         //Settings the Kernel Value triggers a Value Change. Event. No need to send it twice
         set => KernelValue = displayValueToValue(value);
      }

      public Unit DisplayUnit
      {
         get => _displayUnit;
         set
         {
            //the unit was set. We have to update the value 
            //18 years=> months, the display value should still be 18 to get 18 months
            double currentDisplayValue = Value;
            _displayUnit = value;
            Value = currentDisplayValue;
         }
      }

      public virtual IDimension Dimension
      {
         get => _dimension;
         set
         {
            _dimension = value;
            _displayUnit = _dimension.DefaultUnit;
         }
      }

      private double displayValueToValue(double displayValue)
      {
         try
         {
            return Dimension.UnitValueToBaseUnitValue(DisplayUnit, displayValue);
         }
         catch (Exception)
         {
            return double.NaN;
         }
      }

      private double valueToDisplayValue(double value)
      {
         try
         {
            return Dimension.BaseUnitValueToUnitValue(DisplayUnit, value);
         }
         catch (Exception)
         {
            return double.NaN;
         }
      }
   }
}