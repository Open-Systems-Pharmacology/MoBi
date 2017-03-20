using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToParameterStartValuesBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IParameterStartValuesBuildingBlock>
   {
      public CreateCommitChangesToParameterStartValuesBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.ParameterStartValues)
      {
      }

      public override IMoBiCommand CreateCommitToBuildingBlockCommand(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         var macroCommand = new MoBiMacroCommand();
         macroCommand.Add(base.CreateCommitToBuildingBlockCommand(simulation, templateBuildingBlock));
         //hide this command that is only required for separation of concerns
         macroCommand.Add(new ResetParamterValuesToDefaultFromStartValuesInSimulationCommand(simulation) {Visible = false});
         return macroCommand;
      }
   }
}