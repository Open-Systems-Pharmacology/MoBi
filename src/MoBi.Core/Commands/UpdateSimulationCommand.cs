using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class UpdateSimulationCommand : MoBiReversibleCommand
   {
      private IMoBiSimulation _simulationToUpdate;
      private IModel _newModel;
      private SimulationConfiguration _updatedSimulationConfiguration;
      private readonly bool _hasChanged;
      private readonly bool _newUntraceableChangesState;
      private readonly string _simulationId;
      private bool _wasChanged;
      private byte[] _simulationConfigurationSerialization;
      private byte[] _modelSerialization;
      private byte[] _simulationEntitySourceSerialization;
      private readonly bool _oldUntraceableChangesState;
      private IReadOnlyCollection<SimulationEntitySource> _newSimulationBuilderEntitySources;

      public UpdateSimulationCommand(IMoBiSimulation simulationToUpdate, IModel newModel, IReadOnlyCollection<SimulationEntitySource> newSimulationBuilderEntitySources, SimulationConfiguration updatedSimulationConfiguration)
         : this(simulationToUpdate, newModel, newSimulationBuilderEntitySources, updatedSimulationConfiguration, wasChanged: true, newUntraceableChangesState: false)
      {
      }

      private UpdateSimulationCommand(IMoBiSimulation simulationToUpdate, IModel newModel, IReadOnlyCollection<SimulationEntitySource> newSimulationBuilderEntitySources, SimulationConfiguration updatedSimulationConfiguration, bool wasChanged, bool newUntraceableChangesState)
      {
         _hasChanged = wasChanged;
         _newUntraceableChangesState = newUntraceableChangesState;
         _oldUntraceableChangesState = simulationToUpdate.HasUntraceableChanges;
         _simulationToUpdate = simulationToUpdate;
         _newModel = newModel;
         _newSimulationBuilderEntitySources = newSimulationBuilderEntitySources;
         _updatedSimulationConfiguration = updatedSimulationConfiguration;
         _simulationId = _simulationToUpdate.Id;
         ObjectType = ObjectTypes.Simulation;
         CommandType = AppConstants.Commands.UpdateCommand;
         Description = AppConstants.Commands.ConfigureSimulationDescription(_simulationToUpdate.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _modelSerialization = context.Serialize(_simulationToUpdate.Model);
         _simulationConfigurationSerialization = context.Serialize(_simulationToUpdate.Configuration);
         _simulationEntitySourceSerialization = context.Serialize(_simulationToUpdate.EntitySources);

         context.UnregisterSimulation(_simulationToUpdate);
         context.PublishEvent(new SimulationUnloadEvent(_simulationToUpdate));

         var oldIdCache = getEntityIdCache(_simulationToUpdate);

         _simulationToUpdate.Update(_updatedSimulationConfiguration, _newModel, _newSimulationBuilderEntitySources);

         updateReferencesToSimulation(context);

         if (_simulationToUpdate.DiagramModel != null)
            replaceDiagramModelNodeIds(_simulationToUpdate.DiagramModel, oldIdCache, getEntityIdCache(_simulationToUpdate));

         context.Register(_simulationToUpdate);
         _wasChanged = _simulationToUpdate.HasChanged;
         _simulationToUpdate.HasChanged = _hasChanged;

         // We need ToList since we are modifying the collection in the loop
         _simulationToUpdate.OriginalQuantityValues.ToList().Each(x => _simulationToUpdate.RemoveOriginalQuantityValue(x));
         _simulationToUpdate.HasUntraceableChanges = _newUntraceableChangesState;
         context.PublishEvent(new SimulationReloadEvent(_simulationToUpdate));
      }

      private void updateReferencesToSimulation(IMoBiContext context)
      {
         var simulationReferenceUpdater = context.Resolve<ISimulationReferenceUpdater>();
         simulationReferenceUpdater.SwapSimulationInParameterAnalysables(_simulationToUpdate, _simulationToUpdate);

         var simulationParameterOriginIdUpdater = context.Resolve<ISimulationParameterOriginIdUpdater>();
         simulationParameterOriginIdUpdater.UpdateSimulationId(_simulationToUpdate);
      }

      private static Dictionary<string, string> makeReplacementDictionary(ICache<string, string> oldIdCache, ICache<string, string> newIdCache)
      {
         var replacementDictionary = new Dictionary<string, string>();
         oldIdCache.Keys.Each(x =>
         {
            if (newIdCache.Contains(x))
               replacementDictionary.Add(oldIdCache[x], newIdCache[x]);
         });
         return replacementDictionary;
      }

      private static Cache<string, string> getEntityIdCache(IMoBiSimulation simulationToUpdate)
      {
         var idCache = new Cache<string, string>();
         simulationToUpdate.Model.Root.GetAllChildren<IEntity>().Each(x => idCache.Add(x.EntityPath(), x.Id));
         return idCache;
      }

      private static void replaceDiagramModelNodeIds(IDiagramModel diagramModel, ICache<string, string> oldIdCache, ICache<string, string> newIdCache)
      {
         var replacementDictionary = makeReplacementDictionary(oldIdCache, newIdCache);
         diagramModel.ReplaceNodeIds(replacementDictionary);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _simulationToUpdate = context.Get<IMoBiSimulation>(_simulationId);
         _newModel = context.Deserialize<IModel>(_modelSerialization);
         _updatedSimulationConfiguration = context.Deserialize<SimulationConfiguration>(_simulationConfigurationSerialization);
         _newSimulationBuilderEntitySources = context.Deserialize<SimulationEntitySources>(_simulationEntitySourceSerialization);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateSimulationCommand(_simulationToUpdate, _newModel, _newSimulationBuilderEntitySources, _updatedSimulationConfiguration, _wasChanged, _oldUntraceableChangesState)
            .AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _simulationToUpdate = null;
         _newModel = null;
         _updatedSimulationConfiguration = null;
         _newSimulationBuilderEntitySources = null;
      }
   }
}