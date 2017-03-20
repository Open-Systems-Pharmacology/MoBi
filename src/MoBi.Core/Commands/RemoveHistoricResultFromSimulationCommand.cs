using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Core.Commands
{
   public class RemoveHistoricResultFromSimulationCommand : MoBiCommand
   {
      private DataRepository _dataRepository;
      private IMoBiSimulation _simulation;

      [Obsolete("For serialization")]
      public RemoveHistoricResultFromSimulationCommand()
      {
      }

      public RemoveHistoricResultFromSimulationCommand(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         _simulation = simulation;
         _dataRepository = dataRepository;
         CommandType = AppConstants.Commands.DeleteCommand;
         ObjectType = "Data";
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, dataRepository.Name, simulation.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _simulation.HistoricResults.Remove(_dataRepository.Id);
         context.PublishEvent(new RemovedDataEvent(_dataRepository));
      }

      protected override void ClearReferences()
      {
         _simulation = null;
         _dataRepository = null;
      }
   }

   public class ClearResultsCommand : MoBiCommand
   {
      private IMoBiSimulation _simulation;

      public ClearResultsCommand(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         CommandType = AppConstants.Commands.DeleteCommand;
         ObjectType = AppConstants.MoBiObjectTypes.Data;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var result = _simulation.Results;
         _simulation.Results = null;
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, result.Name, _simulation.Name);
         context.PublishEvent(new RemovedDataEvent(result));
      }

      protected override void ClearReferences()
      {
         _simulation = null;
      }
   }
}