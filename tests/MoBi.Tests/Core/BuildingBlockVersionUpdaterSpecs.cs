using System.Collections.Generic;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;

namespace MoBi.Core
{
   public abstract class concern_for_BuildingBlockVersionUpdater : ContextSpecification<IBuildingBlockVersionUpdater>
   {
      protected IMoBiProjectRetriever _projectRetriever;
      protected IEventPublisher _eventPublisher;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _dialogCreator = A.Fake<IDialogCreator>();
         sut = new BuildingBlockVersionUpdater(_projectRetriever, _eventPublisher, _dialogCreator);
      }
   }

   internal class When_the_building_block_version_updater_is_updating_the_building_block_version_used_in_a_building_block : concern_for_BuildingBlockVersionUpdater
   {
      private IBuildingBlock _changeBuildingBlock;
      private IList<IModelCoreSimulation> _affectedSimulations;
      private IMoBiSimulation _affectedSimulation;
      private readonly uint _targetVersion = 4;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _changeBuildingBlock = new ParameterValuesBuildingBlock();
         _changeBuildingBlock.Version = 3;
         _changeBuildingBlock.Id = "TRALLLLA";
         _affectedSimulations = new List<IModelCoreSimulation>();
         _affectedSimulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _affectedSimulation.Uses(_changeBuildingBlock)).Returns(true);

         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>._)).Invokes((call =>
         {
            var statusEvent = call.GetArgument<SimulationStatusChangedEvent>(0);
            _affectedSimulations.Add(statusEvent.Simulation);
         }));

         var project = DomainHelperForSpecs.NewProject();
         project.AddSimulation(_affectedSimulation);
         _module = new Module { _changeBuildingBlock };
         _module.PKSimVersion = "1";
         _module.ModuleImportVersion = _module.Version;
         _module.IsPKSimModule = true;
         project.AddModule(_module);
         A.CallTo(() => _projectRetriever.Current).Returns(project);
      }

      protected override void Because()
      {
         sut.UpdateBuildingBlockVersion(_changeBuildingBlock, true, true);
      }

      [Observation]
      public void should_increment_building_block_version()
      {
         _changeBuildingBlock.Version.ShouldBeEqualTo(_targetVersion);
      }

      [Observation]
      public void should_publish_module_status_changed_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<ModuleStatusChangedEvent>.That.Matches(x => x.Module.Equals(_module)))).MustHaveHappened();
      }

      [Observation]
      public void should_publish_simulation_status_changed_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>._)).MustHaveHappened();
         _affectedSimulations.ShouldOnlyContain(_affectedSimulation);
      }

      [Observation]
      public void the_dialog_creator_warns_user_that_the_pk_sim_module_will_be_converted()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Captions.TheModuleWillBeConvertedFromPKSimToExtensionModule(_module.Name))).MustHaveHappened();
      }

      [Observation]
      public void module_is_pksim_property_should_be_changed_to_false()
      {
         _module.IsPKSimModule.ShouldBeFalse();
      }
   }
}