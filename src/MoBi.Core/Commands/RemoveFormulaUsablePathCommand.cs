using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class RemoveFormulaUsablePathCommand : RemoveItemCommand<IFormulaUsablePath, IFormula, IBuildingBlock>
   {
      public RemoveFormulaUsablePathCommand(IFormula parent, IFormulaUsablePath itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddFormulaUsablePathCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, _itemToRemove.Alias, _parent.Name);
         _parent.RemoveObjectPath(_itemToRemove);
         context.PublishEvent(new RemovedFormulaUsablePathEvent(_parent, _itemToRemove));
         context.PublishEvent(new FormulaChangedEvent(_parent));
      }
   }
}