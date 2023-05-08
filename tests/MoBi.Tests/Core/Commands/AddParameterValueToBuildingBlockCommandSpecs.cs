using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddParameterValueToBuildingBlockCommand : ContextSpecification<AddParameterValueToBuildingBlockCommand>
   {
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected ParameterValue _psv;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ParameterValuesBuildingBlock();

         _psv = new ParameterValue {Path = new ObjectPath("path1"), Value = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1)};
         sut = new AddParameterValueToBuildingBlockCommand(_buildingBlock, _psv);
         A.CallTo(() => _context.Get<PathAndValueEntityBuildingBlock<ParameterValue>>(A<string>._)).Returns(_buildingBlock);
      }
   }

   public class after_successful_parameter_value_import : concern_for_AddParameterValueToBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void parameter_start_value_added_to_building_block()
      {
         _buildingBlock.ShouldContain(_psv);
      }
   }

   public class inverse_command_removes_parameter_value_from_building_block : concern_for_AddParameterValueToBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void parameter_start_value_should_be_removed_from_building_block()
      {
         _buildingBlock.ShouldBeEmpty();
      }
   }
}