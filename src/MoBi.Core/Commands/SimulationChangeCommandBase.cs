using MoBi.Core.Domain.Model;
using MoBi.Core.Services;

namespace MoBi.Core.Commands
{
   public abstract class SimulationChangeCommandBase : MoBiReversibleCommand
   {
      protected IMoBiSimulation _simulation;
      protected bool _changed;
      private object _changedObject;
      public bool Increment { get; set; }
      public bool WasChanged { get; set; }
      private readonly string _simulationId;

      protected SimulationChangeCommandBase(object changedObject, IMoBiSimulation simulation)
      {
         _changedObject = changedObject;
         _simulation = simulation;
         _simulationId = simulation.Id;
         //default settings that will be changed in inverse commands
         Increment = true;
         _changed = true;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         DoExecute(context);
         manageSimulationChange(context);
      }

      private void manageSimulationChange(IMoBiContext context)
      {
         WasChanged = _simulation.HasChanged;
         _simulation.HasChanged = _changed;
         _simulation.MarkResultsOutOfDate();
         var buildConfigurationUpdater = context.Resolve<IBuildConfigurationUpdater>();
         buildConfigurationUpdater.UpdateBuildingConfiguration(_changedObject, _simulation, Increment);
      }

      protected abstract void DoExecute(IMoBiContext context);

      protected override void ClearReferences()
      {
         _changedObject = null;
         _simulation = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulation = context.Get<IMoBiSimulation>(_simulationId);
      }
   }
}