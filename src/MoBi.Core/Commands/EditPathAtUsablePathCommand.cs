using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditPathAtUsablePathCommand : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      private readonly FormulaUsablePath _oldObjectPath;
      private readonly string _alias;
      private readonly ObjectPath _newObjectPath;
      private readonly string _formulaId;
      private FormulaUsablePath _formulaPathToUpdate;
      private IFormula _formula;

      public EditPathAtUsablePathCommand(IFormula formula, ObjectPath newObjectPath, FormulaUsablePath formulaPathToUpdate, IBuildingBlock buildingBlock) : base(buildingBlock)
      {
         _newObjectPath = newObjectPath;
         _formulaId = formula.Id;
         _formulaPathToUpdate = formulaPathToUpdate;
         //we need a clone here to save the old path before updating it
         _oldObjectPath = _formulaPathToUpdate.Clone<FormulaUsablePath>();
         _alias = _formulaPathToUpdate.Alias;
         Description = AppConstants.Commands.EditFormulaUsablePath(_oldObjectPath.PathAsString, newObjectPath.PathAsString, _alias, formula.Name, buildingBlock.Name);
         ObjectType = ObjectTypes.FormulaUsablePath;
         CommandType = AppConstants.Commands.EditCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var remove = new List<string>(_formulaPathToUpdate);
         remove.Each(toRemove => _formulaPathToUpdate.Remove(toRemove));
         _newObjectPath.Each(newEntry => _formulaPathToUpdate.Add(newEntry));
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _formulaPathToUpdate = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditPathAtUsablePathCommand(_formula, _oldObjectPath, _formulaPathToUpdate, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _formula = context.Get<IFormula>(_formulaId);
         _formulaPathToUpdate = _formula.FormulaUsablePathBy(_alias);
      }
   }
}