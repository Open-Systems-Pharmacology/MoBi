using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class ModuleContentChangedCommand<T> : MoBiReversibleCommand, IWillConvertPKSimModuleToExtensionModule where T : class, IBuildingBlock
   {
      protected Module _existingModule;
      private readonly string _existingModuleId;
      protected T _buildingBlock;
      protected bool _newPkSimModuleState;
      private readonly bool _oldPKSimModuleState;

      protected ModuleContentChangedCommand(T buildingBlock, Module existingModule)
      {
         _existingModule = existingModule;
         _existingModuleId = existingModule.Id;
         _buildingBlock = buildingBlock;
         _oldPKSimModuleState = _existingModule.IsPKSimModule;
         _newPkSimModuleState = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         DoExecute(context);
         SetPKSimModuleState();
         RaiseEvents(context);
      }

      protected abstract void RaiseEvents(IMoBiContext context);

      protected abstract void DoExecute(IMoBiContext context);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _existingModule = context.Get<Module>(_existingModuleId);
      }

      protected void PublishSimulationStatusChangedEvents(Module changedModule, IMoBiContext context)
      {
         var affectedSimulations = context.CurrentProject.SimulationsUsing(changedModule);
         affectedSimulations.Each(x => context.PublishEvent(new SimulationStatusChangedEvent(x)));
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
         _existingModule = null;
      }

      public bool WillConvertPKSimModuleToExtensionModule => Module.IsPKSimModule;
      public Module Module => _existingModule;

      protected void SetPKSimModuleState()
      {
         _existingModule.IsPKSimModule = _newPkSimModuleState;
      }

      public void WithNewPKSimModuleStateFrom(ModuleContentChangedCommand<T> newPKSimModuleState)
      {
         _newPkSimModuleState = newPKSimModuleState._oldPKSimModuleState;
      }
   }
}