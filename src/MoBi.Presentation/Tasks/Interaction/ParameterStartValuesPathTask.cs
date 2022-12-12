using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IParameterStartValuePathTask : IStartValuePathTask<IParameterStartValuesBuildingBlock, ParameterStartValue>
   {
   }

   public class ParameterStartValuePathTask : AbstractStartValuePathTask<IParameterStartValuesBuildingBlock, ParameterStartValue>, IParameterStartValuePathTask
   {

      public ParameterStartValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask,context)
      {
      }

      public override IMoBiCommand UpdateStartValueNameCommand(IParameterStartValuesBuildingBlock startValues, ParameterStartValue startValue, string newValue)
      {
         return new ChangeParameterStartValueNameCommand(startValues, startValue.Path, newValue);
      }

      public override IMoBiCommand UpdateStartValueContainerPathCommand(IParameterStartValuesBuildingBlock buildingBlock, ParameterStartValue startValue, int indexToUpdate, string newValue)
      {
         var targetPath = startValue.ContainerPath.Clone<IObjectPath>();
         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditParameterStartValuePathCommand(buildingBlock, startValue, targetPath);
      }
   }
}