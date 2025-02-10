using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddIndividualBuildingBlockToProjectCommand : AddBuildingBlockToProjectCommand<IndividualBuildingBlock>
   {
      public AddIndividualBuildingBlockToProjectCommand(IndividualBuildingBlock buildingBlock) : base(buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveIndividualBuildingBlockFromProjectCommand(_buildingBlock).AsInverseFor(this);
      }

      protected override void AddToProject(MoBiProject project)
      {
         project.AddIndividualBuildingBlock(_buildingBlock);
      }
   }
}