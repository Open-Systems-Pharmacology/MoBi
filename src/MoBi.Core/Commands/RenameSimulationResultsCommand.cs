using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Core.Commands
{
   public class RenameSimulationResultsCommand : MoBiReversibleCommand
   {
      private DataRepository _dataRepository;
      private IMoBiSimulation _simulation;
      private readonly string _newName;
      public string DataRepositoryId { get; private set; }
      public string SimulationId { get; private set; }
      public string OldName { get; private set; }

   
      public RenameSimulationResultsCommand(DataRepository dataRepository,IMoBiSimulation simulation,string newName)
      {
         _dataRepository = dataRepository;
         _simulation = simulation;
         _newName = newName;
         DataRepositoryId = _dataRepository.Id;
         SimulationId = simulation.Id;
         CommandType = AppConstants.Commands.RenameCommand;
         Description = AppConstants.Commands.RenameDescription(dataRepository, newName);

      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RenameSimulationResultsCommand(_dataRepository, _simulation, OldName).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _dataRepository = null;
         _simulation = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         OldName = _dataRepository.Name;
         _dataRepository.Name = _newName;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulation = context.Get<IMoBiSimulation>(SimulationId);
         _dataRepository = _simulation.HistoricResults[DataRepositoryId] ?? _simulation.Results;
      }
   }
}