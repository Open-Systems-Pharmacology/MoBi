using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveIndividualBuildingBlockFromProjectCommand : RemoveBuildingBlockFromProjectCommand<IndividualBuildingBlock>
   {
      public RemoveIndividualBuildingBlockFromProjectCommand(IndividualBuildingBlock buildingBlock) : base(buildingBlock)
      {
      }

      protected override void RemoveFromProject(MoBiProject project)
      {
         project.RemoveIndividualBuildingBlock(_buildingBlock);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddIndividualBuildingBlockToProjectCommand(_buildingBlock).AsInverseFor(this);
      }
   }
}