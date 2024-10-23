using System.Collections.Generic;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Interaction;
using MoBi.Presentation.Tasks.Interaction.MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public class concern_for_InteractionTasksForMultipleBuildingBlocks : ContextSpecification<InteractionTasksForMultipleBuildingBlocks>
   {
      protected IInteractionTasksForEventBuildingBlock _interactionTasksForEventGroupBuildingBlock;
      protected IInitialConditionsTask<InitialConditionsBuildingBlock> _interactionTasksForInitialConditionBuildingBlock;
      protected IInteractionTasksForMoleculeBuildingBlock _interactionTasksForMoleculeBuildingBlock;
      protected IInteractionTasksForPassiveTransportBuildingBlock _interactionTasksForPassiveTransportBuildingBlock;
      protected IParameterValuesTask _interactionTasksForParameterValues;
      protected IInteractionTasksForReactionBuildingBlock _interactionTasksForMobiReactionBuildingBlock;
      protected IInteractionTasksForSpatialStructure _interactionTasksForMobiSpatialStructureBuildingBlock;
      protected IInteractionTasksForObserverBuildingBlock _interactionTasksForObserverBuildingBlock;
      protected IInteractionTasksForIndividualBuildingBlock _interactionTasksForIndividualBuildingBlock;
      protected IInteractionTasksForExpressionProfileBuildingBlock _interactionTasksForExpressionProfileBuildingBlock;
      protected IInteractionTaskContext _interactionTaskContext;
      protected IInteractionTaskContext _context;
      protected EventGroupBuildingBlock _eventGroupBuildingBlockUsedInSimulation;
      protected MoBiSpatialStructure _spatialStructureBuildingBlockUsedInSimulation;
      protected EventGroupBuildingBlock _eventGroupBuildingBlockNotUsedInSimulation;
      protected MoBiSpatialStructure _spatialStructureBuildingBlockNotUsedInSimulation;

      private MoBiProject _project;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         _project = DomainHelperForSpecs.NewProject();
         _context = A.Fake<IInteractionTaskContext>();
         sut = new InteractionTasksForMultipleBuildingBlocks(
            _interactionTasksForEventGroupBuildingBlock,
            _interactionTasksForInitialConditionBuildingBlock,
            _interactionTasksForMoleculeBuildingBlock,
            _interactionTasksForPassiveTransportBuildingBlock,
            _interactionTasksForParameterValues,
            _interactionTasksForMobiReactionBuildingBlock,
            _interactionTasksForMobiSpatialStructureBuildingBlock,
            _interactionTasksForObserverBuildingBlock,
            _interactionTasksForIndividualBuildingBlock,
            _interactionTasksForExpressionProfileBuildingBlock,
            _interactionTaskContext
         );
         A.CallTo(() => _context.DialogCreator.MessageBoxYesNo(A<string>.Ignored, A<ViewResult>.Ignored)).Returns(ViewResult.Yes);

         _simulation = new MoBiSimulation().WithName("simulation");
         _simulation.Configuration = new SimulationConfiguration();
         var moduleUsedOnSimulation = new Module().WithName("moduleNameUsedInSimulation");
         var moduleNotUsedInSimulation = new Module().WithName("moduleNameNotUsedInSimulation");
         _eventGroupBuildingBlockUsedInSimulation = new EventGroupBuildingBlock().WithId("newEventGroupBuildingBlockId");
         _spatialStructureBuildingBlockUsedInSimulation = new MoBiSpatialStructure().WithId("newSpatialStructureId");
         _eventGroupBuildingBlockNotUsedInSimulation = new EventGroupBuildingBlock().WithId("newEventGroupBuildingBlockIdNotUsed");
         _spatialStructureBuildingBlockNotUsedInSimulation = new MoBiSpatialStructure().WithId("newSpatialStructureIdNotUsed");

         moduleUsedOnSimulation.Add(_eventGroupBuildingBlockUsedInSimulation);
         moduleUsedOnSimulation.Add(_spatialStructureBuildingBlockUsedInSimulation);
         moduleNotUsedInSimulation.Add(_eventGroupBuildingBlockNotUsedInSimulation);
         moduleNotUsedInSimulation.Add(_spatialStructureBuildingBlockNotUsedInSimulation);
         _project.AddModule(moduleNotUsedInSimulation);
         _simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(moduleUsedOnSimulation));
         _project.AddSimulation(_simulation);

         A.CallTo(() => _context.Context.CurrentProject).Returns(_project);
      }
   }

   public class When_removing_two_buildingBlocks_used_in_simulation : concern_for_InteractionTasksForMultipleBuildingBlocks
   {
      protected override void Because()
      {
         var lst = new List<IBuildingBlock> { _eventGroupBuildingBlockUsedInSimulation, _spatialStructureBuildingBlockUsedInSimulation };
         sut.RemoveBuildingBlocks(lst);
      }

      [Observation]
      public void buildingBlocks_should_not_be_removed()
      {
         A.CallTo(() => _interactionTasksForEventGroupBuildingBlock.Remove(_eventGroupBuildingBlockUsedInSimulation, A<Module>.Ignored, A<IBuildingBlock>.Ignored, A<bool>.Ignored)).MustNotHaveHappened();
         A.CallTo(() => _interactionTasksForMobiSpatialStructureBuildingBlock.Remove(_spatialStructureBuildingBlockUsedInSimulation, A<Module>.Ignored, A<IBuildingBlock>.Ignored, A<bool>.Ignored)).MustNotHaveHappened();
      }

      [Observation]
      public void message_with_information_about_used_buildingBlocks_should_be_shown()
      {
         A.CallTo(() => _context.DialogCreator.MessageBoxInfo(A<string>.That.Contains(AppConstants.Dialog.BuildingBlocksUsedInSimulation))).MustHaveHappened();
         A.CallTo(() => _context.DialogCreator.MessageBoxInfo(A<string>.That.Contains(_eventGroupBuildingBlockUsedInSimulation.Name))).MustHaveHappened();
         A.CallTo(() => _context.DialogCreator.MessageBoxInfo(A<string>.That.Contains(_spatialStructureBuildingBlockUsedInSimulation.Name))).MustHaveHappened();
      }
   }

   public class When_removing_two_buildingBlocks_not_used_in_simulation : concern_for_InteractionTasksForMultipleBuildingBlocks
   {
      protected override void Because()
      {
         var lst = new List<IBuildingBlock> { _eventGroupBuildingBlockNotUsedInSimulation, _spatialStructureBuildingBlockNotUsedInSimulation };
         sut.RemoveBuildingBlocks(lst);
      }

      [Observation]
      public void buildingBlocks_should_be_removed()
      {
         A.CallTo(() => _interactionTasksForEventGroupBuildingBlock.Remove(_eventGroupBuildingBlockNotUsedInSimulation, A<Module>.Ignored, A<IBuildingBlock>.Ignored, A<bool>.Ignored)).MustHaveHappened();
         A.CallTo(() => _interactionTasksForMobiSpatialStructureBuildingBlock.Remove(_spatialStructureBuildingBlockNotUsedInSimulation, A<Module>.Ignored, A<IBuildingBlock>.Ignored, A<bool>.Ignored)).MustHaveHappened();
      }
   }
}