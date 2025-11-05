using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SetQuantityUnitInSimulationCommand : ContextSpecification<SetQuantityUnitInSimulationCommand>
   {
      protected IMoBiSimulation _simulation;
      protected IQuantity _quantity;
      protected IMoBiContext _context;
      protected Unit _oldUnit;
      protected Unit _newUnit;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _quantity = new Parameter();
         _newUnit = DomainHelperForSpecs.AmountDimension.Units.First();
         _oldUnit = DomainHelperForSpecs.AmountDimension.Units.Last();
         _quantity.DisplayUnit = _oldUnit;
         _simulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };
         A.CallTo(() => _context.Resolve<IQuantityValueInSimulationChangeTracker>()).Returns(DomainHelperForSpecs.QuantityValueChangeTracker(A.Fake<IEventPublisher>()));
         sut = new SetQuantityUnitInSimulationCommand(_quantity, _newUnit, _simulation);
      }
   }

   public class When_setting_the_quantity_unit_in_a_simulation : concern_for_SetQuantityUnitInSimulationCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_display_unit_should_be_updated()
      {
         _quantity.DisplayUnit.ShouldBeEqualTo(_newUnit);
      }
   }
}