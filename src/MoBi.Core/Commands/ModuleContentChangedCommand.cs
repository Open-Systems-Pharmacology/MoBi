using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
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
         var projectRetriever = context.Resolve<IMoBiProjectRetriever>();
         var affectedSimulations = projectRetriever.Current.SimulationsUsing(changedModule);
         affectedSimulations.Each(x => refreshSimulation(x, context));
      }

      private void refreshSimulation(IMoBiSimulation simulation, IMoBiContext context)
      {
         context.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
         _existingModule = null;
      }
   }
}