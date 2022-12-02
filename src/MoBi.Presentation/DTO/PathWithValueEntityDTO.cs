using System.Collections.Generic;
using System.ComponentModel;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public abstract class PathWithValueEntityDTO<T> : BreadCrumbsDTO<T> where T : IValidatable, INotifier, IWithDimension, IWithDisplayUnit, IUsingFormula
   {
      public string ContainerPathPropertyName;
      public string FormulaPropertyName;
      private ValueFormulaDTO _formula;
      public T StartValueObject { get; }

      protected PathWithValueEntityDTO(T underlyingObject) : base(underlyingObject)
      {
         StartValueObject = underlyingObject;
         StartValueObject.PropertyChanged += underlyingObjectOnPropertyChanged;
         ContainerPathPropertyName = MoBiReflectionHelper.PropertyName<IStartValue>(x => x.ContainerPath);
         FormulaPropertyName = MoBiReflectionHelper.PropertyName<IStartValue>(x => x.Formula);
      }

      public string Name
      {
         get => StartValueObject.Name;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public IDimension Dimension
      {
         get => StartValueObject.Dimension;
         set
         {
            // We don't want the binding to set the value in the underlying object, only the command should do that
         }
      }

      public Unit DisplayUnit
      {
         get => StartValueObject.DisplayUnit;
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

      private void underlyingObjectOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
      {
         var changedProperty = propertyChangedEventArgs.PropertyName;
         if (changedProperty.Equals(ContainerPathPropertyName))
         {
            ContainerPath = GetContainerPath();
         }
         else if (changedProperty.Equals(FormulaPropertyName))
         {
            Formula = StartValueObject.Formula.IsExplicit() ? new ValueFormulaDTO(StartValueObject.Formula as ExplicitFormula) : new EmptyFormulaDTO();
         }
      }

      protected abstract IObjectPath GetContainerPath();

   }
}