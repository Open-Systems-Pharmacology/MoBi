using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Helper;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ParameterDTO : ObjectBaseDTO, IParameterDTO
   {
      public bool HasRHS { get; set; }
      public FormulaBuilderDTO RHSFormula { get; set; }
      public FormulaBuilderDTO Formula { get; set; }
      public IParameter Parameter { get; }
      public bool IsAdvancedParameter { get; set; }
      public IGroup Group { get; set; }
      public bool CanBeVariedInPopulation { get; set; }
      public bool CanBeVaried => Parameter.CanBeVaried;
      public bool IsFavorite { get; set; }
      public string DisplayName { get; set; }
      public FormulaType FormulaType { get; set; }
      public int Sequence { get; set; }
      public double Percentile { get; set; }
      public PathElements PathElements { get; set; } = new PathElements();
      public PathElement SimulationPathElement => PathElements[PathElementId.Simulation];
      public PathElement TopContainerPathElement => PathElements[PathElementId.TopContainer];
      public PathElement ContainerPathElement => PathElements[PathElementId.Container];
      public PathElement BottomCompartmentPathElement => PathElements[PathElementId.BottomCompartment];
      public PathElement MoleculePathElement => PathElements[PathElementId.Molecule];
      public PathElement NamePathElement => PathElements[PathElementId.Name];

      public string Category { get; }
      public string DisplayPathAsString => PathElements.Select(x => x.DisplayName).ToString(Constants.DISPLAY_PATH_SEPARATOR);
      public bool Editable => true;

      public event EventHandler ValueChanged = delegate { };

      private static readonly string _valueName = MoBiReflectionHelper.PropertyName<IQuantity>(x => x.Value);

      public IEnumerable<Unit> AllUnits
      {
         get => Dimension.Units;
         set
         {
            /*nothing to do here*/
         }
      }

      protected override void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName.Equals(_valueName))
         {
            ValueChanged(this, EventArgs.Empty);
         }

         base.HandlePropertyChanged(sender, e);
      }

      public string GroupName => Group.FullName;

      public ParameterDTO(IParameter parameter) : base(parameter)
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

      public bool Persistable
      {
         get => Parameter.Persistable;
         set => Parameter.Persistable = value;
      }

      public ParameterBuildMode BuildMode
      {
         get => Parameter.BuildMode;
         set
         {
            /*nothing to do here since the BuildMode should be set in the command*/
         }
      }

      public IDimension Dimension
      {
         get => Parameter.Dimension;
         set
         {
            /*nothing to do here since the Dimension should be set in the command*/
         }
      }

      public Unit DisplayUnit
      {
         get => Parameter.DisplayUnit;
         set
         {
            /*nothing to do here since the unit should be set in the command*/
         }
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         Parameter.UpdateValueOriginFrom(ValueOrigin);
      }

      public ValueOrigin ValueOrigin
      {
         get => Parameter.ValueOrigin;
         set => UpdateValueOriginFrom(value);
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

      public bool IsIndividualPreview { get; set; }

      public SimulationEntitySourceReference SourceReference { get; set; }

      public string ModuleName => SourceReference?.Module?.Name ?? string.Empty;
      public string BuildingBlockName => SourceReference?.BuildingBlock.Name ?? string.Empty;
      public string SourceName => SourceReference?.Source.Name ?? string.Empty;

      public IBuildingBlock BuildingBlock => SourceReference?.BuildingBlock;
   }
}