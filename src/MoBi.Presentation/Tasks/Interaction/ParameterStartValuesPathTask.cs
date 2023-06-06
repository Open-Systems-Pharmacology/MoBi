using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuePathTask : IStartValuePathTask<ILookupBuildingBlock<ParameterValue>, ParameterValue>
   {
   }

   public class ParameterValuePathTask : AbstractStartValuePathTask<ILookupBuildingBlock<ParameterValue>, ParameterValue>, IParameterValuePathTask
   {
      public ParameterValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask, context)
      {
      }

      public override IMoBiCommand UpdateNameCommand(ILookupBuildingBlock<ParameterValue> startValues, ParameterValue pathAndValueEntity, string newValue)
      {
         return new ChangeParameterValueNameCommand(startValues, pathAndValueEntity.Path, newValue);
      }

      public override IMoBiCommand UpdateContainerPathCommand(ILookupBuildingBlock<ParameterValue> buildingBlock, ParameterValue pathAndValueEntity, int indexToUpdate, string newValue)
      {
         var targetPath = pathAndValueEntity.ContainerPath.Clone<ObjectPath>();
         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditParameterValuePathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }
   }
}