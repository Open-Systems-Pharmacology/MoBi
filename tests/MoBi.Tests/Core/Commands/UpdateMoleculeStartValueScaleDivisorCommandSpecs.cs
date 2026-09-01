using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   abstract class concern_for_UpdateInitialConditionScaleDivisorCommand : ContextSpecification<UpdateInitialConditionScaleDivisorCommand>
   {
      protected double _oldScaleDivisor;
      protected double _newScaleDivisor;
      protected InitialCondition _startValue;
      protected InitialConditionsBuildingBlock _buildingBlock;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _oldScaleDivisor = 0;
         _startValue = new InitialCondition { Id = "startValueId", Name = "Drug"};
         _buildingBlock = new InitialConditionsBuildingBlock { Id = "id"};
         _buildingBlock.Add(_startValue);
         _context = A.Fake<IMoBiContext>();
         sut = new UpdateInitialConditionScaleDivisorCommand(_buildingBlock, _startValue, _newScaleDivisor, _oldScaleDivisor);

         // the initial condition is not registered in the object repository when it was added after its building block was registered
         A.CallTo(() => _context.Get<InitialCondition>(_startValue.Id)).Returns((InitialCondition)null);
         A.CallTo(() => _context.Get<ILookupBuildingBlock<InitialCondition>>(_buildingBlock.Id)).Returns(_buildingBlock);
      }
   }

   class When_executing_command_to_modify_scale_factor : concern_for_UpdateInitialConditionScaleDivisorCommand
   {
      protected override void Context()
      {
         _newScaleDivisor = 2;
         base.Context();
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_scale_factor()
      {
         _startValue.ScaleDivisor.ShouldBeEqualTo(_newScaleDivisor);
      }
   }

   class When_reverting_command_to_modify_scale_factor : concern_for_UpdateInitialConditionScaleDivisorCommand
   {
      protected override void Context()
      {
         _newScaleDivisor = 2;
         base.Context();
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void scale_factor_should_be_reverted_to_oringial_value()
      {
         _startValue.ScaleDivisor.ShouldBeEqualTo(_oldScaleDivisor);
      }
   }
}
