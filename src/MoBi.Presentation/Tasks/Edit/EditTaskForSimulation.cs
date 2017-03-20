using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTaskForSimulation : IEditTaskFor<IMoBiSimulation>
   {
   }

   public class EditTaskForSimulation : EditTaskFor<IMoBiSimulation>, IEditTaskForSimulation
   {
      private readonly IParameterIdentificationSimulationPathUpdater _parameterIdentificationSimulationPathUpdater;

      public EditTaskForSimulation(IInteractionTaskContext interactionTaskContext, IParameterIdentificationSimulationPathUpdater parameterIdentificationSimulationPathUpdater) : base(interactionTaskContext)
      {
         _parameterIdentificationSimulationPathUpdater = parameterIdentificationSimulationPathUpdater;
      }

      protected override IEnumerable<string> GetUnallowedNames(IMoBiSimulation simulation, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return _context.CurrentProject.Simulations.Select(bb => bb.Name);
      }

      public override void Rename(IMoBiSimulation simulationToRename, IEnumerable<IObjectBase> existingObjectsInParent, IBuildingBlock buildingBlock)
      {
         string oldName = simulationToRename.Name;
         var forbiddenNames = GetForbiddenNames(simulationToRename, _context.CurrentProject.Simulations);
         var renameCommmand = _interactionTask.Rename(simulationToRename, forbiddenNames, buildingBlock);
         if (renameCommmand.IsEmpty()) return;
         _context.AddToHistory(renameCommmand);
         _parameterIdentificationSimulationPathUpdater.UpdatePathsForRenamedSimulation(simulationToRename, oldName, simulationToRename.Name);
         simulationToRename.HasChanged = true;
      }
   }
}