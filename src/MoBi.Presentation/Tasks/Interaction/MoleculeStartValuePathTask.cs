using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IMoleculeStartValuePathTask : IStartValuePathTask<MoleculeStartValuesBuildingBlock, MoleculeStartValue>
   {
   }

   public class MoleculeStartValuePathTask : AbstractStartValuePathTask<MoleculeStartValuesBuildingBlock, MoleculeStartValue>, IMoleculeStartValuePathTask
   {
      public MoleculeStartValuePathTask(IFormulaTask formulaTask, IMoBiContext context) : base(formulaTask,context)
      {
      }

      public override IMoBiCommand UpdateStartValueNameCommand(MoleculeStartValuesBuildingBlock startValues, MoleculeStartValue startValue, string newValue)
      {
         return new ChangeMoleculeStartValueNameCommand(startValues, startValue.Path, newValue);
      }

      public override IMoBiCommand UpdateStartValueContainerPathCommand(MoleculeStartValuesBuildingBlock buildingBlock, MoleculeStartValue startValue, int indexToUpdate, string newValue)
      {
         var targetPath = startValue.ContainerPath.Clone<ObjectPath>();

         if (indexToUpdate > targetPath.Count)
            return new MoBiEmptyCommand();

         ConfigureTargetPath(indexToUpdate, newValue, targetPath);

         return new EditMoleculeStartValuePathCommand(buildingBlock, startValue, targetPath);
      }
   }
}