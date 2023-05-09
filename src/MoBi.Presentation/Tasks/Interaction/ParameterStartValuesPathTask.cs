using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterValuePathTask : IStartValuePathTask<ParameterValuesBuildingBlock, ParameterValue>
   {
   }

   public class ParameterValueStartValuePathTask : AbstractStartValuePathTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuePathTask
   {

      public ParameterValueStartValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask,context)
      {
      }

      public override IMoBiCommand UpdateNameCommand(ParameterValuesBuildingBlock startValues, ParameterValue pathAndValueEntity, string newValue)
      {
         return new ChangeParameterValueNameCommand(startValues, pathAndValueEntity.Path, newValue);
      }

      public override IMoBiCommand UpdateContainerPathCommand(ParameterValuesBuildingBlock buildingBlock, ParameterValue pathAndValueEntity, int indexToUpdate, string newValue)
      {
         var targetPath = pathAndValueEntity.ContainerPath.Clone<ObjectPath>();
         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditParameterValuePathCommand(buildingBlock, pathAndValueEntity, targetPath);
      }
   }
}