using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddParameterStartValueToBuildingBlockCommand : ContextSpecification<AddParameterStartValueToBuildingBlockCommand>
   {
      protected IParameterStartValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected ParameterStartValue _psv;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ParameterStartValuesBuildingBlock();

         _psv = new ParameterStartValue {Path = new ObjectPath("path1"), StartValue = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1)};
         sut = new AddParameterStartValueToBuildingBlockCommand(_buildingBlock, _psv);
         A.CallTo(() => _context.Get<IStartValuesBuildingBlock<IParameterStartValue>>(A<string>._)).Returns(_buildingBlock);
      }
   }

   public class after_successful_parameter_start_value_import : concern_for_AddParameterStartValueToBuildingBlockCommand
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

   public class inverse_command_removes_parameter_start_value_from_building_block : concern_for_AddParameterStartValueToBuildingBlockCommand
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