using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using static System.String;

namespace MoBi.Core.Commands
{
   public class ChangeValueFormulaCommand<T> : BuildingBlockChangeCommandBase<IBuildingBlock<T>> where T : PathAndValueEntity, IObjectBase, IUsingFormula
   {
      private readonly string _objectBaseId;
      private readonly string _newFormulaId;
      private readonly string _oldFormulaId;
      protected IFormula _newFormula;
      protected IFormula _oldFormula;
      protected T _changedPathAndValueEntity;
      protected ObjectPath Path { get; set; }

      public ChangeValueFormulaCommand(IBuildingBlock<T> buildingBlock, T pathAndValueEntity, IFormula newFormula, IFormula oldFormula): base(buildingBlock)
      {
         _newFormula = newFormula;
         _oldFormula = oldFormula;
         _objectBaseId = _buildingBlock.Id;
         _changedPathAndValueEntity = pathAndValueEntity;
         ObjectType = new ObjectTypeResolver().TypeFor(pathAndValueEntity);

         Description = AppConstants.Commands.EditDescription(ObjectType, AppConstants.Captions.FormulaName, _oldFormula?.ToString() ?? AppConstants.NullString, _newFormula?.ToString() ?? AppConstants.NullString, _changedPathAndValueEntity.Path.PathAsString);

         CommandType = AppConstants.Commands.EditCommand;

         _oldFormulaId = GetFormulaId(_oldFormula);
         _newFormulaId = GetFormulaId(_newFormula);

      }

      protected string GetFormulaId(IFormula formula)
      {
         return formula == null ? Empty : formula.Id;
      }

      protected IFormula GetFormula(string formulaId, IMoBiContext context)
      {
         return formulaId.IsNotEmpty() ? context.Get<IFormula>(formulaId) : null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeValueFormulaCommand<T>(_buildingBlock, _changedPathAndValueEntity, _oldFormula, _newFormula).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _changedPathAndValueEntity = null;
         _buildingBlock = null;
         _newFormula = null;
         _oldFormula = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _changedPathAndValueEntity.Formula = _newFormula;
         Path = _changedPathAndValueEntity.Path;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _buildingBlock = context.Get<IBuildingBlock<T>>(_objectBaseId);
         _changedPathAndValueEntity = _buildingBlock.Single(pathAndValueEntity => pathAndValueEntity.Path.Equals(Path));
         _oldFormula = GetFormula(_oldFormulaId, context);
         _newFormula = GetFormula(_newFormulaId, context);
      }
   }
}