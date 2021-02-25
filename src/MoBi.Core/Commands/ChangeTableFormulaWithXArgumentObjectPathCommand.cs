using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Commands
{
   public class ChangeTableFormulaWithXArgumentTableObjectPathCommand : ChangeTableFormulaWithReferencePathCommandBase<TableFormulaWithXArgument>
   {
      public ChangeTableFormulaWithXArgumentTableObjectPathCommand(TableFormulaWithXArgument tableFormulaWithXArgument, IFormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock)
         : base(tableFormulaWithXArgument, newFormulaUsablePath, buildingBlock, x => x.TableObjectAlias, tableFormulaWithXArgument.AddTableObjectPath)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeTableFormulaWithXArgumentTableObjectPathCommand(_tableFormulaWithReference, _oldFormulaUsablePath, _buildingBlock).AsInverseFor(this);
      }
   }

   public class ChangeTableFormulaWithXArgumentXArgumentObjectPathCommand : ChangeTableFormulaWithReferencePathCommandBase<TableFormulaWithXArgument>
   {
      public ChangeTableFormulaWithXArgumentXArgumentObjectPathCommand(TableFormulaWithXArgument tableFormulaWithXArgument, IFormulaUsablePath newFormulaUsablePath, IBuildingBlock buildingBlock)
         : base(tableFormulaWithXArgument, newFormulaUsablePath, buildingBlock, x => x.XArgumentAlias, tableFormulaWithXArgument.AddXArgumentObjectPath)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new ChangeTableFormulaWithXArgumentXArgumentObjectPathCommand(_tableFormulaWithReference, _oldFormulaUsablePath, _buildingBlock).AsInverseFor(this);
      }
   }
}