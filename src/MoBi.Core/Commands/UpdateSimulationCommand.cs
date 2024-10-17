using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
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
      private readonly string _simulationId;
      private bool _wasChanged;
      private byte[] _simulationConfigurationSerialization;
      private byte[] _modelSerialization;
      private readonly string _changedBuildingBlockName;
      private readonly string _changedBuildingBlockType;

      public UpdateSimulationCommand(IMoBiSimulation simulationToUpdate, IModel newModel, SimulationConfiguration updatedSimulationConfiguration)
         : this(simulationToUpdate, newModel, updatedSimulationConfiguration, true, string.Empty, string.Empty)
      {
      }

      public UpdateSimulationCommand(IMoBiSimulation simulationToUpdate, IModel newModel, SimulationConfiguration updatedSimulationConfiguration, IBuildingBlock templateBuildingBlock)
         : this(simulationToUpdate, newModel, updatedSimulationConfiguration, true, templateBuildingBlock.Name, string.Empty)
      {
         _changedBuildingBlockType = new ObjectTypeResolver().TypeFor(templateBuildingBlock);
      }

      private UpdateSimulationCommand(IMoBiSimulation simulationToUpdate, IModel newModel, SimulationConfiguration updatedSimulationConfiguration, bool wasChanged,
         string buildingBlockName, string changedBuildingBlockType)
      {
         _hasChanged = wasChanged;
         _simulationToUpdate = simulationToUpdate;
         _newModel = newModel;
         _updatedSimulationConfiguration = updatedSimulationConfiguration;
         _simulationId = _simulationToUpdate.Id;
         _changedBuildingBlockName = buildingBlockName;
         _changedBuildingBlockType = changedBuildingBlockType;
         ObjectType = ObjectTypes.Simulation;
         CommandType = AppConstants.Commands.UpdateCommand;

         if (string.IsNullOrEmpty(_changedBuildingBlockName))
            Description = AppConstants.Commands.ConfigureSimulationDescription(_simulationToUpdate.Name);
         else
            Description = AppConstants.Commands.UpdateCommandDescription(_simulationToUpdate.Name, _changedBuildingBlockName, _changedBuildingBlockType);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _modelSerialization = context.Serialize(_simulationToUpdate.Model);
         _simulationConfigurationSerialization = context.Serialize(_simulationToUpdate.Configuration);
         context.UnregisterSimulation(_simulationToUpdate);
         context.PublishEvent(new SimulationUnloadEvent(_simulationToUpdate));

         var oldIdCache = getEntityIdCache(_simulationToUpdate);

         _simulationToUpdate.Update(_updatedSimulationConfiguration, _newModel);

         updateReferencesToSimulation(context);

         if (_simulationToUpdate.DiagramModel != null)
            replaceDiagramModelNodeIds(_simulationToUpdate.DiagramModel, oldIdCache, getEntityIdCache(_simulationToUpdate));

         context.Register(_simulationToUpdate);
         _wasChanged = _simulationToUpdate.HasChanged;
         _simulationToUpdate.HasChanged = _hasChanged;

         // We need ToList since we are modifying the collection in the loop
         _simulationToUpdate.OriginalQuantityValues.ToList().Each(x => _simulationToUpdate.RemoveOriginalQuantityValue(x.Path));
         
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
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new UpdateSimulationCommand(_simulationToUpdate, _newModel, _updatedSimulationConfiguration, _wasChanged, _changedBuildingBlockName, _changedBuildingBlockType)
            .AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _simulationToUpdate = null;
         _newModel = null;
         _updatedSimulationConfiguration = null;
      }
   }
}