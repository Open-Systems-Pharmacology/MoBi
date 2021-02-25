using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class ChangeStartValueFormulaCommand<T> : BuildingBlockChangeCommandBase<IBuildingBlock> where T : class, IStartValue
   {
      private readonly string _objectBaseId;
      private readonly string _newFormulaId;
      private readonly string _oldFormulaId;
      protected IFormula _newFormula;
      protected IFormula _oldFormula;
      protected IStartValuesBuildingBlock<T> _startValuesBuildingBlock;
      protected T _changedStartValue;
      public IObjectPath Path { get; set; }


      public ChangeStartValueFormulaCommand(IStartValuesBuildingBlock<T> buildingBlock, T startValue, IFormula newFormula, IFormula oldFormula): base(buildingBlock)
      {
         _newFormula = newFormula;
         _oldFormula = oldFormula;
         _objectBaseId = _buildingBlock.Id;
         _changedStartValue = startValue;
         ObjectType = new ObjectTypeResolver().TypeFor(startValue);

         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.FormulaName, _oldFormula?.ToString() ?? AppConstants.NullString, _newFormula?.ToString() ?? AppConstants.NullString, _changedStartValue.Path.PathAsString);

         CommandType = AppConstants.Commands.EditCommand;

         _oldFormulaId = GetFormulaId(_oldFormula);
         _newFormulaId = GetFormulaId(_newFormula);

      }

      protected string GetFormulaId(IFormula formula)
      {
         return formula == null ? String.Empty : formula.Id;
      }

      protected IFormula GetFormula(string formulaId, IMoBiContext context)
      {
         return formulaId.IsNotEmpty() ? context.Get<IFormula>(formulaId) : null;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _changedStartValue = null;
         _startValuesBuildingBlock = null;
         _newFormula = null;
         _oldFormula = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _changedStartValue.Formula = _newFormula;
         Path = _changedStartValue.Path;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeStartValueFormulaCommand<T>(_startValuesBuildingBlock, _changedStartValue, _oldFormula, _newFormula).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _startValuesBuildingBlock = context.Get<IStartValuesBuildingBlock<T>>(_objectBaseId);
         _changedStartValue = _startValuesBuildingBlock.Single(startValue => startValue.Path.Equals(Path));
         _oldFormula = GetFormula(_oldFormulaId, context);
         _newFormula = GetFormula(_newFormulaId, context);
      }
   }
}