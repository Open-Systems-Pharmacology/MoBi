using System;
using FakeItEasy;
using MoBi.Core.Domain;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core.Commands
{
   internal class concern_for_ClearOriginalQuantitiesTrackerCommand : ContextSpecification<ClearOriginalQuantitiesTrackerCommand>
   {
      protected MoBiSimulation _simulation;
      protected IMoBiContext _context;
      private readonly byte[] _bytes = Array.Empty<byte>();
      protected OriginalQuantityValue _deserializedOriginalQuantityValue;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _simulation = new MoBiSimulation();
         var originalQuantityValue = new OriginalQuantityValue();
         _deserializedOriginalQuantityValue = new OriginalQuantityValue();
         _simulation.AddOriginalQuantityValue(originalQuantityValue);
         sut = new ClearOriginalQuantitiesTrackerCommand(_simulation);

         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
         A.CallTo(() => _context.Serialize(originalQuantityValue)).Returns(_bytes);
         A.CallTo(() => _context.Deserialize<OriginalQuantityValue>(_bytes)).Returns(_deserializedOriginalQuantityValue);
      }
   }

   internal class When_clearing_the_original_quantity_values : concern_for_ClearOriginalQuantitiesTrackerCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_simulation_should_not_have_quantity_values()
      {
         _simulation.OriginalQuantityValues.ShouldBeEmpty();
      }
   }

   internal class When_reversing_clearing_the_original_quantity_values : concern_for_ClearOriginalQuantitiesTrackerCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_simulation_should_not_have_quantity_values()
      {
         _simulation.OriginalQuantityValues.ShouldContain(_deserializedOriginalQuantityValue);
      }
   }
}
