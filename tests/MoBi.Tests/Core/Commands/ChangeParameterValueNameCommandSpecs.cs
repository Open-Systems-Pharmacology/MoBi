using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ChangeParameterValueNameCommand : ContextSpecification<ChangeParameterValueNameCommand>
   {
      protected IMoBiContext _context;
      protected ParameterValue _parameterValue;
      protected ParameterValuesBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _parameterValue = new ParameterValue {Path = new ObjectPath("Path1", "Path2", "Name")};

         _buildingBlock = new ParameterValuesBuildingBlock {_parameterValue};
         sut = new ChangeParameterValueNameCommand(_buildingBlock, _parameterValue.Path, "Name2");

         A.CallTo(() => _context.Get<ParameterValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class changing_name_of_parameter_value : concern_for_ChangeParameterValueNameCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void name_of_start_value_has_been_changed()
      {
         _parameterValue.ParameterName.ShouldBeEqualTo("Name2");
      }

      [Observation]
      public void name_of_start_value_becomes_key_for_building_block()
      {
         _buildingBlock[_parameterValue.Path].ShouldNotBeNull();
      }
   }

   public class reverting_command_for_rename_of_parameter_value : concern_for_ChangeParameterValueNameCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void name_change_should_be_reverted()
      {
         _parameterValue.ParameterName.ShouldBeEqualTo("Name");
      }
   }
}