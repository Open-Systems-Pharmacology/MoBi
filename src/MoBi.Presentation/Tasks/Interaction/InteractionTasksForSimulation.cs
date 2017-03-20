using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Assets;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForSimulation : IInteractionTasksForChildren<IMoBiProject, IMoBiSimulation>
   {
      IMoBiCommand CreateSimulation();
      IMoBiCommand AddToProject(IMoBiSimulation simulation);

      /// <summary>
      ///    Removes <paramref name="simulations" /> from the current project
      /// </summary>
      /// <returns>The executed command used to perform the removal</returns>
      IMoBiCommand RemoveMultipleSimulations(IReadOnlyList<IMoBiSimulation> simulations);
   }

   public class InteractionTasksForSimulation : InteractionTasksForChildren<IMoBiProject, IMoBiSimulation>, IInteractionTasksForSimulation
   {
      private readonly ISimulationReferenceUpdater _simulationReferenceUpdater;

      public InteractionTasksForSimulation(IInteractionTaskContext interactionTaskContext, IEditTasksForSimulation editTask, ISimulationReferenceUpdater simulationReferenceUpdater)
         : base(interactionTaskContext, editTask)
      {
         _simulationReferenceUpdater = simulationReferenceUpdater;
      }

      protected override string ObjectName => ObjectTypes.Simulation;

      protected override IMoBiCommand RunRemoveCommand(IMoBiSimulation simulationToRemove, IMoBiProject parent, IBuildingBlock buildingBlock)
      {
         _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(simulationToRemove);

         return base.RunRemoveCommand(simulationToRemove, parent, buildingBlock);
      }

      public IMoBiCommand RemoveMultipleSimulations(IReadOnlyList<IMoBiSimulation> simulations)
      {
         var currentProject = _interactionTaskContext.Context.CurrentProject;
         if (DialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveSimulationsFromProject(currentProject.Name)) != ViewResult.Yes)
            return new MoBiEmptyCommand();

         simulations.Each(simulation => _simulationReferenceUpdater.RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(simulation));

         var macroCommand = new MoBiMacroCommand
         {
            Description = AppConstants.Commands.RemoveSimulationsFromProject,
            ObjectType = ObjectTypes.Simulation,
            CommandType = AppConstants.Commands.DeleteCommand
         };

         simulations.Each(simulation =>
         {
            macroCommand.AddCommand(GetRemoveCommand(simulation, currentProject, null).Run(Context));
            Context.PublishEvent(new RemovedEvent(simulation, currentProject));
         });

         return macroCommand;
      }

      public IMoBiCommand CreateSimulation()
      {
         var simulation = createSimulation();
         if (simulation == null)
            return new MoBiEmptyCommand();

         var command = addSimulationToProjectCommmand(simulation).Run(_interactionTaskContext.Context);
         _editTask.Edit(simulation);
         return command;
      }

      public IMoBiCommand AddToProject(IMoBiSimulation simulation)
      {
         return AddToProject(simulation, _interactionTaskContext.Context.CurrentProject, null);
      }

      private IMoBiSimulation createSimulation()
      {
         using (var presenter = ApplicationController.Start<ICreateSimulationPresenter>())
         {
            return presenter.Create();
         }
      }

      public override IMoBiCommand AddNew(IMoBiProject moBiProject, IBuildingBlock buildingBlockToAddTo)
      {
         return addSimulationToProjectCommmand(createSimulation()).Run(_interactionTaskContext.Context);
      }

      public override IMoBiCommand GetRemoveCommand(IMoBiSimulation transportBuilderToRemove, IMoBiProject project, IBuildingBlock buildingBlock)
      {
         return new RemoveSimulationCommand(transportBuilderToRemove);
      }

      public override IMoBiCommand GetAddCommand(IMoBiSimulation simulation, IMoBiProject project, IBuildingBlock buildingBlock)
      {
         return addSimulationToProjectCommmand(simulation);
      }

      private IMoBiCommand addSimulationToProjectCommmand(IMoBiSimulation simulation)
      {
         if (simulation == null)
            return new MoBiEmptyCommand();

         return new AddSimulationCommand(simulation);
      }
   }
}