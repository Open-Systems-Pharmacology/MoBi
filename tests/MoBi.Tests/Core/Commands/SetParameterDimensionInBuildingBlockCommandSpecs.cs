using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetParameterDimensionInBuildingBlockCommand : ContextSpecification<SetParameterDimensionInBuildingBlockCommand>
   {
      protected IParameter _parameter;
      protected IDimension _newDimension;
      private IBuildingBlock _buildingBlock;
      protected IDimension _oldDimension;
      private IFormula _formula;
      protected IMoBiContext _context;
      protected Unit _oldDisplayUnit;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _formula = new ConstantFormula(20);
         _newDimension= A.Fake<IDimension>();
         _oldDimension= A.Fake<IDimension>();
         _buildingBlock= A.Fake<IBuildingBlock>();
         _oldDisplayUnit = A.Fake<Unit>();
         _parameter = new Parameter().WithDimension(_oldDimension).WithId("id").WithDisplayUnit(_oldDisplayUnit); ;
         _parameter.Formula = _formula;
         sut = new SetParameterDimensionInBuildingBlockCommand(_parameter,_newDimension,_buildingBlock);
      }
   }

   public class When_setting_new_dimension_for_parameter_without_right_hand_side : concern_for_SetParameterDimensionInBuildingBlockCommand
   {
      private Unit _newDisplayUnit;

      protected override void Context()
      {
         base.Context();
         _newDisplayUnit = A.Fake<Unit>();
         var displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         A.CallTo(() => _context.Resolve<IDisplayUnitRetriever>()).Returns(displayUnitRetriever);
         A.CallTo(displayUnitRetriever).WithReturnType<Unit>().Returns(_newDisplayUnit);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void dimension_of_parameter_changes()
      {
         _parameter.Dimension.ShouldBeEqualTo(_newDimension);
      }

   }

   public class When_setting_new_dimension_for_parameter_with_right_hand_side : concern_for_SetParameterDimensionInBuildingBlockCommand
   {
      private IDimension _newRightHandSideDimension;
      private Unit _newDisplayUnit;

      protected override void Context()
      {
         base.Context();
         _newRightHandSideDimension = A.Fake<IDimension>();
         _newDisplayUnit = A.Fake<Unit>();
         _parameter.RHSFormula= A.Fake<IFormula>();
         A.CallTo(() => _context.DimensionFactory.GetOrAddRHSDimensionFor(_newDimension)).Returns(_newRightHandSideDimension);
         var displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         A.CallTo(() => _context.Resolve<IDisplayUnitRetriever>()).Returns(displayUnitRetriever);
         A.CallTo(displayUnitRetriever).WithReturnType<Unit>().Returns(_newDisplayUnit);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void dimension_of_parameter_changes()
      {
         _parameter.Dimension.ShouldBeEqualTo(_newDimension);
      }

      [Observation]
      public void dimension_of_formula_should_change()
      {
         _parameter.Formula.Dimension.ShouldBeEqualTo(_newDimension);
      }

      [Observation]
      public void right_hand_side_dimension_should_change()
      {
         _parameter.RHSFormula.Dimension.ShouldBeEqualTo(_newRightHandSideDimension);
      }

      [Observation]
      public void parameter_display_unit_should_change()
      {
         _parameter.DisplayUnit.ShouldBeEqualTo(_newDisplayUnit);
      }
   }

   public class When_reverting_command_to_update_dimension_on_parameter : concern_for_SetParameterDimensionInBuildingBlockCommand
   {
      private IDimension _newRightHandSideDimension;
      private Unit _newDisplayUnit;

      protected override void Context()
      {
         base.Context();
         _newRightHandSideDimension = A.Fake<IDimension>();
         _newDisplayUnit = A.Fake<Unit>();
         A.CallTo(() => _context.DimensionFactory.GetOrAddRHSDimensionFor(_newDimension)).Returns(_newRightHandSideDimension);
         var displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         A.CallTo(() => _context.Resolve<IDisplayUnitRetriever>()).Returns(displayUnitRetriever);
         A.CallTo(displayUnitRetriever).WithReturnType<Unit>().Returns(_newDisplayUnit);

         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void dimension_should_be_reverted()
      {
         _parameter.Dimension.ShouldBeEqualTo(_oldDimension);
      }

      [Observation]
      public void parameter_display_unit_should_be_reverted()
      {
         _parameter.DisplayUnit.ShouldBeEqualTo(_oldDisplayUnit);
      }
   }

}	