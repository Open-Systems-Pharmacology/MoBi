using System.Collections.Generic;
using System.ComponentModel;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using static MoBi.Assets.AppConstants.Captions;
using static OSPSuite.Core.Domain.Constants.Distribution;

namespace MoBi.Presentation.DTO
{
   public class PathAndValueEntityDTO<T, TSubParameter> : PathAndValueEntityDTO<T> where T : PathAndValueEntity where TSubParameter : PathAndValueEntityDTO<T>
   {
      private readonly List<TSubParameter> _subParameters = new List<TSubParameter>();
      public IReadOnlyList<TSubParameter> SubParameters => _subParameters;

      public TSubParameter MeanDTO => SubParameters.FindByName(MEAN);

      protected PathAndValueEntityDTO(T underlyingObject) : base(underlyingObject)
      {
      }

      public void AddSubParameter(TSubParameter subParameter)
      {
         _subParameters.Add(subParameter);
      }

      public bool IsDistributed => PathWithValueObject.DistributionType != null;

      public string DistributionType => $"{PathWithValueObject.DistributionType} {Distribution}";

      public override Unit DisplayUnit
      {
         get => IsDistributed ? MeanDTO?.DisplayUnit : base.DisplayUnit;
         set => base.DisplayUnit = value;
      }

      public override double? Value
      {
         get => IsDistributed ? MeanDTO?.Value : base.Value;
         set => base.Value = value;
      }
   }

   public class PathAndValueEntityDTO<T> : BreadCrumbsDTO<T>, IPathAndValueEntityDTO 
      where T : PathAndValueEntity, IValidatable, INotifier, IWithDimension, IWithDisplayUnit, IUsingFormula
   {
      public string ContainerPathPropertyName;
      public string FormulaPropertyName;
      private ValueFormulaDTO _formula;
      public T PathWithValueObject { get; }

      protected PathAndValueEntityDTO(T underlyingObject) : base(underlyingObject)
      {
         PathWithValueObject = underlyingObject;
         PathWithValueObject.PropertyChanged += underlyingObjectOnPropertyChanged;
         ContainerPathPropertyName = MoBiReflectionHelper.PropertyName<PathAndValueEntity>(x => x.ContainerPath);
         FormulaPropertyName = MoBiReflectionHelper.PropertyName<PathAndValueEntity>(x => x.Formula);
         ContainerPath = underlyingObject.ContainerPath;
      }

      public string Name
      {
         get => PathWithValueObject.Name;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public IDimension Dimension
      {
         get => PathWithValueObject.Dimension;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public virtual Unit DisplayUnit
      {
         get => PathWithValueObject.DisplayUnit;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public IEnumerable<Unit> AllUnits
      {
         get => Dimension.Units;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public ValueFormulaDTO Formula
      {
         get => _formula;
         set
         {
            _formula = value;
            OnPropertyChanged(() => Formula);
         }
      }

      public virtual double? Value
      {
         get
         {
            if (Formula == null || Formula.Formula == null)
               return PathWithValueObject.ConvertToDisplayUnit(PathWithValueObject.Value);
            return double.NaN;
         }
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public virtual ValueOrigin ValueOrigin
      {
         get => PathWithValueObject.ValueOrigin;
         set => UpdateValueOriginFrom(value);
      }

      private void underlyingObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
      {
         var changedProperty = propertyChangedEventArgs.PropertyName;
         if (changedProperty.Equals(ContainerPathPropertyName))
         {
            ContainerPath = GetContainerPath();
         }
         else if (changedProperty.Equals(FormulaPropertyName))
         {
            Formula = PathWithValueObject.Formula.IsExplicit() ? new ValueFormulaDTO(PathWithValueObject.Formula as ExplicitFormula) : new EmptyFormulaDTO();
         }
      }

      protected virtual ObjectPath GetContainerPath()
      {
         return ContainerPath;
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         PathWithValueObject.UpdateValueOriginFrom(ValueOrigin);
      }
   }
}