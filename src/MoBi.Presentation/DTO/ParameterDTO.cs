using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Assets;

namespace MoBi.Presentation.DTO
{
   public class ParameterDTO : ObjectBaseDTO, ITaggedEntityDTO, IMoBiParameterDTO
   {
      public bool HasRHS { get; set; }
      public FormulaBuilderDTO RHSFormula { get; set; }
      public FormulaBuilderDTO Formula { get; set; }
      public IParameter Parameter { get; private set; }
      public bool IsAdvancedParameter { get; set; }
      public IList<TagDTO> Tags { get; set; }
      public IGroup Group { get; set; }
      public bool CanBeVariedInPopulation { get; set; }
      public bool IsFavorite { get; set; }
      public string DisplayName { get; set; }
      public FormulaType FormulaType { get; set; }
      public int  Sequence { get; set; }
      public double Percentile { get; set; }
      public PathElements PathElements { get; set; }= new PathElements();
      public PathElementDTO SimulationPathElement => PathElements[PathElement.Simulation];
      public PathElementDTO TopContainerPathElement => PathElements[PathElement.TopContainer];
      public PathElementDTO ContainerPathElement => PathElements[PathElement.Container];
      public PathElementDTO BottomCompartmentPathElement => PathElements[PathElement.BottomCompartment];
      public PathElementDTO MoleculePathElement => PathElements[PathElement.Molecule];
      public PathElementDTO NamePathElement => PathElements[PathElement.Name];

      public string Category { get; }
      public string DisplayPathAsString => PathElements.Select(x => x.DisplayName).ToString(Constants.DISPLAY_PATH_SEPARATOR);
      public bool Editable => true;

      public event EventHandler ValueChanged = delegate { };

      private static readonly string _valueName = MoBiReflectionHelper.PropertyName<IQuantity>(x => x.Value);

      public IEnumerable<Unit> AllUnits
      {
         get { return Dimension.Units; }
         set
         {
            /*nothing to do here*/
         }
      }

      public override void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(_valueName))
         {
            ValueChanged(this, EventArgs.Empty);
         }

         base.HandlePropertyChanged(sender, e);
      }

      public string GroupName => Group.FullName;

      public ParameterDTO(IParameter parameter)
      {
         Parameter = parameter;
         Rules.Add(valueForConstantParameterShouldBeDefined());
         ListOfValues = new Cache<double, string>();
      }

      private IBusinessRule valueForConstantParameterShouldBeDefined()
      {
         return CreateRule.For<ParameterDTO>()
            .Property(dto => dto.Value)
            .WithRule((dto, value) => constantValueIsDefined(value, dto))
            .WithError(AppConstants.Validation.UndefinedParameter);
      }

      private static bool constantValueIsDefined(double value, ParameterDTO dto)
      {
         if (dto.Formula == null)
            return false;

         if (!string.Equals(dto.Formula.FormulaType, ObjectTypes.ConstantFormula))
            return true;

         return !double.IsNaN(value);
      }

      public virtual bool Persistable
      {
         get { return Parameter.Persistable; }
         set { Parameter.Persistable = value; }
      }

      public virtual ParameterBuildMode BuildMode
      {
         get { return Parameter.BuildMode; }
         set
         {
            /*nothing to do here since the BuildMode should be set in the command*/
         }
      }

      public virtual IDimension Dimension
      {
         get { return Parameter.Dimension; }
         set
         {
            /*nothing to do here since the Dimension should be set in the command*/
         }
      }

      public virtual Unit DisplayUnit
      {
         get { return Parameter.DisplayUnit; }
         set
         {
            /*nothing to do here since the unit should be set in the command*/
         }
      }

      public virtual string ValueDescription
      {
         get { return Parameter.ValueDescription; }
         set
         {
            /*nothing to do here since the unit should be set in the command*/
         }
      }

      public bool IsDiscrete => false;
      public ICache<double, string> ListOfValues { get; }

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

      public double KernelValue => Parameter.Value;

      public void Release()
      {
         if (Parameter == null)
            return;

         Parameter.PropertyChanged -= HandlePropertyChanged;
      }
   }
}