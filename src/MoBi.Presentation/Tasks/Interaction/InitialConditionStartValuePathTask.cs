using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInitialConditionPathTask : IStartValuePathTask<InitialConditionsBuildingBlock, InitialCondition>
   {
   }

   public class InitialConditionStartValuePathTask : AbstractStartValuePathTask<InitialConditionsBuildingBlock, InitialCondition>, IInitialConditionPathTask
   {
      public InitialConditionStartValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask,context)
      {
      }

      public override IMoBiCommand UpdateNameCommand(InitialConditionsBuildingBlock startValues, InitialCondition pathAndValueEntity, string newValue)
      {
         return new ChangeInitialConditionNameCommand(startValues, pathAndValueEntity.Path, newValue);
      }

      public override IMoBiCommand UpdateContainerPathCommand(InitialConditionsBuildingBlock buildingBlock, InitialCondition pathAndValueEntity, int indexToUpdate, string newValue)
      {
         var targetPath = pathAndValueEntity.ContainerPath.Clone<ObjectPath>();

         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditInitialConditionPathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }
   }
}