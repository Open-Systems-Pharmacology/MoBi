using System;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Exceptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.UnitSystem
{
   public interface IMoBiDimensionConverter : IDimensionConverter
   {
      bool CanBeUsedFor(object refObject);
      void SetRefObject(object refObject);
   }

   public interface IMoBiDimensionConverter<T> : IMoBiDimensionConverter where T : IWithDimension
   {
      void SetRefObject(T refObject);
   }

   public abstract class DimensionConverter<T> : IMoBiDimensionConverter<T> where T : IWithDimension
   {
      private readonly IDimension _sourceDimension;
      private readonly IDimension _targetDimension;

      protected DimensionConverter(IDimension sourceDimension, IDimension targetDimension)
      {
         _sourceDimension = sourceDimension;
         _targetDimension = targetDimension;
      }

      public virtual bool CanResolveParameters()
      {
         return GetFactor().HasValue;
      }

      public bool CanBeUsedFor(object refObject)
      {
         return refObject.IsAnImplementationOf<T>();
      }

      public void SetRefObject(object refObject)
      {
         if (CanBeUsedFor(refObject))
         {
            SetRefObject((T) refObject);
         }
         else
         {
            throw new MoBiException();
         }
      }

      public abstract void SetRefObject(T refObject);

      public double ConvertToTargetBaseUnit(double sourceBaseUnitValue)
      {
         var factor = GetFactor();
         if (factor.HasValue && !Double.IsNaN(factor.Value))
            return sourceBaseUnitValue * factor.Value;

         throw new MoBiException();
      }

      public double ConvertToSourceBaseUnit(double targetBaseUnitValue)
      {
         var factor = GetFactor();
         if (factor.HasValue && !Double.IsNaN(factor.Value))
            return targetBaseUnitValue / factor.Value;

         throw new MoBiException();
      }

      public bool CanConvertTo(IDimension dimension)
      {
         return _targetDimension.Equals(dimension);
      }

      public bool CanConvertFrom(IDimension dimension)
      {
         return _sourceDimension.Equals(dimension);
      }

      public abstract string UnableToResolveParametersMessage { get; }

      protected abstract double? GetFactor();
   }

   public class MolWeightDimensionConverterForFormulaUsable : DimensionConverter<IQuantity>
   {
      private IQuantity _formulaUsable;
      private ObjectPath _useablePath;

      public MolWeightDimensionConverterForFormulaUsable(IDimension sourceDimension, IDimension targetDimension) : base(sourceDimension, targetDimension)
      {
      }

      public override bool CanResolveParameters()
      {
         var root = _formulaUsable?.RootContainer;
         if (root == null)
            return false;

         _useablePath = new ObjectPath(root.Name, _formulaUsable.Name, Constants.Parameters.MOL_WEIGHT);
         return _useablePath.TryResolve<IFormulaUsable>(root) != null;
      }

      public override void SetRefObject(IQuantity refObject)
      {
         _formulaUsable = refObject;
      }

      public override string UnableToResolveParametersMessage => $"Could not resolve path {_useablePath}";

      protected override double? GetFactor()
      {
         if (!CanResolveParameters())
            return null;

         var root = _formulaUsable.RootContainer;
         var useablePath = new ObjectPath(root.Name, _formulaUsable.Name, Constants.Parameters.MOL_WEIGHT);
         var entity = useablePath.Resolve<IFormulaUsable>(root);
         if (double.IsNaN(entity.Value))
            return null;

         return 1.0 / entity.Value;
      }
   }


   public abstract class MolWeightDimensionConverterForDataColumn : DimensionConverter<DataColumn>
   {
      protected double? _mw;

      protected MolWeightDimensionConverterForDataColumn(IDimension sourceDimension, IDimension targetDimension) : base(sourceDimension, targetDimension)
      {
      }

      public override string UnableToResolveParametersMessage => "MolWeight not set at DataColumn";

      public override void SetRefObject(DataColumn refObject)
      {
         if (refObject.DataInfo.MolWeight != null && ! Double.IsNaN(refObject.DataInfo.MolWeight.Value))
         {
            _mw = refObject.DataInfo.MolWeight;
         }
         else
         {
            _mw = null;
         }
      }
   }

   public class ConcentrationToMolarConcentrationConverterForDataColumn : MolWeightDimensionConverterForDataColumn
   {
      public ConcentrationToMolarConcentrationConverterForDataColumn(IDimension sourceDimension, IDimension targetDimension)
         : base(sourceDimension, targetDimension)
      {
      }

      protected override double? GetFactor()
      {
         return _mw.HasValue ? 1 / _mw : null;
      }
   }

   public class MolarConcentrationToConcentrationConverterForDataColumn : MolWeightDimensionConverterForDataColumn
   {
      public MolarConcentrationToConcentrationConverterForDataColumn(IDimension sourceDimension, IDimension targetDimension)
         : base(sourceDimension, targetDimension)
      {
      }

      protected override double? GetFactor()
      {
         return _mw.HasValue ? _mw : null;
      }
   }
}