using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class AddFormulaToFormulaCacheCommand : AddObjectBaseCommand<IFormula, IBuildingBlock>
   {
      public AddFormulaToFormulaCacheCommand(IBuildingBlock buildingBlock, IFormula itemToAdd) : base(buildingBlock, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveFormulaFromFormulaCacheCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(IFormula child, IBuildingBlock buildingBlock, IMoBiContext context)
      {
         buildingBlock.AddFormula(child);
      }
   }

   public class RemoveFormulaFromFormulaCacheCommand : RemoveObjectBaseCommand<IFormula, IBuildingBlock>
   {
      public RemoveFormulaFromFormulaCacheCommand(IBuildingBlock buildingBlock, IFormula itemToRemove) : base(buildingBlock, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddFormulaToFormulaCacheCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(IFormula childToRemove, IBuildingBlock buildingBlock, IMoBiContext context)
      {
         buildingBlock.FormulaCache.Remove(childToRemove);
      }
   }
}