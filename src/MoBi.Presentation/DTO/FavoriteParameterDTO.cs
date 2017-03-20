using System;
using System.Collections.Generic;
using System.ComponentModel;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public interface IMoBiParameterDTO : IParameterDTO
   {
      FormulaBuilderDTO RHSFormula { get; set; }
      FormulaBuilderDTO Formula { get; set; }
   }

   public class FavoriteParameterDTO : PathRepresentableDTO, IViewItem, IMoBiParameterDTO
   {
      public IDimension Dimension { get; set; }
      public FormulaBuilderDTO RHSFormula { get; set; }
      public FormulaBuilderDTO Formula { get; set; }

      public virtual Unit DisplayUnit
      {
         get { return Parameter.DisplayUnit; }
         set
         {
            /*nothing to do here since the unit should be set in the command*/
         }
      }

      /// <summary>
      ///    Returns the value in display unit
      /// </summary>
      public double Value
      {
         get
         {
            try
            {
               return Parameter.ConvertToDisplayUnit(Parameter.Value);
            }
            catch (Exception)
            {
               return double.NaN;
            }
         }
         set
         {
            /*nothing to do here since the value should be set in the command*/
         }
      }

      public IParameter Parameter { get; private set; }

      public double KernelValue => Parameter.Value;

      public bool IsFavorite { get; set; }

      public string ValueDescription
      {
         get { return Parameter.ValueDescription; }
         set { }
      }

      public bool IsDiscrete => false;
      public ICache<double, string> ListOfValues { get; }
      public string DisplayName { get; set; }
      public FormulaType FormulaType { get; set; }
      public int Sequence { get; set; }
      public double Percentile { get; set; }
      public bool Editable => true;
      public event EventHandler ValueChanged = delegate { };

      public string Name
      {
         get { return PathElements[PathElement.Name].DisplayName; }
         set { PathElements[PathElement.Name].DisplayName = value; }
      }

      public string Description
      {
         get { return Parameter.Description; }
         set { Parameter.Description = value; }
      }

      public FavoriteParameterDTO(IParameter parameter)
      {
         Parameter = parameter;
         Parameter.PropertyChanged += HandlePropertyChanged;
         ListOfValues = new Cache<double, string>();
      }

      public IEnumerable<Unit> AllUnits
      {
         get { return Dimension.Units; }
         set { }
      }

      public void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         RaisePropertyChanged(e.PropertyName);
      }

      public void Release()
      {
         Parameter.PropertyChanged -= HandlePropertyChanged;
      }
   }
}