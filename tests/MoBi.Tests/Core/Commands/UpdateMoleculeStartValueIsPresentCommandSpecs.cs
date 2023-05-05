using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateMoleculeStartValueIsPresentCommand : ContextSpecification<UpdateMoleculeStartValueIsPresentCommand>
   {
      protected InitialConditionsBuildingBlock _startValueBuildingBlock;
      protected InitialCondition _startValue;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _startValueBuildingBlock = new InitialConditionsBuildingBlock();
         _startValue = new InitialCondition { IsPresent = false };
         _startValueBuildingBlock.Add(_startValue);
         _context = A.Fake<IMoBiContext>();

         sut = new UpdateMoleculeStartValueIsPresentCommand(_startValueBuildingBlock, _startValue, true);
      }
   }

   public class When_updating_the_start_value_is_present_status : concern_for_UpdateMoleculeStartValueIsPresentCommand
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

   public class When_reverting_the_start_value_is_present_status : concern_for_UpdateMoleculeStartValueIsPresentCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<InitialCondition>(_startValue.Id)).Returns(_startValue);
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
