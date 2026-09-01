using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveSelectedBuildingBlockFromModuleConfigurationCommand<T> : RemoveBuildingBlockFromModuleCommand<T> where T : class, IBuildingBlock
   {
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;

      public RemoveSelectedBuildingBlockFromModuleConfigurationCommand(T buildingBlock, ModuleConfiguration moduleConfiguration, IMoBiSimulation simulation) : base(buildingBlock, moduleConfiguration.Module)
      {
         _simulation = simulation;
         _simulationId = simulation.Id;
      }

      private ModuleConfiguration moduleConfiguration => _simulation.Configuration.ModuleConfigurations.First(x => Equals(x.Module, _existingModule));

      protected override void DoExecute(IMoBiContext context)
      {
         base.DoExecute(context);
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
         return new AddSelectedBuildingBlockToModuleConfigurationCommand<T>(_buildingBlock, moduleConfiguration, _simulation).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _simulation = null;
      }
   }
}
