using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuePathTask : IPathAndValueEntityPathTask<ILookupBuildingBlock<ParameterValue>, ParameterValue>
   {
   }

   public class ParameterValuePathTask : AbstractPathAndValueEntityPathTask<ILookupBuildingBlock<ParameterValue>, ParameterValue>, IParameterValuePathTask
   {
      public ParameterValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask, context)
      {
      }

      public override IMoBiCommand UpdateNameCommand(ILookupBuildingBlock<ParameterValue> parameterValues, ParameterValue pathAndValueEntity, string newValue)
      {
         return new ChangeParameterValueNameCommand(parameterValues, pathAndValueEntity.Path, newValue);
      }

      public override IMoBiCommand UpdateContainerPathCommand(ILookupBuildingBlock<ParameterValue> buildingBlock, ParameterValue pathAndValueEntity, int indexToUpdate, string newValue)
      {
         var targetPath = pathAndValueEntity.ContainerPath.Clone<ObjectPath>();
         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return SetContainerPathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }

      public override IMoBiCommand SetContainerPathCommand(ILookupBuildingBlock<ParameterValue> buildingBlock, ParameterValue pathAndValueEntity, ObjectPath targetPath)
      {
         return new EditParameterValuePathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }
   }
}