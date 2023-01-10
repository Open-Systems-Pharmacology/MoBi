using System.Collections.Generic;
using System.ComponentModel;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public  class PathWithValueEntityDTO<T> : BreadCrumbsDTO<T>, IWithDisplayUnitDTO where T : PathAndValueEntity, IValidatable, INotifier, IWithDimension, IWithDisplayUnit, IUsingFormula
   {
      public string ContainerPathPropertyName;
      public string FormulaPropertyName;
      private ValueFormulaDTO _formula;
      public T PathWithValueObject { get; }

      protected PathWithValueEntityDTO(T underlyingObject) : base(underlyingObject)
      {
         PathWithValueObject = underlyingObject;
         PathWithValueObject.PropertyChanged += underlyingObjectOnPropertyChanged;
         ContainerPathPropertyName = MoBiReflectionHelper.PropertyName<IStartValue>(x => x.ContainerPath);
         FormulaPropertyName = MoBiReflectionHelper.PropertyName<IStartValue>(x => x.Formula);
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

      public Unit DisplayUnit
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

      public double? Value
      {
         get
         {
            if (Formula == null || Formula.Formula == null)
               return PathWithValueObject.ConvertToDisplayUnit(PathWithValueObject.Value);
            return double.NaN;
         }
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

      protected virtual IObjectPath GetContainerPath()
      {
         return ContainerPath;
      }
   }
}