using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddSelectedBuildingBlockToModuleConfigurationCommand<T> : AddBuildingBlockToModuleCommand<T> where T : class, IBuildingBlock
   {
      private IMoBiSimulation _simulation;
      private readonly string _simulationId;

      public AddSelectedBuildingBlockToModuleConfigurationCommand(T buildingBlock, ModuleConfiguration moduleConfiguration, IMoBiSimulation simulation) : base(buildingBlock, moduleConfiguration.Module)
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
            case InitialConditionsBuildingBlock initialConditions:
               moduleConfiguration.SelectedInitialConditions = initialConditions;
               break;
            case ParameterValuesBuildingBlock parameterValues:
               moduleConfiguration.SelectedParameterValues = parameterValues;
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
         return new RemoveSelectedBuildingBlockFromModuleConfigurationCommand<T>(_buildingBlock, moduleConfiguration, _simulation).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _simulation = null;
      }
   }
}
