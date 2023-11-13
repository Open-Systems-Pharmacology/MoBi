using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForSimulation : IInteractionTasksForChildren<MoBiProject, IMoBiSimulation>
   {
      IMoBiCommand CreateSimulation();
      IMoBiCommand AddToProject(IMoBiSimulation simulation);

      /// <summary>
      ///    Removes <paramref name="simulations" /> from the current project
      /// </summary>
      /// <returns>The executed command used to perform the removal</returns>
      IMoBiCommand RemoveMultipleSimulations(IReadOnlyList<IMoBiSimulation> simulations);

      /// <summary>
      ///    Returns the project building block that matches <paramref name="buildingBlock" />
      /// </summary>
      TBuildingBlock TemplateBuildingBlockFor<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock;

      Module TemplateModuleFor(Module module);
      
      IEnumerable<IBuildingBlock> FindChangedBuildingBlocks(IMoBiSimulation simulation);
      IEnumerable<Module> FindChangedModules(IMoBiSimulation simulation);
   }

   public class InteractionTasksForSimulation : InteractionTasksForChildren<MoBiProject, IMoBiSimulation>, IInteractionTasksForSimulation
   {
      private readonly ISimulationReferenceUpdater _simulationReferenceUpdater;
      private readonly ISimulationFactory _simulationFactory;
      private readonly ITemplateResolverTask _templateResolverTask;

      public InteractionTasksForSimulation(IInteractionTaskContext interactionTaskContext,
         IEditTasksForSimulation editTask,
         ISimulationReferenceUpdater simulationReferenceUpdater,
         ISimulationFactory simulationFactory,
         ITemplateResolverTask templateResolverTask)
         : base(interactionTaskContext, editTask)
      {
         _simulationReferenceUpdater = simulationReferenceUpdater;
         _simulationFactory = simulationFactory;
         _templateResolverTask = templateResolverTask;
      }

      protected override string ObjectName => ObjectTypes.Simulation;

      protected override IMoBiCommand RunRemoveCommand(IMoBiSimulation simulationToRemove, MoBiProject parent, IBuildingBlock buildingBlock)
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

      public TBuildingBlock TemplateBuildingBlockFor<TBuildingBlock>(TBuildingBlock buildingBlock) where TBuildingBlock : class, IBuildingBlock
      {
         // In the repository, there should always be exactly one template match. A template match requires
         // building block name/type and module name match. There could be multiple building blocks with the
         // same name and type but they would have to have different parent modules. For building blocks
         // without a parent module, two building blocks cannot have the same name and type
         return _templateResolverTask.TemplateBuildingBlockFor(buildingBlock);
      }

      public Module TemplateModuleFor(Module module)
      {
         return _templateResolverTask.TemplateModuleFor(module);
      }

      public IMoBiCommand CreateSimulation()
      {
         var simulation = createSimulation();
         if (simulation == null)
            return new MoBiEmptyCommand();

         var command = addSimulationToProjectCommand(simulation).Run(_interactionTaskContext.Context);
         _editTask.Edit(simulation);
         return command;
      }

      public IMoBiCommand AddToProject(IMoBiSimulation simulation)
      {
         return AddTo(simulation, _interactionTaskContext.Context.CurrentProject, null);
      }

      private IMoBiSimulation createSimulation()
      {
         using (var presenter = ApplicationController.Start<ICreateSimulationConfigurationPresenter>())
         {
            var moBiSimulation = _simulationFactory.Create();
            initializeDefaultIndividual(moBiSimulation);
            var simulationConfiguration = presenter.CreateBasedOn(moBiSimulation);

            if (simulationConfiguration == null)
               return null;


            return _simulationFactory.CreateSimulationAndValidate(simulationConfiguration, presenter.SimulationName);
         }
      }

      private void initializeDefaultIndividual(IMoBiSimulation moBiSimulation)
      {
         moBiSimulation.Configuration.Individual = _interactionTaskContext.Context.CurrentProject.IndividualsCollection.FirstOrDefault();
      }

      public override IMoBiCommand AddNew(MoBiProject moBiProject, IBuildingBlock buildingBlockToAddTo)
      {
         return addSimulationToProjectCommand(createSimulation()).Run(_interactionTaskContext.Context);
      }

      public override IMoBiCommand GetRemoveCommand(IMoBiSimulation transportBuilderToRemove, MoBiProject project, IBuildingBlock buildingBlock)
      {
         return new RemoveSimulationCommand(transportBuilderToRemove);
      }

      public override IMoBiCommand GetAddCommand(IMoBiSimulation simulation, MoBiProject project, IBuildingBlock buildingBlock)
      {
         return addSimulationToProjectCommand(simulation);
      }

      private IMoBiCommand addSimulationToProjectCommand(IMoBiSimulation simulation)
      {
         if (simulation == null)
            return new MoBiEmptyCommand();

         return new AddSimulationCommand(simulation);
      }

      public IEnumerable<IBuildingBlock> FindChangedBuildingBlocks(IMoBiSimulation simulation)
      {
         return simulation.BuildingBlocks().Where(buildingBlock => TemplateBuildingBlockFor(buildingBlock).Version != buildingBlock.Version);
      }

      public IEnumerable<Module> FindChangedModules(IMoBiSimulation simulation)
      {
         return simulation.Configuration.ModuleConfigurations.
            Where(moduleConfiguration => !versionMatch(TemplateModuleFor(moduleConfiguration.Module), moduleConfiguration)).
            Select(moduleConfiguration => moduleConfiguration.Module);
      }

      private bool versionMatch(Module templateModule, ModuleConfiguration moduleConfiguration)
      {
         var simulationModule = moduleConfiguration.Module;
         var templateInitialConditions = TemplateBuildingBlockFor(moduleConfiguration.SelectedInitialConditions);
         var templateParameterValues = TemplateBuildingBlockFor(moduleConfiguration.SelectedParameterValues);

         return string.Equals(
            templateModule.VersionWith(templateParameterValues, templateInitialConditions),
            simulationModule.VersionWith(moduleConfiguration.SelectedParameterValues, moduleConfiguration.SelectedInitialConditions));
      }

   }
}