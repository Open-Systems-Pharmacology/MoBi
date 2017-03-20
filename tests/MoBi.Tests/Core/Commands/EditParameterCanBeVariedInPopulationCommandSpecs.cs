using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_EditParameterCanBeVariedInPopulationCommandSpecs : ContextSpecification<EditParameterCanBeVariedInPopulationCommand>
   {
      protected IParameter _parameter;
      protected bool _newValue;
      protected IBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _parameter = new Parameter().WithId("Para");
         _newValue = true;
         _parameter.CanBeVariedInPopulation = false;
         _buildingBlock = A.Fake<IBuildingBlock>();
         sut = new EditParameterCanBeVariedInPopulationCommand(_parameter, _newValue, _buildingBlock);
      }
   }

   internal class When_restoring_execution_data : concern_for_EditParameterCanBeVariedInPopulationCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_context);
      }

      [Observation]
      public void should_retrieve_parameter_from_context()
      {
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).MustHaveHappened();
      }
   }

   internal class When_asking_for_inverse_command_of_a_EditParameterCanBeVariedInPopulationCommand : concern_for_EditParameterCanBeVariedInPopulationCommandSpecs
   {
      private IReversibleCommand<IMoBiContext> _result;
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context= A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<IParameter>(_parameter.Id)).Returns(_parameter);
      }

      protected override void Because()
      {
         _result = sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_return_an_EditParameterIsVaraiableInPopulationCommand()
      {
         _result.ShouldBeAnInstanceOf<EditParameterCanBeVariedInPopulationCommand>();
      }

      [Observation]
      public void should_have_switched_the_new_and_old_value()
      {
         _parameter.CanBeVariedInPopulation.ShouldBeFalse();
      }
   }

   internal class When_executing_an_EditParameterIsVaraiableInPopulationCommand : concern_for_EditParameterCanBeVariedInPopulationCommandSpecs
   {
      protected override void Because()
      {
         sut.Execute(A.Fake<IMoBiContext>());
      }

      [Observation]
      public void should_set_can_be_varied_in_population_to_new_value()
      {
         _parameter.CanBeVariedInPopulation.ShouldBeEqualTo(_newValue);
      }
   }
}