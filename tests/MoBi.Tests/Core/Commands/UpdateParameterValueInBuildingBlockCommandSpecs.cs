using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateParameterValueInBuildingBlockCommand : ContextSpecification<UpdateParameterValueInBuildingBlockCommand>
   {
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected ObjectPath _path;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _buildingBlock = new ParameterValuesBuildingBlock();

         _path = new ObjectPath(new[] { string.Format("path{0}", 1) });
         _buildingBlock.Add(new ParameterValue{Path = _path , Value = -1, DisplayUnit = new Unit("Dimensionless", 1.0, 1) });

         sut = new UpdateParameterValueInBuildingBlockCommand(_buildingBlock, _path, 1.0);
         A.CallTo(() => _context.Get<ParameterValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   public class When_updating_parameter_start_values : concern_for_UpdateParameterValueInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void command_fieds_updated()
      {
         sut.CommandType.ShouldBeEqualTo(AppConstants.Commands.UpdateCommand);
         sut.Description.ShouldBeEqualTo(AppConstants.Commands.UpdateParameterValue(_path, 1.0, _buildingBlock[_path].DisplayUnit));
         sut.ObjectType.ShouldBeEqualTo(ObjectTypes.ParameterValue);
      }

      [Observation]
      public void start_values_must_be_updated()
      {
         _buildingBlock.Each(psv => psv.Value.Value.ShouldBeGreaterThanOrEqualTo(0));
      }
   }

   public class When_retrieving_inverse : concern_for_UpdateParameterValueInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void reverses_updates_to_result_in_original_list()
      {
         _buildingBlock[_path].Value.Value.ShouldBeSmallerThan(0.0 + double.Epsilon);
      }
   }
}
