using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using MoBi.Assets;
using MoBi.Core.Domain.Services;
using MoBi.Core.Helper;

namespace MoBi.Core.Commands
{
   public abstract class AddPathAndValueEntityFromQuantityInSimulationCommand<TQuantity, TPathAndValueEntity> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<TPathAndValueEntity>>
      where TPathAndValueEntity : PathAndValueEntity
      where TQuantity : class, IQuantity
   {
      protected TQuantity _quantity;
      protected ObjectPath _objectPath;
      private readonly IObjectTypeResolver _objectTypeResolver;
      protected IMoBiSimulation _simulation;
      private readonly string _simulationId;
      protected SimulationEntitySource _originalSource;
      private readonly string _quantityId;

      protected AddPathAndValueEntityFromQuantityInSimulationCommand(TQuantity quantity, PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock, IMoBiSimulation simulation) : base(buildingBlock)
      {
         _quantity = quantity;
         _quantityId = _quantity.Id;
         CommandType = AppConstants.Commands.AddCommand;
         _objectTypeResolver = new ObjectTypeResolver();
         _simulation = simulation;
         _simulationId = simulation.Id;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var entityPathResolver = context.Resolve<IEntityPathResolver>();
         _objectPath = entityPathResolver.ObjectPathFor(_quantity);

         if (_buildingBlock[_objectPath] != null)
            return;

         var pathAndValueEntity = CreateNewEntity(context);
         ObjectType = _objectTypeResolver.TypeFor(pathAndValueEntity);
         Description = AppConstants.Commands.AddedPathAndValueEntity(pathAndValueEntity, _buildingBlock.Name, ObjectType);
         _buildingBlock.Add(pathAndValueEntity);

         var entitySourceUpdater = context.Resolve<ISimulationEntitySourceUpdater>();
         _originalSource = _simulation.EntitySources.SourceByPath(_objectPath);
         entitySourceUpdater.UpdateSourcesForNewPathAndValueEntity(_buildingBlock, _objectPath, _simulation);
      }

      protected abstract TPathAndValueEntity CreateNewEntity(IMoBiContext context);

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _buildingBlock = null;
         _quantity = null;
         _simulation = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _simulation = context.CurrentProject.Simulations.FindById(_simulationId);
         _buildingBlock = context.Get<PathAndValueEntityBuildingBlock<TPathAndValueEntity>>(_buildingBlockId);
         _quantity = _simulation.All<TQuantity>().FindById(_quantityId);
      }

      protected abstract class RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
      {
         protected TQuantity _quantity;
         private readonly ObjectPath _objectPath;
         protected IMoBiSimulation _simulation;
         private T _pathAndValueEntity;
         private readonly ObjectTypeResolver _objectTypeResolver;
         private readonly SimulationEntitySource _source;
         private readonly string _simulationId;
         private readonly string _quantityId;

         protected RemovePathAndValueEntityFromBuildingBlockInSimulationCommand(TQuantity quantity ,ObjectPath objectPath, PathAndValueEntityBuildingBlock<T> pathAndValueEntitiesBuildingBlock, IMoBiSimulation simulation, SimulationEntitySource source) : base(pathAndValueEntitiesBuildingBlock)
         {
            _quantity = quantity;
            _quantityId = quantity.Id;
            _objectPath = objectPath;
            _simulation = simulation;
            _simulationId = simulation.Id;
            CommandType = AppConstants.Commands.DeleteCommand;
            _objectTypeResolver = new ObjectTypeResolver();
            _source = source;
         }

         protected override void ExecuteWith(IMoBiContext context)
         {
            base.ExecuteWith(context);
            if (_objectPath == null)
               return;

            _pathAndValueEntity = _buildingBlock[_objectPath];
            if (_pathAndValueEntity == null)
               return;

            ObjectType = _objectTypeResolver.TypeFor(_pathAndValueEntity);
            Description = AppConstants.Commands.RemovedPathAndValueEntity(_pathAndValueEntity, _buildingBlock.Name, ObjectType);

            _buildingBlock.Remove(_pathAndValueEntity);

            if(_source != null)
               _simulation.EntitySources.Add(_source);
         }

         protected override void ClearReferences()
         {
            base.ClearReferences();
            _quantity = null;
            _simulation = null;
         }

         public override void RestoreExecutionData(IMoBiContext context)
         {
            base.RestoreExecutionData(context);
            _simulation = context.CurrentProject.Simulations.FindById(_simulationId);
            _quantity = _simulation.All<TQuantity>().FindById(_quantityId);
         }
      }
   }

   public class AddParameterValueFromQuantityInSimulationCommand : AddPathAndValueEntityFromQuantityInSimulationCommand<IParameter, ParameterValue>
   {
      public AddParameterValueFromQuantityInSimulationCommand(IParameter parameter, PathAndValueEntityBuildingBlock<ParameterValue> parameterValuesBuildingBlock, IMoBiSimulation simulation)
         : base(parameter, parameterValuesBuildingBlock, simulation)
      {
      }

      protected override ParameterValue CreateNewEntity(IMoBiContext context)
      {
         var parameterValuesCreator = context.Resolve<IParameterValuesCreator>();
         return parameterValuesCreator.CreateParameterValue(_objectPath, _quantity);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveParameterValueFromQuantityInSimulationCommand(_quantity, _objectPath, _buildingBlock, _simulation, _originalSource)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      private class RemoveParameterValueFromQuantityInSimulationCommand : RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<ParameterValue>
      {
         public RemoveParameterValueFromQuantityInSimulationCommand(IParameter quantity, ObjectPath objectPath, PathAndValueEntityBuildingBlock<ParameterValue> pathAndValueEntitiesBuildingBlock, IMoBiSimulation simulation, SimulationEntitySource source) : 
            base(quantity, objectPath, pathAndValueEntitiesBuildingBlock, simulation, source)
         {
         }

         protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return new AddParameterValueFromQuantityInSimulationCommand(_quantity, _buildingBlock, _simulation)
            {
               Visible = Visible
            }.AsInverseFor(this);
         }
      }
   }

   public class AddInitialConditionFromQuantityInSimulationCommand : AddPathAndValueEntityFromQuantityInSimulationCommand<MoleculeAmount, InitialCondition>
   {

      public AddInitialConditionFromQuantityInSimulationCommand(MoleculeAmount moleculeAmount, PathAndValueEntityBuildingBlock<InitialCondition> initialConditionsBuildingBlock, IMoBiSimulation simulation)
         : base(moleculeAmount, initialConditionsBuildingBlock, simulation)
      {
      }

      protected override InitialCondition CreateNewEntity(IMoBiContext context)
      {
         var initialConditionsCreator = context.Resolve<IInitialConditionsCreator>();
         return initialConditionsCreator.CreateInitialCondition(_objectPath, _quantity);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveInitialConditionFromQuantityInSimulationCommand(_quantity, _objectPath, _buildingBlock, _simulation, _originalSource)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      private class RemoveInitialConditionFromQuantityInSimulationCommand : RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<InitialCondition>
      {
         public RemoveInitialConditionFromQuantityInSimulationCommand(MoleculeAmount quantity, ObjectPath objectPath, PathAndValueEntityBuildingBlock<InitialCondition> pathAndValueEntitiesBuildingBlock, IMoBiSimulation simulation, SimulationEntitySource source) :
            base(quantity, objectPath, pathAndValueEntitiesBuildingBlock, simulation, source)
         {
         }

         protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
         {
            return new AddInitialConditionFromQuantityInSimulationCommand(_quantity, _buildingBlock, _simulation)
            {
               Visible = Visible
            }.AsInverseFor(this);
         }
      }
   }
}