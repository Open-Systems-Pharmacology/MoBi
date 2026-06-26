using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter;

public interface ISimulationUpdatePresenter : IDisposablePresenter,
   IListener<SimulationConfigurationStartedEvent>,
   IListener<SimulationConfigurationSucceededEvent>,
   IListener<SimulationConfigurationFailedEvent>
{

   /// <summary>
   /// Updates the given <paramref name="simulationsToUpdate"/> from their building blocks in parallel, showing per-simulation progress in a
   /// modal dialog. Blocks until the dialog is dismissed.
   /// </summary>
   /// <returns>The (already executed) command describing the update</returns>
   ICommand Start(IReadOnlyList<IMoBiSimulation> simulationsToUpdate);

   void Cancel();
}

public class SimulationUpdatePresenter : AbstractDisposablePresenter<ISimulationUpdateView, ISimulationUpdatePresenter>, ISimulationUpdatePresenter
{
   private readonly IMoBiContext _context;
   private readonly IEventPublisher _eventPublisher;
   private readonly ISimulationUpdateTask _simulationUpdateTask;
   private readonly ISimulationToSimulationUpdateStatusDTOMapper _statusMapper;
   private readonly Cache<string, SimulationUpdateStatusDTO> _statusDTOsCache = new(getKey: x => x.SimulationId);
   private CancellationTokenSource _cancellationTokenSource;

   public SimulationUpdatePresenter(ISimulationUpdateView view,
      IMoBiContext context,
      IEventPublisher eventPublisher,
      ISimulationUpdateTask simulationUpdateTask,
      ISimulationToSimulationUpdateStatusDTOMapper statusMapper) : base(view)
   {
      _context = context;
      _eventPublisher = eventPublisher;
      _simulationUpdateTask = simulationUpdateTask;
      _statusMapper = statusMapper;
      _view.AttachPresenter(this);
   }

   public ICommand Start(IReadOnlyList<IMoBiSimulation> simulationsToUpdate)
   {
      var macroCommand = new MoBiMacroCommand
      {
         ObjectType = ObjectTypes.Simulation,
         CommandType = AppConstants.Commands.UpdateCommand
      };

      _context.PublishEvent(new ClearNotificationsEvent(MessageOrigin.Simulation));

      _statusDTOsCache.Clear();
      _statusDTOsCache.AddRange(simulationsToUpdate.MapAllUsing(_statusMapper));
      _view.BindTo(_statusDTOsCache);

      _cancellationTokenSource = new CancellationTokenSource();

      _simulationUpdateTask
         .ConfigureSimulationsInParallel(simulationsToUpdate, _cancellationTokenSource.Token)
         .ContinueWith(configurationTask => applyConfigurations(configurationTask.Result, macroCommand), TaskScheduler.FromCurrentSynchronizationContext());

      _view.Caption = AppConstants.Captions.UpdatingSimulation;
      _view.Display();

      return macroCommand;
   }

   public void Cancel() => _cancellationTokenSource?.Cancel();

   public void Handle(SimulationConfigurationStartedEvent eventToHandle) => updateStatus(eventToHandle.Simulation.Id, RunStatus.Running);

   public void Handle(SimulationConfigurationSucceededEvent eventToHandle) => updateStatus(eventToHandle.Simulation.Id, RunStatus.Created);

   public void Handle(SimulationConfigurationFailedEvent eventToHandle) => updateStatus(eventToHandle.Simulation.Id, RunStatus.Faulted);

   protected override void Cleanup()
   {
      _eventPublisher.RemoveListener(this);
      base.Cleanup();
   }

   private void applyConfigurations(IReadOnlyList<ModelCreationAndValidationResult> results, MoBiMacroCommand macroCommand)
   {
      try
      {
         results.Each(result => applyConfiguration(result, macroCommand));
      }
      finally
      {
         macroCommand.Description = AppConstants.Commands.ConfigureSimulationsDescription(macroCommand.Count);
         _view.ShowCompleted();
      }
   }

   private void applyConfiguration(ModelCreationAndValidationResult result, MoBiMacroCommand macroCommand)
   {
      var command = createCommand(result);
      macroCommand.Add(command);

      if (!(command is MoBiEmptyCommand))
         updateStatus(result.Simulation.Id, RunStatus.RanToCompletion);
      else
      {
         var canceledBeforeConfiguring = !result.WasConfigured && _cancellationTokenSource.IsCancellationRequested;
         updateStatus(result.Simulation.Id, canceledBeforeConfiguring ? RunStatus.Canceled : RunStatus.Faulted);
      }
   }

   private ICommand createCommand(ModelCreationAndValidationResult result)
   {
      if (!result.IsValid)
         return new MoBiEmptyCommand();

      var updateSimulationCommand = new UpdateSimulationCommand(result.Simulation, result.CreationResult.Model, result.CreationResult.SimulationBuilder.EntitySources, result.ClonedConfiguration);
      updateSimulationCommand.RunCommand(_context);
      return updateSimulationCommand;
   }

   private void updateStatus(string simulationId, RunStatus status)
   {
      if (!_statusDTOsCache.Contains(simulationId))
         return;

      var statusDTO = _statusDTOsCache[simulationId];
      statusDTO.Status = status;
      _view.RefreshData();
   }
}