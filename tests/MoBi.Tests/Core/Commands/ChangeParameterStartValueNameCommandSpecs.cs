using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeParameterStartValueNameCommand : ContextSpecification<ChangeParameterStartValueNameCommand>
   {
      protected IMoBiContext _context;
      protected ParameterStartValue _parameterStartValue;
      protected ParameterStartValuesBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _parameterStartValue = new ParameterStartValue {Path = new ObjectPath("Path1", "Path2", "Name")};

         _buildingBlock = new ParameterStartValuesBuildingBlock {_parameterStartValue};
         sut = new ChangeParameterStartValueNameCommand(_buildingBlock, _parameterStartValue.Path, "Name2");

         A.CallTo(() => _context.Get<ParameterStartValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class When_changing_name_of_parameter_start_value : concern_for_ChangeParameterStartValueNameCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void name_of_start_value_has_been_changed()
      {
         _parameterStartValue.ParameterName.ShouldBeEqualTo("Name2");
      }

      [Observation]
      public void name_of_start_value_becomes_key_for_building_block()
      {
         _buildingBlock[_parameterStartValue.Path].ShouldNotBeNull();
      }
   }

   public class When_reverting_command_for_rename_of_parameter_start_value : concern_for_ChangeParameterStartValueNameCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void name_change_should_be_reverted()
      {
         _parameterStartValue.ParameterName.ShouldBeEqualTo("Name");
      }
   }
}