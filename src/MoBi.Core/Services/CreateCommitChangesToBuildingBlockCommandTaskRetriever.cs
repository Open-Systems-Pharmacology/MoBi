using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Services
{
   public interface ICreateCommitChangesToBuildingBlockCommandTaskRetriever
   {
      ICreateCommitChangesToBuildingBlockCommandTask TaskFor(IBuildingBlock buildingBlock);
   }

   internal class CreateCommitChangesToBuildingBlockCommandTaskRetriever : ICreateCommitChangesToBuildingBlockCommandTaskRetriever
   {
      private readonly IReadOnlyList<ICreateCommitChangesToBuildingBlockCommandTask> _taskRepository;

      public CreateCommitChangesToBuildingBlockCommandTaskRetriever(IRepository<ICreateCommitChangesToBuildingBlockCommandTask> taskRepository)
      {
         _taskRepository = taskRepository.All().ToList();
      }

      public ICreateCommitChangesToBuildingBlockCommandTask TaskFor(IBuildingBlock buildingBlock)
      {
         return _taskRepository.SingleOrDefault(task => task.IsSatisfiedBy(buildingBlock));
      }
   }
}