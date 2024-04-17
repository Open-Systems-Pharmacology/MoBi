using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetParameterValueWithUnitCommandSpecs : ContextSpecification<PathAndValueEntityValueOrUnitChangedCommand<ParameterValue, ParameterValuesBuildingBlock>>
   {
      protected ParameterValue _psv;
      protected double _newValue =2.2;
      protected Unit _newUnit;
      protected IDimension _dimension;
      protected double _oldValue=1.1;
      protected Unit _oldUnit;
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected IBuildingBlockVersionUpdater _buildingBlockVersionUpdater;

      protected override void Context()
      {
         _psv = new ParameterValue{ Value = _oldValue, Dimension = _dimension};
         _dimension = A.Fake<IDimension>();
         _newUnit = new Unit("Neu",2,0);
         _context = A.Fake<IMoBiContext>();
         _buildingBlockVersionUpdater = A.Fake<IBuildingBlockVersionUpdater>();
         _buildingBlock = new ParameterValuesBuildingBlock
         {
            Version = 1
         };
         A.CallTo(() => _dimension.Unit("Neu")).Returns(_newUnit);
         _oldUnit = new Unit("Old",1,0);
         _psv.DisplayUnit = _oldUnit;
         _psv.Dimension = _dimension;

         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_oldUnit,_oldValue)).Returns(_oldValue);
         A.CallTo(() => _dimension.UnitValueToBaseUnitValue(_newUnit,_newValue)).Returns(_newValue);
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(_buildingBlockVersionUpdater);
         A.CallTo(() => _context.Get<ParameterValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         
         _buildingBlock.Add(_psv);
      }
   }

   public class When_executing_a_set_parameter_value_with_unit_command : concern_for_SetParameterValueWithUnitCommandSpecs
   {
      protected override void Context()
      {
         base.Context();
         sut = new PathAndValueEntityValueOrUnitChangedCommand<ParameterValue, ParameterValuesBuildingBlock>(_psv, _newValue, _newUnit, _buildingBlock);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_change_StartValue_and_display_unit_of_the_parameter_start_value()
      {
         _psv.DisplayUnit.ShouldBeEqualTo(_newUnit);
         _psv.Value.ShouldBeEqualTo(_newValue);
      }

      [Observation]
      public void should_have_set_parameter_start_values_building_block_to_changed()
      {
         A.CallTo(() => _buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_buildingBlock, true)).MustHaveHappened();
      }
   }

   public class When_reverting_to_distributed_from_constant_formula : concern_for_SetParameterValueWithUnitCommandSpecs
   {
      protected override void Context()
      {
         base.Context();
         _psv.DistributionType = DistributionType.Normal;
         sut = new PathAndValueEntityValueOrUnitChangedCommand<ParameterValue, ParameterValuesBuildingBlock>(_psv, _newValue, _newUnit, _buildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_change_the_parameter_to_be_distributed()
      {
         _psv.DistributionType.ShouldBeEqualTo(DistributionType.Normal);
      }
   }

   public class When_converting_a_distributed_path_and_value_entity: concern_for_SetParameterValueWithUnitCommandSpecs
   {
      protected override void Context()
      {
         base.Context();
         _psv.DistributionType = DistributionType.Normal;
         sut = new PathAndValueEntityValueOrUnitChangedCommand<ParameterValue, ParameterValuesBuildingBlock>(_psv, _newValue, _newUnit, _buildingBlock);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_change_the_parameter_to_be_non_distributed()
      {
         _psv.DistributionType.ShouldBeEqualTo(null);
      }
   }
}	