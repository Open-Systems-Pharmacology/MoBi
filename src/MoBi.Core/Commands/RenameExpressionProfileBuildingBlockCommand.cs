using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class RenameExpressionProfileBuildingBlockCommand : RenameObjectBaseCommand
   {
      private ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;
      private readonly string _oldMoleculeName;

      public RenameExpressionProfileBuildingBlockCommand(ExpressionProfileBuildingBlock expressionProfileBuildingBlock, string newName, IBuildingBlock buildingBlock) : base(expressionProfileBuildingBlock, newName, buildingBlock)
      {
         _expressionProfileBuildingBlock = expressionProfileBuildingBlock;
         _oldMoleculeName = _expressionProfileBuildingBlock.MoleculeName;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _expressionProfileBuildingBlock = null;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RenameExpressionProfileBuildingBlockCommand(_expressionProfileBuildingBlock, OldName, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _expressionProfileBuildingBlock = context.Get<ExpressionProfileBuildingBlock>(ObjectId);
      }

      protected override void RenameObjectBase(IMoBiContext context)
      {
         base.RenameObjectBase(context);
         // We need to modify the path of all builders when the building block name changes
         // The building block name reflects the molecule name so when that changes, the path of all builders needs to be updated
         // to the new molecule name
         _expressionProfileBuildingBlock.ExpressionParameters.Each(x =>
         {
            var objectPath = x.Path;
            objectPath.Replace(_oldMoleculeName, _expressionProfileBuildingBlock.MoleculeName);
            x.Path = objectPath;
         });
      }
   }
}