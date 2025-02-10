using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddExpressionProfileBuildingBlockToProjectCommand : AddBuildingBlockToProjectCommand<ExpressionProfileBuildingBlock>
   {
      public AddExpressionProfileBuildingBlockToProjectCommand(ExpressionProfileBuildingBlock buildingBlock) : base(buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveExpressionProfileBuildingBlockFromProjectCommand(_buildingBlock).AsInverseFor(this);
      }

      protected override void AddToProject(MoBiProject project)
      {
         project.AddExpressionProfileBuildingBlock(_buildingBlock);
      }
   }
}