using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class ModuleContentChangedCommand<T> : MoBiReversibleCommand where T : class, IBuildingBlock
   {
      protected Module _existingModule;
      private readonly string _existingModuleId;
      protected T _buildingBlock;

      protected ModuleContentChangedCommand(T buildingBlock, Module existingModule)
      {
         _existingModule = existingModule;
         _existingModuleId = existingModule.Id;
         _buildingBlock = buildingBlock;
      }

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
   }
}