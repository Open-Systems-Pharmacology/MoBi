using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterBuildModeInBuildingBlockCommand : ContextSpecification<EditParameterBuildModeInBuildingBlockCommand>
   {
      private IBuildingBlock _buildingBlock;
      protected IParameter _parameter;
      protected ParameterBuildMode _newBuildMode;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _parameter = new Parameter { BuildMode = ParameterBuildMode.Global };
         _newBuildMode = ParameterBuildMode.Local;
         _buildingBlock = A.Fake<IBuildingBlock>();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
         sut = new EditParameterBuildModeInBuildingBlockCommand(_newBuildMode, _parameter, _buildingBlock);
      }
   }

   public class When_reverting_the_update_of_a_build_mode_in_a_parameter : concern_for_EditParameterBuildModeInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_build_mode_should_be_reverted()
      {
         _parameter.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
      }
   }

   public class When_updating_the_build_mode_of_a_parameter : concern_for_EditParameterBuildModeInBuildingBlockCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_build_mode_should_be_changed()
      {
         _parameter.BuildMode.ShouldBeEqualTo(_newBuildMode);
      }
   }
}
