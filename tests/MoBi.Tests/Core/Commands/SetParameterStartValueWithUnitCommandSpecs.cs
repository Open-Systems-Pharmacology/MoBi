using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;


namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetParameterStartValueWithUnitCommandSpecs : ContextSpecification<SetParameterStartValueWithUnitCommand>
   {
      protected IParameterStartValue _psv;
      protected double _newValue =2.2;
      protected Unit _newUnit;
      protected IDimension _dimension;
      protected double _oldValue=1.1;
      protected Unit _oldUnit;
      protected IParameterStartValuesBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _psv = new ParameterStartValue{StartValue = _oldValue, Dimension = _dimension};
         _dimension = A.Fake<IDimension>();
         _newUnit = new Unit("Neu",2,0);
         _buildingBlock = A.Fake<IParameterStartValuesBuildingBlock>();
         _buildingBlock.Version = 1;
         A.CallTo(() => _dimension.Unit("Neu")).Returns(_newUnit);
         _oldUnit = new Unit("Old",1,0);
         _psv.DisplayUnit = _oldUnit;
         _psv.Dimension = _dimension;
         A.CallTo(() => _dimension.BaseUnitValueToUnitValue(_oldUnit,_oldValue)).Returns(_oldValue);
         A.CallTo(() => _dimension.UnitValueToBaseUnitValue(_newUnit,_newValue)).Returns(_newValue);
         sut = new SetParameterStartValueWithUnitCommand(_psv, _newValue, _newUnit, _buildingBlock);
      }
   }

   public class When_executing_a_SetParameterStartValueWithUnitCommand : concern_for_SetParameterStartValueWithUnitCommandSpecs
   {
      private IMoBiContext _context;
      private IBuildingBlockVersionUpdater _buildingBlockVersionUpdater;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         _buildingBlockVersionUpdater= A.Fake<IBuildingBlockVersionUpdater>();
         A.CallTo(() => _context.Resolve<IBuildingBlockVersionUpdater>()).Returns(_buildingBlockVersionUpdater);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_change_StartValue_and_display_unit_of_the_parameter_start_value()
      {
         _psv.DisplayUnit.ShouldBeEqualTo(_newUnit);
         _psv.StartValue.ShouldBeEqualTo(_newValue);
      }

      [Observation]
      public void should_have_set_parameter_start_values_building_block_to_chnaged()
      {
         A.CallTo(() => _buildingBlockVersionUpdater.UpdateBuildingBlockVersion(_buildingBlock, true)).MustHaveHappened();
      }
   }

}	