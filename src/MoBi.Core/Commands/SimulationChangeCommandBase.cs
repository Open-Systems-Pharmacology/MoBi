using MoBi.Core.Domain.Model;

namespace MoBi.Core.Commands
{
   public abstract class SimulationChangeCommandBase : MoBiReversibleCommand
   {
      protected IMoBiSimulation _simulation;
      protected bool _changed;
      public bool Increment { get; set; }
      public bool WasChanged { get; set; }
      private readonly string _simulationId;

      protected SimulationChangeCommandBase(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _simulationId = simulation.Id;
         //default settings that will be changed in inverse commands
         Increment = true;
         _changed = true;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         DoExecute(context);
         WasChanged = _simulation.HasChanged;
         _simulation.HasChanged = _changed;
         _simulation.MarkResultsOutOfDate();
      }

      protected abstract void DoExecute(IMoBiContext context);

      protected override void ClearReferences()
      {
         _simulation = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulation = context.Get<IMoBiSimulation>(_simulationId);
      }
   }
}