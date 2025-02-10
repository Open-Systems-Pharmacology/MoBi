using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using MoBi.Assets;
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

      protected AddPathAndValueEntityFromQuantityInSimulationCommand(TQuantity quantity, PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock) : base(buildingBlock)
      {
         _quantity = quantity;
         CommandType = AppConstants.Commands.AddCommand;
         _objectTypeResolver = new ObjectTypeResolver();
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
      }

      protected abstract TPathAndValueEntity CreateNewEntity(IMoBiContext context);

      protected override void ClearReferences()
      {
         base.ClearReferences();
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
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<PathAndValueEntityBuildingBlock<TPathAndValueEntity>>(_buildingBlockId);
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
         return initialConditionsCreator.CreateInitialCondition(_objectPath, _quantity);
      }
   }
}