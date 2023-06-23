using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetCreateProcessRateParameterCommandSpecs : ContextSpecification<SetCreateProcessRateParameterCommand>
   {
      protected bool _newValue;
      protected bool _oldValue;
      protected ReactionBuilder _reaactionBuilder;

      protected override void Context()
      {
         _newValue = false;
         _oldValue = true;
         _reaactionBuilder = new ReactionBuilder();
         _reaactionBuilder.CreateProcessRateParameter = _oldValue;
         _reaactionBuilder.ProcessRateParameterPersistable = true;
         sut = new SetCreateProcessRateParameterCommand(_newValue, _reaactionBuilder, A.Fake<IBuildingBlock>());
      }
   }

   internal class When_executing_a_SetCreateProcessRateParameterCommand : concern_for_SetCreateProcessRateParameterCommandSpecs
   {
      protected override void Because()
      {
         sut.Execute(A.Fake<IMoBiContext>());
      }

      [Observation]
      public void should_set_CreateProcessRateParameter_to_new_Value()
      {
         _reaactionBuilder.CreateProcessRateParameter.ShouldBeFalse();
      }

      [Observation]
      public void should_set_the_plot_process_rate_parameter_to_false_if_the_create_process_rate_is_false()
      {
         _reaactionBuilder.ProcessRateParameterPersistable.ShouldBeFalse();
      }
   }
}