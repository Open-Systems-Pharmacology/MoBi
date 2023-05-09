using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveParameterValueFromBuildingBlockCommand : ContextSpecification<RemoveParameterValueFromBuildingBlockCommand>
   {
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected ParameterValue _psv;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ParameterValuesBuildingBlock();

         _psv = new ParameterValue{Path=new ObjectPath("path1"), Value = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1) };
         _buildingBlock.Add(_psv);
         sut = new RemoveParameterValueFromBuildingBlockCommand(_buildingBlock, _psv.Path);

         A.CallTo(() => _context.Deserialize<ParameterValue>(A<byte[]>._)).Returns(_psv);
         A.CallTo(() => _context.Get<PathAndValueEntityBuildingBlock<ParameterValue>>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class removing_parameter_value_from_path : concern_for_RemoveParameterValueFromBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void parameter_start_value_should_not_be_contained_in_building_block()
      {
         _buildingBlock.ShouldBeEmpty();
      }
   }

   public class When_retrieving_the_inverse_command_of_remove_parameter : concern_for_RemoveParameterValueFromBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void msv_should_be_added_to_building_block()
      {
         _buildingBlock.ShouldContain(_psv);
      }
   }
}
