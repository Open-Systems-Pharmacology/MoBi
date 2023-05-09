using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class AddPathAndValueEntityFromQuantityInSimulationCommand<TQuantity, TPathAndValueEntity> : MoBiReversibleCommand
      where TPathAndValueEntity : PathAndValueEntity
      where TQuantity : class, IQuantity
   {
      protected TQuantity _quantity;
      private PathAndValueEntityBuildingBlock<TPathAndValueEntity> _buildingBlock;
      private readonly string _buildBuildingBlockId;
      protected ObjectPath _objectPath;

      protected AddPathAndValueEntityFromQuantityInSimulationCommand(TQuantity quantity, PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock)
      {
         _quantity = quantity;
         _buildingBlock = buildingBlock;
         _buildBuildingBlockId = buildingBlock.Id;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var entityPathResolver = context.Resolve<IEntityPathResolver>();
         _objectPath = entityPathResolver.ObjectPathFor(_quantity);

         if (_buildingBlock[_objectPath] != null)
            return;

         _buildingBlock.Add(CreateNewEntity(context));
      }

      protected abstract TPathAndValueEntity CreateNewEntity(IMoBiContext context);

      protected override void ClearReferences()
      {
         _buildingBlock = null;
         _quantity = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemovePathAndValueEntityFromBuildingBlockInSimulationCommand<TPathAndValueEntity>(_objectPath, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Get<PathAndValueEntityBuildingBlock<TPathAndValueEntity>>(_buildBuildingBlockId);
      }
   }

   public class AddParameterValueFromQuantityInSimulationCommand : AddPathAndValueEntityFromQuantityInSimulationCommand<IParameter, ParameterValue>
   {
      public AddParameterValueFromQuantityInSimulationCommand(IParameter parameter, ParameterValuesBuildingBlock parameterValuesBuildingBlock)
         : base(parameter, parameterValuesBuildingBlock)
      {
      }

      protected override ParameterValue CreateNewEntity(IMoBiContext context)
      {
         var parameterValuesCreator = context.Resolve<IParameterValuesCreator>();
         return parameterValuesCreator.CreateParameterValue(_objectPath, _quantity);
      }
   }

   public class AddInitialConditionFromQuantityInSimulationCommand : AddPathAndValueEntityFromQuantityInSimulationCommand<MoleculeAmount, InitialCondition>
   {
      public AddInitialConditionFromQuantityInSimulationCommand(MoleculeAmount moleculeAmount, InitialConditionsBuildingBlock initialConditionsBuildingBlock)
         : base(moleculeAmount, initialConditionsBuildingBlock)
      {
      }

      protected override InitialCondition CreateNewEntity(IMoBiContext context)
      {
         var initialConditionsCreator = context.Resolve<IInitialConditionsCreator>();
         var containerPath = _objectPath.Clone<ObjectPath>();
         var lastIndex = containerPath.Count - 1;
         var name = containerPath[lastIndex];
         containerPath.RemoveAt(lastIndex);

         return initialConditionsCreator.CreateInitialCondition(containerPath, name, _quantity.Dimension, _quantity.DisplayUnit, _quantity.ValueOrigin);
      }
   }
}