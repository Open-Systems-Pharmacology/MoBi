using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Exchange;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveSimulationCommand : ContextWithLoadedProject
   {
      protected IMoBiContext _context;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         var simulationTransfer = LoadPKML<SimulationTransfer>("Sim");
         _context = IoC.Resolve<IMoBiContext>();
         _simulation = simulationTransfer.Simulation as IMoBiSimulation;
         _context.Register(_simulation);
         _context.Get<IFormula>(_simulation.Model.Root.GetAllChildren<IUsingFormula>(x => x.Formula.IsCachable()).FirstOrDefault().Formula.Id).ShouldNotBeNull();
      }
   }

   public class When_executing_an_remove_simulation_command : concern_for_RemoveSimulationCommand
   {
      protected override void Because()
      {
         new RemoveSimulationCommand(_simulation).Execute(_context);
      }

      [Observation]
      public void should_remove_all_ids_from_context()
      {
         _simulation.Model.Root.GetAllChildren<IUsingFormula>(x => x.Formula.IsCachable())
            .Any(x => _context.ObjectRepository.ContainsObjectWithId(x.Formula.Id)).ShouldBeFalse();

         _simulation.Model.Neighborhoods.GetAllChildren<IUsingFormula>(x => x.Formula.IsCachable())
            .Any(x => _context.ObjectRepository.ContainsObjectWithId(x.Formula.Id)).ShouldBeFalse();
      }
   }
}