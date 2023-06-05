using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInitialConditionPathTask : IStartValuePathTask<ILookupBuildingBlock<InitialCondition>, InitialCondition>
   {
   }

   public class InitialConditionPathTask : AbstractStartValuePathTask<ILookupBuildingBlock<InitialCondition>, InitialCondition>, IInitialConditionPathTask
   {
      public InitialConditionPathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask, context)
      {
      }

      public override IMoBiCommand UpdateNameCommand(ILookupBuildingBlock<InitialCondition> startValues, InitialCondition pathAndValueEntity, string newValue)
      {
         return new ChangeInitialConditionNameCommand(startValues, pathAndValueEntity.Path, newValue);
      }

      public override IMoBiCommand UpdateContainerPathCommand(ILookupBuildingBlock<InitialCondition> buildingBlock, InitialCondition pathAndValueEntity, int indexToUpdate, string newValue)
      {
         var targetPath = pathAndValueEntity.ContainerPath.Clone<ObjectPath>();

         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditInitialConditionPathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }
   }
}