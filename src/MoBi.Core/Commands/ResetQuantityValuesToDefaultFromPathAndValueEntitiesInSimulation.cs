using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public abstract class ResetQuantityValuesToDefaultFromPathAndValueEntitiesInSimulation<TPathAndValueEntity> : MoBiCommand where TPathAndValueEntity : PathAndValueEntity
   {
      protected IMoBiSimulation _simulation;
      private PathAndValueEntityBuildingBlock<TPathAndValueEntity> _buildingBlock;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IMoBiFormulaTask _formulaTask;
      protected IEntityPathResolver _entityPathResolver;

      protected ResetQuantityValuesToDefaultFromPathAndValueEntitiesInSimulation(IMoBiSimulation simulation, PathAndValueEntityBuildingBlock<TPathAndValueEntity> buildingBlock)
      {
         _simulation = simulation;
         _buildingBlock = buildingBlock;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         _entityPathResolver = context.Resolve<IEntityPathResolver>();
         _cloneManagerForModel = context.Resolve<ICloneManagerForModel>();
         _formulaTask = context.Resolve<IMoBiFormulaTask>();

         foreach (var quantity in AllQuantitiesToReset())
         {
            resetQuantity(quantity, QuantityUsedToFindPathFor(quantity),context);
         }
      }

      private void resetQuantity(IQuantity quantityToReset, IQuantity quantityUsedToFindPath, IMoBiContext context)
      {
         if (quantityUsedToFindPath == null)
            return;

         var objectPath = _entityPathResolver.ObjectPathFor(quantityUsedToFindPath);
         var pathAndValueEntity = _buildingBlock[objectPath];

         if (pathAndValueEntity == null)
            return;

         quantityToReset.Formula = defaultFormulaBasedOn(pathAndValueEntity);
         quantityToReset.IsFixedValue = false;

         context.PublishEvent(new QuantityChangedEvent(quantityToReset));
      }

      private IFormula defaultFormulaBasedOn(PathAndValueEntity pathAndValueEntity)
      {
         if (pathAndValueEntity.Formula != null)
            return _cloneManagerForModel.Clone(pathAndValueEntity.Formula);

         return _formulaTask.CreateNewFormula<ConstantFormula>(pathAndValueEntity.Dimension).WithValue(pathAndValueEntity.Value.GetValueOrDefault(double.NaN));
      }

      protected abstract IQuantity QuantityUsedToFindPathFor(IQuantity quantity);

      protected abstract IReadOnlyList<IQuantity> AllQuantitiesToReset();

      protected override void ClearReferences()
      {
         _simulation = null;
         _cloneManagerForModel = null;
         _formulaTask = null;
         _entityPathResolver = null;
         _buildingBlock = null;
      }
   }
}