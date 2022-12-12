using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_UpdateValueOriginInStartValueCommand : ContextSpecification<UpdateValueOriginInStartValueCommand<ParameterStartValue>>
   {
      protected ParameterStartValue _parameterStartValue;
      protected ValueOrigin _newValueOrigin;
      protected IParameterStartValuesBuildingBlock _startValueBuildingBlock;
      protected IMoBiContext _context;
      protected ValueOrigin _originalValueOrigin;

      protected override void Context()
      {
         _parameterStartValue = new ParameterStartValue();
         _originalValueOrigin = new ValueOrigin
         {
            Source = ValueOriginSources.Publication,
            Method = ValueOriginDeterminationMethods.Assumption,
            Description = "BEFORE"
         };

         _newValueOrigin = new ValueOrigin
         {
            Description = "AFTER",
            Method = ValueOriginDeterminationMethods.InVitro,
            Source = ValueOriginSources.Internet
         };

         _parameterStartValue.UpdateValueOriginFrom(_originalValueOrigin);
         _startValueBuildingBlock = A.Fake<IParameterStartValuesBuildingBlock>().WithId("PSV BB");
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IStartValuesBuildingBlock<ParameterStartValue>>(_startValueBuildingBlock.Id)).Returns(_startValueBuildingBlock);
         sut = new UpdateValueOriginInStartValueCommand<ParameterStartValue>(_parameterStartValue, _newValueOrigin, _startValueBuildingBlock);
      }
   }

   public class When_executing_the_update_value_origin_in_start_value_command : concern_for_UpdateValueOriginInStartValueCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_update_the_underlying_start_value_value_origin()
      {
         _parameterStartValue.ValueOrigin.ShouldBeEqualTo(_newValueOrigin);
      }
   }

   public class When_reverting_the_execution_of_an_update_value_origin_for_start_value_command : concern_for_UpdateValueOriginInStartValueCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_revert_the_value_origin_to_its_original_state()
      {
         _parameterStartValue.ValueOrigin.ShouldBeEqualTo(_originalValueOrigin);
      }
   }
}