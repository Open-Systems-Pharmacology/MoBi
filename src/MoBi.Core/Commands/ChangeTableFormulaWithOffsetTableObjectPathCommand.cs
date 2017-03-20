using System;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public abstract class ChangeTableFormulaWithOffsetPathPropertyCommandBase : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      protected TableFormulaWithOffset _tableFormulaWithOffset;
      protected IFormulaUsablePath _newFormulaUsablePath;
      protected IFormulaUsablePath _oldFormulaUsablePath;
      private readonly Func<TableFormulaWithOffset, string> _aliasFunc;
      private readonly Action<IFormulaUsablePath> _addAction;
      private readonly string _tableFormulaWithOffsetId;

      protected ChangeTableFormulaWithOffsetPathPropertyCommandBase(TableFormulaWithOffset tableFormulaWithOffset,
         IFormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock, Func<TableFormulaWithOffset, string> aliasFunc, Action<IFormulaUsablePath> addAction)
         : base(buildingBlock)
      {
         _tableFormulaWithOffset = tableFormulaWithOffset;
         _tableFormulaWithOffsetId = _tableFormulaWithOffset.Id;
         _newFormulaUsablePath = newFormulaUsablePath;
         _aliasFunc = aliasFunc;
         _addAction = addAction;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _tableFormulaWithOffset = context.Get<TableFormulaWithOffset>(_tableFormulaWithOffsetId);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _oldFormulaUsablePath = _tableFormulaWithOffset.ObjectPaths.SingleOrDefault(path => _aliasFunc(_tableFormulaWithOffset).Equals(path.Alias));
         if (_oldFormulaUsablePath != null)
            _tableFormulaWithOffset.RemoveObjectPath(_oldFormulaUsablePath);

         if (_newFormulaUsablePath != null)
            _addAction(_newFormulaUsablePath);
      }
   }

   public class ChangeTableFormulaWithOffsetTableObjectPathCommand : ChangeTableFormulaWithOffsetPathPropertyCommandBase
   {
      public ChangeTableFormulaWithOffsetTableObjectPathCommand(TableFormulaWithOffset tableFormulaWithOffset, IFormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock)
         : base(tableFormulaWithOffset, newFormulaUsablePath, buildingBlock, x => x.TableObjectAlias, tableFormulaWithOffset.AddTableObjectPath)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeTableFormulaWithOffsetTableObjectPathCommand(_tableFormulaWithOffset, _oldFormulaUsablePath, _buildingBlock).AsInverseFor(this);
      }
   }

   public class ChangeTableFormulaWithOffsetOffsetObjectPathCommand : ChangeTableFormulaWithOffsetPathPropertyCommandBase
   {
      public ChangeTableFormulaWithOffsetOffsetObjectPathCommand(TableFormulaWithOffset tableFormulaWithOffset, IFormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock)
         : base(tableFormulaWithOffset, newFormulaUsablePath, buildingBlock, x => x.OffsetObjectAlias, tableFormulaWithOffset.AddOffsetObjectPath)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeTableFormulaWithOffsetOffsetObjectPathCommand(_tableFormulaWithOffset, _oldFormulaUsablePath, _buildingBlock).AsInverseFor(this);
      }
   }
}