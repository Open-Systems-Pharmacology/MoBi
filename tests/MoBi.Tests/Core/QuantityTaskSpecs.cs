using System.Linq;
using FakeItEasy;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_QuantityTask : ContextSpecification<IQuantityTask>
   {
      protected IMoBiContext _context;
      protected IQuantitySynchronizer _quantitySynchronizer;
      protected IParameter _parameter;
      protected ICommand _result;
      protected IMoBiSimulation _simulation;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _quantitySynchronizer = A.Fake<IQuantitySynchronizer>();
         _context = A.Fake<IMoBiContext>();
         _simulation = A.Fake<IMoBiSimulation>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _parameter = new Parameter().WithFormula(new ExplicitFormula("1+2"));
         _parameter.Dimension = DomainHelperForSpecs.TimeDimension;
         sut = new QuantityTask(_context, _quantitySynchronizer);
      }
   }

   public class When_resetting_the_value_of_a_quantity_in_a_simulation : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.Value = 5;
      }

      protected override void Because()
      {
         _result = sut.ResetQuantityValue(_parameter, _simulation);
      }

      [Observation]
      public void should_ensure_that_the_description_of_the_resulting_commmand_is_set()
      {
         string.IsNullOrEmpty(_result.Description).ShouldBeFalse();
      }

      [Observation]
      public void should_reset_the_value()
      {
         _parameter.Value.ShouldBeEqualTo(3);
         _parameter.IsFixedValue.ShouldBeFalse();
      }
   }

   public class When_updating_the_value_of_a_parameter_in_a_simulation_that_is_not_default_and_for_which_the_value_origin_was_defined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Internet;
         _parameter.IsDefault = false;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayValue(_parameter, 5, _simulation);
      }

      [Observation]
      public void should_update_the_parameter_value()
      {
         _parameter.Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_not_update_the_value_origin()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Internet);
      }
   }

   public class When_updating_the_value_of_a_parameter_in_a_simulation_that_is_default_but_for_which_the_value_origin_was_defined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Internet;
         _parameter.IsDefault = true;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayValue(_parameter, 5, _simulation);
      }

      [Observation]
      public void should_update_the_parameter_value()
      {
         _parameter.Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_update_the_value_origin()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
         _parameter.ValueOrigin.Method.ShouldBeEqualTo(ValueOriginDeterminationMethods.Undefined);
      }

      [Observation]
      public void should_update_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_the_value_of_a_parameter_in_a_simulation_that_is_default_but_for_which_the_value_origin_was_undefined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Undefined;
         _parameter.ValueOrigin.Method = ValueOriginDeterminationMethods.Undefined;
         _parameter.IsDefault = true;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayValue(_parameter, 5, _simulation);
      }

      [Observation]
      public void should_update_the_parameter_value()
      {
         _parameter.Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_update_the_value_origin_to_unknown()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
      }

      [Observation]
      public void should_update_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_the_display_unit_of_a_parameter_in_a_simulation_that_is_default_but_for_which_the_value_origin_was_undefined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Undefined;
         _parameter.ValueOrigin.Method = ValueOriginDeterminationMethods.Undefined;
         _parameter.IsDefault = true;
         _parameter.DisplayUnit = null;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayUnit(_parameter, _parameter.Dimension.DefaultUnit, _simulation);
      }

      [Observation]
      public void should_update_the_display_unit()
      {
         _parameter.DisplayUnit.ShouldBeEqualTo(_parameter.Dimension.DefaultUnit);
      }

      [Observation]
      public void should_update_the_value_origin_to_unknown()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
      }

      [Observation]
      public void should_update_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_the_value_of_a_parameter_in_a_building_block_that_is_not_default_and_for_which_the_value_origin_was_defined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Internet;
         _parameter.IsDefault = false;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayValue(_parameter, 5, _buildingBlock);
      }

      [Observation]
      public void should_update_the_parameter_value()
      {
         _parameter.Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_not_update_the_value_origin()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Internet);
      }
   }

   public class When_updating_the_value_of_a_parameter_in_a_building_block_that_is_default_but_for_which_the_value_origin_was_defined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Internet;
         _parameter.IsDefault = true;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayValue(_parameter, 5, _buildingBlock);
      }

      [Observation]
      public void should_update_the_parameter_value()
      {
         _parameter.Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_update_the_value_origin()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
         _parameter.ValueOrigin.Method.ShouldBeEqualTo(ValueOriginDeterminationMethods.Undefined);
      }

      [Observation]
      public void should_update_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_the_value_of_a_parameter_in_a_building_block_that_is_default_but_for_which_the_value_origin_was_undefined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Undefined;
         _parameter.ValueOrigin.Method = ValueOriginDeterminationMethods.Undefined;
         _parameter.IsDefault = true;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayValue(_parameter, 5, _buildingBlock);
      }

      [Observation]
      public void should_update_the_parameter_value()
      {
         _parameter.Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_update_the_value_origin_to_unknown()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
      }

      [Observation]
      public void should_update_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_the_display_unit_of_a_parameter_in_a_building_block_that_is_default_but_for_which_the_value_origin_was_undefined : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.ValueOrigin.Source = ValueOriginSources.Undefined;
         _parameter.ValueOrigin.Method = ValueOriginDeterminationMethods.Undefined;
         _parameter.IsDefault = true;
         _parameter.DisplayUnit = null;
      }

      protected override void Because()
      {
         _result = sut.SetQuantityDisplayUnit(_parameter, _parameter.Dimension.DefaultUnit, _buildingBlock);
      }

      [Observation]
      public void should_update_the_display_unit()
      {
         _parameter.DisplayUnit.ShouldBeEqualTo(_parameter.Dimension.DefaultUnit);
      }

      [Observation]
      public void should_update_the_value_origin_to_unknown()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
      }

      [Observation]
      public void should_update_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_default_state_and_value_origin_for_a_quantity_that_is_not_default : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.IsDefault = false;
      }

      protected override void Because()
      {
         _result = sut.UpdateDefaultStateAndValueOriginFor(_parameter, _buildingBlock);
      }

      [Observation]
      public void should_return_an_empty_macro_command()
      {
         _result.IsEmptyMacro().ShouldBeTrue();
      }
   }

   public class When_updating_default_state_and_value_origin_for_a_quantity_that_is_default_with_defined_value_origin : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.IsDefault = true;
         _parameter.ValueOrigin.Source = ValueOriginSources.Database;
      }

      protected override void Because()
      {
         _result = sut.UpdateDefaultStateAndValueOriginFor(_parameter, _buildingBlock);
      }

      [Observation]
      public void should_return_a_macro_command_containing_only_the_default_command_and_the_update_command()
      {
         var macro = _result.DowncastTo<MoBiMacroCommand>();
         macro.Count.ShouldBeEqualTo(2);
         macro.All().ElementAt(0).ShouldBeAnInstanceOf<SetParameterDefaultStateInBuildingBlockCommand>();
         macro.All().ElementAt(1).ShouldBeAnInstanceOf<UpdateValueOriginInBuildingBlockCommand>();
      }

      [Observation]
      public void should_have_set_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }
   }

   public class When_updating_default_state_and_value_origin_for_a_quantity_that_is_default_with_undefined_value_origin : concern_for_QuantityTask
   {
      protected override void Context()
      {
         base.Context();
         _parameter.IsDefault = true;
         _parameter.ValueOrigin.Source = ValueOriginSources.Undefined;
      }

      protected override void Because()
      {
         _result = sut.UpdateDefaultStateAndValueOriginFor(_parameter, _buildingBlock);
      }

      [Observation]
      public void should_return_a_macro_command_containing_the_set_default_command_and_the_update_value_origin_command()
      {
         var macro = _result.DowncastTo<MoBiMacroCommand>();
         macro.Count.ShouldBeEqualTo(2);
         macro.All().ElementAt(0).ShouldBeAnInstanceOf<SetParameterDefaultStateInBuildingBlockCommand>();
         macro.All().ElementAt(1).ShouldBeAnInstanceOf<UpdateValueOriginInBuildingBlockCommand>();
      }

      [Observation]
      public void should_have_set_the_default_flag_to_false()
      {
         _parameter.IsDefault.ShouldBeFalse();
      }

      [Observation]
      public void should_have_updated_the_value_origin()
      {
         _parameter.ValueOrigin.Source.ShouldBeEqualTo(ValueOriginSources.Unknown);
      }
   }

   public class When_updating_the_value_origin_of_a_quantity_defined_in_a_simulation : concern_for_QuantityTask
   {
      private IQuantity _quantity;
      private ValueOrigin _newValueOrigin;
      private IMoBiCommand _synchronizeCommand;

      protected override void Context()
      {
         base.Context();
         _synchronizeCommand = A.Fake<IMoBiCommand>();
         _quantity = new Parameter();
         _newValueOrigin = new ValueOrigin {Method = ValueOriginDeterminationMethods.InVitro};
         A.CallTo(() => _quantitySynchronizer.Synchronize(_quantity, _simulation)).Returns(_synchronizeCommand);
      }

      protected override void Because()
      {
         _result = sut.UpdateQuantityValueOriginInSimulation(_quantity, _newValueOrigin, _simulation);
      }

      [Observation]
      public void should_update_the_value_origin_in_the_quantity()
      {
         _quantity.ValueOrigin.ShouldBeEqualTo(_newValueOrigin);
      }

      [Observation]
      public void should_synchronize_the_value_origin_in_the_corresponding_blocks()
      {
         var macroComamnd = _result.DowncastTo<MoBiMacroCommand>();
         macroComamnd.Count.ShouldBeEqualTo(3);
         macroComamnd.All().ElementAt(0).ShouldBeEqualTo(_synchronizeCommand);
         macroComamnd.All().ElementAt(2).ShouldBeEqualTo(_synchronizeCommand);
      }
   }
}