using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateInitialConditionIsPresentCommand : ContextSpecification<UpdateInitialConditionIsPresentCommand>
   {
      protected InitialConditionsBuildingBlock _startValueBuildingBlock;
      protected InitialCondition _startValue;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _startValueBuildingBlock = new InitialConditionsBuildingBlock { Id = "buildingBlockId" };
         _startValue = new InitialCondition { Name = "Drug", IsPresent = false };
         _startValueBuildingBlock.Add(_startValue);
         _context = A.Fake<IMoBiContext>();

         sut = new UpdateInitialConditionIsPresentCommand(_startValueBuildingBlock, _startValue, true);
      }
   }

   public class When_updating_the_start_value_is_present_status : concern_for_UpdateInitialConditionIsPresentCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_value_of_is_present_should_be_updated()
      {
         _startValue.IsPresent.ShouldBeTrue();
      }
   }

   public class When_reverting_the_start_value_is_present_status : concern_for_UpdateInitialConditionIsPresentCommand
   {
      protected override void Context()
      {
         base.Context();
         // the initial condition is not registered in the object repository when it was added after its building block was registered
         A.CallTo(() => _context.Get<InitialCondition>(_startValue.Id)).Returns((InitialCondition)null);
         A.CallTo(() => _context.Get<ILookupBuildingBlock<InitialCondition>>(_startValueBuildingBlock.Id)).Returns(_startValueBuildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_is_present_should_be_reverted()
      {
         _startValue.IsPresent.ShouldBeFalse();
      }
   }
}
