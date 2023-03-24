using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateDimensionInParameterStartValueCommand : ContextSpecification<UpdateDimensionInStartValueCommand<ParameterStartValue>>
   {
      protected IDimension _newDimension;
      protected IDimension _oldDimension;
      protected ParameterStartValue _startValue;
      protected ParameterStartValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected Unit _newDisplayUnit;
      protected Unit _oldDisplayUnit;

      protected override void Context()
      {
         _buildingBlock = new ParameterStartValuesBuildingBlock();
         _context = A.Fake<IMoBiContext>();
         _newDimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);
         _newDisplayUnit = _newDimension.DefaultUnit;

         _oldDimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Concentration);
         _oldDisplayUnit = _oldDimension.DefaultUnit;

         _startValue = new ParameterStartValue {Dimension = _oldDimension, StartValue = 1.0, DisplayUnit = _oldDisplayUnit};


         sut = new UpdateDimensionInStartValueCommand<ParameterStartValue>(_startValue, _newDimension, _newDisplayUnit, _buildingBlock);
      }
   }

   public class When_converting_dimensions_on_parameter_start_value : concern_for_UpdateDimensionInParameterStartValueCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void dimensions_associated_with_start_value_have_changed()
      {
         _startValue.Dimension.ShouldBeEqualTo(_newDimension);
      }

      [Observation]
      public void dimensions_update_associated_display_unit()
      {
         _startValue.DisplayUnit.ShouldBeEqualTo(_newDisplayUnit);
      }
   }

   public class When_reversing_dimension_change : concern_for_UpdateDimensionInParameterStartValueCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void original_dimension_restored()
      {
         _startValue.Dimension.ShouldBeEqualTo(_oldDimension);
      }

      [Observation]
      public void original_display_unit_restored()
      {
         _startValue.DisplayUnit.ShouldBeEqualTo(_oldDisplayUnit);
      }
   }
}