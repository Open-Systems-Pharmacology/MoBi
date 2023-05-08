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

   public class ParameterValuePathTask : AbstractStartValuePathTask<ParameterValuesBuildingBlock, ParameterValue>, IParameterValuePathTask
   {

      public ParameterValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask,context)
      {
      }

      public override IMoBiCommand UpdateStartValueNameCommand(ParameterValuesBuildingBlock startValues, ParameterValue startValue, string newValue)
      {
         return new ChangeParameterValueNameCommand(startValues, startValue.Path, newValue);
      }

      public override IMoBiCommand UpdateStartValueContainerPathCommand(ParameterValuesBuildingBlock buildingBlock, ParameterValue startValue, int indexToUpdate, string newValue)
      {
         var targetPath = startValue.ContainerPath.Clone<ObjectPath>();
         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditParameterValuePathCommand(buildingBlock, startValue, targetPath);
      }
   }
}