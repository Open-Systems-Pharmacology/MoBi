using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class AddFormulaUsablePathCommand : AddItemCommand<IFormulaUsablePath, IFormula, IBuildingBlock>
   {
      private readonly IFormulaUsablePath _pathToAdd;

      public AddFormulaUsablePathCommand(IFormula parent, IFormulaUsablePath itemToAdd, IBuildingBlock buidingBlock) : base(parent, itemToAdd, buidingBlock)
      {
         _pathToAdd = itemToAdd;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveFormulaUsablePathCommand(_parent, _pathToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _parent.AddObjectPath(_pathToAdd);
         Description = AppConstants.Commands.AddToDescription(ObjectType, _pathToAdd.Alias, _parent.Name);
         context.PublishEvent(new AddedFormulaUsablePathEvent(_parent, _pathToAdd));
      }
   }
}