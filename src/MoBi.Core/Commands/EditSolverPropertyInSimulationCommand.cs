using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditSolverPropertyInSimulationCommand : SimulationChangeCommandBase
   {
      private readonly string _propertyName;
      private readonly object _newValue;
      private readonly object _oldValue;

      public EditSolverPropertyInSimulationCommand(string propertyName, object newValue, object oldValue, IMoBiSimulation simulation)
         : base(simulation.Solver, simulation)
      {
         _propertyName = propertyName;
         _newValue = newValue;
         _oldValue = oldValue;
         ObjectType = ObjectTypes.SolverProperty;
         CommandType = AppConstants.Commands.EditCommand;
         Description = AppConstants.Commands.EditSolverPropertyInSimulationDescription(propertyName, newValue.ToString(), simulation.Name);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditSolverPropertyInSimulationCommand(_propertyName, _oldValue, _newValue, _simulation).AsInverseFor(this);
      }

      protected override void DoExecute(IMoBiContext context)
      {
         var solver = _simulation.Solver;
         var propInfo = solver.GetType().GetProperty(_propertyName);
         propInfo.SetValue(solver, _newValue);
      }
   }
}