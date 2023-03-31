﻿using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Events;

namespace MoBi.Core
{
   public abstract class concern_for_BuildingBlockVersionUpdater : ContextSpecification<IBuildingBlockVersionUpdater>
   {
      protected IMoBiProjectRetriever _projectRetriever;
      protected IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _projectRetriever= A.Fake<IMoBiProjectRetriever>();
         _eventPublisher= A.Fake<IEventPublisher>();
         sut = new BuildingBlockVersionUpdater(_projectRetriever,_eventPublisher);
      }
   }

   internal class When_the_building_block_version_updater_is_updating_the_building_block_version_used_in_a_building_block : concern_for_BuildingBlockVersionUpdater
   {
      private IBuildingBlock _changeBuildingBlock;
      private IList<IModelCoreSimulation> _affectedSimulations;
      private IMoBiSimulation _affectedSimulation;
      private readonly uint _targetVersion = 4;

      protected override void Context()
      {
         base.Context();
         _changeBuildingBlock = A.Fake<IBuildingBlock>();
         _changeBuildingBlock.Version = 3;
         _changeBuildingBlock.Id = "TRALLLLA";
         _affectedSimulations = new List<IModelCoreSimulation>();
         _affectedSimulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _affectedSimulation.IsCreatedBy(_changeBuildingBlock)).Returns(true);
         
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>._)).Invokes((call =>
         {
            var statusEvent = call.GetArgument<SimulationStatusChangedEvent>(0);
            _affectedSimulations.Add(statusEvent.Simulation);
         }));

         var project = DomainHelperForSpecs.NewProject();
         project.AddSimulation(_affectedSimulation);
         A.CallTo(() => _projectRetriever.Current).Returns(project);
      }

      protected override void Because()
      {
         sut.UpdateBuildingBlockVersion(_changeBuildingBlock, true);
      }

      [Observation]
      public void should_increment_building_block_version()
      {
         _changeBuildingBlock.Version.ShouldBeEqualTo(_targetVersion);
      }

      [Observation]
      public void should_publish_simulation_status_changed_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<SimulationStatusChangedEvent>._)).MustHaveHappened();
         _affectedSimulations.ShouldOnlyContain(_affectedSimulation);
      }
   }
}	