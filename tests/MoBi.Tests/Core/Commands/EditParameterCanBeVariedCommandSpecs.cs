using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterCanBeVariedCommand : ContextSpecification<EditParameterCanBeVariedCommand>
   {
      protected IParameter _parameter;
      protected bool _newValue;
      private MoBiReactionBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _parameter = new Parameter().WithId("Para");
         _newValue = true;
         _parameter.CanBeVaried = false;
         _buildingBlock = new MoBiReactionBuildingBlock();
         sut = new EditParameterCanBeVariedCommand(_parameter, _newValue, _buildingBlock);
      }
   }

   internal class When_asking_for_inverse_command_of_a_EditParameterCanBeVariedCommand : concern_for_EditParameterCanBeVariedCommand
   {
      private ICommand<IMoBiContext> _result;
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
      }

      protected override void Because()
      {
         _result = sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_return_an_EditParameterIsVariableCommand()
      {
         _result.ShouldBeAnInstanceOf<EditParameterCanBeVariedCommand>();
      }

      [Observation]
      public void should_have_switched_the_new_and_old_value()
      {
         _parameter.CanBeVaried.ShouldBeFalse();
      }
   }

   internal class When_executing_an_EditParameterIsVaraiableCommand : concern_for_EditParameterCanBeVariedCommand
   {
      protected override void Because()
      {
         sut.Execute(A.Fake<IMoBiContext>());
      }

      [Observation]
      public void should_set_can_be_varied_to_new_value()
      {
         _parameter.CanBeVaried.ShouldBeEqualTo(_newValue);
      }
   }
}