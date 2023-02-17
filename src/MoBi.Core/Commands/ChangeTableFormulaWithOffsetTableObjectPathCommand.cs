using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class ChangeTableFormulaWithOffsetTableObjectPathCommand : ChangeTableFormulaWithReferencePathCommandBase<TableFormulaWithOffset>
   {
      public ChangeTableFormulaWithOffsetTableObjectPathCommand(TableFormulaWithOffset tableFormulaWithOffset, FormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock)
         : base(tableFormulaWithOffset, newFormulaUsablePath, buildingBlock, x => x.TableObjectAlias, tableFormulaWithOffset.AddTableObjectPath)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeTableFormulaWithOffsetTableObjectPathCommand(_tableFormulaWithReference, _oldFormulaUsablePath, _buildingBlock).AsInverseFor(this);
      }
   }

   public class ChangeTableFormulaWithOffsetOffsetObjectPathCommand : ChangeTableFormulaWithReferencePathCommandBase<TableFormulaWithOffset>
   {
      public ChangeTableFormulaWithOffsetOffsetObjectPathCommand(TableFormulaWithOffset tableFormulaWithOffset, FormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock)
         : base(tableFormulaWithOffset, newFormulaUsablePath, buildingBlock, x => x.OffsetObjectAlias, tableFormulaWithOffset.AddOffsetObjectPath)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeTableFormulaWithOffsetOffsetObjectPathCommand(_tableFormulaWithReference, _oldFormulaUsablePath, _buildingBlock).AsInverseFor(this);
      }
   }
}  