using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand<T> : RemoveBuildingBlockFromModuleCommand<T> where T : class, IBuildingBlock
   {
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;

      public RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand(T buildingBlock, IMoBiSimulation simulation) : base(buildingBlock, simulation.Configuration.ModuleConfigurations.Last().Module)
      {
         _simulation = simulation;
         _simulationId = simulation.Id;
      }

      protected override void DoExecute(IMoBiContext context)
      {
         base.DoExecute(context);
         var moduleConfiguration = _simulation.Configuration.ModuleConfigurations.Last();
         switch (_buildingBlock)
         {
            case InitialConditionsBuildingBlock _:
               moduleConfiguration.SelectedInitialConditions = null;
               break;
            case ParameterValuesBuildingBlock _:
               moduleConfiguration.SelectedParameterValues = null;
               break;
         }
      }

      protected override void RaiseEvents(IMoBiContext context)
      {
         context.PublishEvent(new SimulationReloadEvent(_simulation));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _simulation = context.Get<IMoBiSimulation>(_simulationId);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddSelectedBuildingBlockToLastModuleConfigurationCommand<T>(_buildingBlock, _simulation);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _simulation = null;
      }
   }
}