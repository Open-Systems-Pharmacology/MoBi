using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_ResetQuantityValueInSimulationCommand : ContextSpecification<ResetQuantityValueInSimulationCommand>
   {
      protected IQuantity _quantity;
      protected IMoBiSimulation _simulation;
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _quantity = new Parameter { IsFixedValue = true };
         _simulation = new MoBiSimulation
         {
            BuildConfiguration = new MoBiBuildConfiguration()
         };

         sut = new ResetQuantityValueInSimulationCommand(_quantity, _simulation);
      }
   }

   public class When_resetting_the_quantity_value_in_a_simulation : concern_for_ResetQuantityValueInSimulationCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_quantity_should_not_be_fixed()
      {
         _quantity.IsFixedValue.ShouldBeFalse();
      }
   }
}
