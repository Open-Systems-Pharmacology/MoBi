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
   public abstract class ResetQuantityValuesToDefaultFromStartValuesInSimulation<TStartValue> : MoBiCommand where TStartValue : class, IStartValue
   {
      protected IMoBiSimulation _simulation;
      private IStartValuesBuildingBlock<TStartValue> _startValuesBuildingBlock;
      protected ICloneManagerForModel _cloneManagerForModel;
      protected IMoBiFormulaTask _formulaTask;
      protected IEntityPathResolver _entityPathResolver;

      protected ResetQuantityValuesToDefaultFromStartValuesInSimulation(IMoBiSimulation simulation, IStartValuesBuildingBlock<TStartValue> startValuesBuildingBlock)
      {
         _simulation = simulation;
         _startValuesBuildingBlock = startValuesBuildingBlock;
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
         var startValue = _startValuesBuildingBlock[objectPath];

         if (startValue == null)
            return;

         quantityToReset.Formula = defaultFormulaBasedOn(startValue);
         quantityToReset.IsFixedValue = false;

         context.PublishEvent(new QuantityChangedEvent(quantityToReset));
      }

      private IFormula defaultFormulaBasedOn(IStartValue startValue)
      {
         if (startValue.Formula != null)
            return _cloneManagerForModel.Clone(startValue.Formula);

         return _formulaTask.CreateNewFormula<ConstantFormula>(startValue.Dimension).WithValue(startValue.Value.GetValueOrDefault(double.NaN));
      }

      protected abstract IQuantity QuantityUsedToFindPathFor(IQuantity quantity);

      protected abstract IReadOnlyList<IQuantity> AllQuantitiesToReset();

      protected override void ClearReferences()
      {
         _simulation = null;
         _cloneManagerForModel = null;
         _formulaTask = null;
         _entityPathResolver = null;
         _startValuesBuildingBlock = null;
      }
   }
}