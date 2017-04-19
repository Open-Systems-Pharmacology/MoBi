using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core
{
   public abstract class concern_for_QuantityTask : ContextSpecification<IQuantityTask>
   {
      private IMoBiContext _context;
      private IQuantitySynchronizer _quantitySynchronizer;

      protected override void Context()
      {
         _quantitySynchronizer = A.Fake<IQuantitySynchronizer>();
         _context = A.Fake<IMoBiContext>();
         sut = new QuantityTask(_context, _quantitySynchronizer);
      }
   }

   public class When_resetting_the_value_of_a_quantity_in_a_simulation : concern_for_QuantityTask
   {
      private IParameter _parameter;
      private ICommand _result;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _parameter = new Parameter().WithFormula(new ExplicitFormula("1+2"));
         _parameter.Value = 5;
      }

      protected override void Because()
      {
         _result = sut.ResetQuantityValue(_parameter, _simulation);
      }

      [Observation]
      public void should_ensure_that_the_description_of_the_resulting_commmand_is_set()
      {
         string.IsNullOrEmpty(_result.Description).ShouldBeFalse();
      }

      [Observation]
      public void should_reset_the_value()
      {
         _parameter.Value.ShouldBeEqualTo(3);
         _parameter.IsFixedValue.ShouldBeFalse();
      }
   }
}