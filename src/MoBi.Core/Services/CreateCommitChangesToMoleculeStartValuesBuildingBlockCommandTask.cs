using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToMoleculeStartValuesBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IMoleculeStartValuesBuildingBlock>
   {
      public CreateCommitChangesToMoleculeStartValuesBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.MoleculeStartValues)
      {

      }

      public override IMoBiCommand CreateCommitToBuildingBlockCommand(IMoBiSimulation simulation, IBuildingBlock templateBuildingBlock)
      {
         var macroCommand = new MoBiMacroCommand();
         macroCommand.Add(CreateCommitCommand(simulation, templateBuildingBlock));
         //hide this command that is only required for separation of concerns
         macroCommand.Add(new ResetMoleculeValuesToDefaultFromStartValuesInSimulationCommand(simulation) {Visible = false});
         return macroCommand;
      }
   }
}