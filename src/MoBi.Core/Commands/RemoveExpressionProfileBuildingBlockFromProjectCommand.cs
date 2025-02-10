using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveExpressionProfileBuildingBlockFromProjectCommand : RemoveBuildingBlockFromProjectCommand<ExpressionProfileBuildingBlock>
   {
      public RemoveExpressionProfileBuildingBlockFromProjectCommand(ExpressionProfileBuildingBlock buildingBlock) : base(buildingBlock)
      {
      }

      protected override void RemoveFromProject(MoBiProject project)
      {
         project.RemoveExpressionProfileBuildingBlock(_buildingBlock);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddExpressionProfileBuildingBlockToProjectCommand(_buildingBlock).AsInverseFor(this);
      }
   }
}