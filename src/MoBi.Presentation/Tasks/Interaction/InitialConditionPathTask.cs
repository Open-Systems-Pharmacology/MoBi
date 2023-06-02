using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInitialConditionPathTask : IStartValuePathTask<IBuildingBlock<InitialCondition>, InitialCondition>
   {
   }

   public class InitialConditionPathTask : AbstractStartValuePathTask<IBuildingBlock<InitialCondition>, InitialCondition>, IInitialConditionPathTask
   {
      public InitialConditionPathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask, context)
      {
      }

      public override IMoBiCommand UpdateNameCommand(IBuildingBlock<InitialCondition> startValues, InitialCondition pathAndValueEntity, string newValue)
      {
         return new ChangeInitialConditionNameCommand(startValues, pathAndValueEntity.Path, newValue);
      }

      public override IMoBiCommand UpdateContainerPathCommand(IBuildingBlock<InitialCondition> buildingBlock, InitialCondition pathAndValueEntity, int indexToUpdate, string newValue)
      {
         var targetPath = pathAndValueEntity.ContainerPath.Clone<ObjectPath>();

         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditInitialConditionPathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }
   }
}