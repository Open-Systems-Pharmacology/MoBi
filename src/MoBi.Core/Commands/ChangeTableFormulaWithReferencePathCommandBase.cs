using System;
using System.Linq;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class ChangeTableFormulaWithReferencePathCommandBase<TFormula> : BuildingBlockChangeCommandBase<IBuildingBlock> where TFormula: Formula
   {
      protected TFormula _tableFormulaWithReference;
      protected IFormulaUsablePath _newFormulaUsablePath;
      protected IFormulaUsablePath _oldFormulaUsablePath;
      private readonly Func<TFormula, string> _aliasFunc;
      private readonly Action<IFormulaUsablePath> _addAction;
      private readonly string _tableFormulaWithReferenceId;

      protected ChangeTableFormulaWithReferencePathCommandBase(TFormula tableFormulaWithReference,
         IFormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock, Func<TFormula, string> aliasFunc, Action<IFormulaUsablePath> addAction)
         : base(buildingBlock)
      {
         _tableFormulaWithReference = tableFormulaWithReference;
         _tableFormulaWithReferenceId = _tableFormulaWithReference.Id;
         _newFormulaUsablePath = newFormulaUsablePath;
         _aliasFunc = aliasFunc;
         _addAction = addAction;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _tableFormulaWithReference = context.Get<TFormula>(_tableFormulaWithReferenceId);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldFormulaUsablePath = _tableFormulaWithReference.ObjectPaths.SingleOrDefault(path => _aliasFunc(_tableFormulaWithReference).Equals(path.Alias));
         if (_oldFormulaUsablePath != null)
            _tableFormulaWithReference.RemoveObjectPath(_oldFormulaUsablePath);

         if (_newFormulaUsablePath != null)
            _addAction(_newFormulaUsablePath);
      }
   }
}