using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddSelectedBuildingBlockToLastModuleConfigurationCommand<T> : AddBuildingBlockToModuleCommand<T> where T : class, IBuildingBlock
   {
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;

      public AddSelectedBuildingBlockToLastModuleConfigurationCommand(T buildingBlock, IMoBiSimulation simulation) : base(buildingBlock, simulation.Configuration.ModuleConfigurations.Last().Module)
      {
         _simulation = simulation;
         _simulationId = simulation.Id;
      }

      protected override void DoExecute(IMoBiContext context)
      {
         base.DoExecute(context);
         switch (_buildingBlock)
         {
            case InitialConditionsBuildingBlock initialConditions:
               _simulation.Configuration.ModuleConfigurations.Last().SelectedInitialConditions = initialConditions;
               break;
            case ParameterValuesBuildingBlock parameterValues:
               _simulation.Configuration.ModuleConfigurations.Last().SelectedParameterValues = parameterValues;
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
         return new RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand<T>(_buildingBlock, _simulation);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _simulation = null;
      }
   }
}