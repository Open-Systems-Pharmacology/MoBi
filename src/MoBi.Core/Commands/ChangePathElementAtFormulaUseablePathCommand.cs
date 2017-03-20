using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class ChangePathElementAtFormulaUseablePathCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private IFormulaUsablePath _path;
      private readonly string _alias;
      private readonly string _formulaId;
      private readonly string _oldElement;
      private readonly string _newElement;
      private IFormula _formula;

      public ChangePathElementAtFormulaUseablePathCommand(string newElement, IFormula formula, string oldElement, IFormulaUsablePath path,IBuildingBlock buildingBlock):base(buildingBlock)
      {
         _newElement = newElement;
         _oldElement = oldElement;
         _formula = formula;
         _formulaId = formula.Id;
         _path = path;
         _alias = path.Alias;
         Description = AppConstants.Commands.ChangePathElementDescription(_alias, _newElement, _oldElement, formula.Name);
         ObjectType =ObjectTypes.FormulaUsablePath;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangePathElementAtFormulaUseablePathCommand(_oldElement, _formula, _newElement, _path, _buildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _formula = null;
         _path = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _path.Replace(_oldElement, _newElement);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _formula = context.Get<IFormula>(_formulaId);
         _path = _formula.FormulaUsablePathBy(_alias);
      }
   }
}