using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Events;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Application
{
   public abstract class concern_for_BuildConfigurationUpdater : ContextSpecification<IBuildConfigurationUpdater>
   {
      private IEventPublisher _eventPublisher;
      protected IAffectedBuildingBlockRetriever _affectedBuildingBlockRetriever;

      protected override void Context()
      {
         _affectedBuildingBlockRetriever = A.Fake<IAffectedBuildingBlockRetriever>();
         _eventPublisher = A.Fake<IEventPublisher>();
         sut = new BuildConfigurationUpdater(_affectedBuildingBlockRetriever, _eventPublisher);
      }
   }

   public class When_the_build_configuration_updater_is_updating_the_configuration_of_a_simulation_based_on_the_changed_on_a_given_object : concern_for_BuildConfigurationUpdater
   {
      private IMoBiSimulation _simulation;
      private IBuildingBlockInfo _buildingBlockInfo;
      private object _changedQuantity;

      protected override void Context()
      {
         base.Context();
         var buildConfiguration = A.Fake<IMoBiBuildConfiguration>();
         var affectedBuildingBlock = new EventGroupBuildingBlock();
         var eventGroupBuildingBlockInfo = new EventGroupBuildingBlockInfo() {BuildingBlock = affectedBuildingBlock};
         _buildingBlockInfo = eventGroupBuildingBlockInfo;
         _simulation = new MoBiSimulation {BuildConfiguration = buildConfiguration, Model = A.Fake<IModel>()};
         _changedQuantity = A.Fake<IQuantity>();
         A.CallTo(() => _affectedBuildingBlockRetriever.RetrieveFor(A<IQuantity>._, _simulation)).Returns(eventGroupBuildingBlockInfo);
      }

      protected override void Because()
      {
         sut.UpdateBuildingConfiguration(_changedQuantity, _simulation, incrementSimulationChange: true);
      }

      [Observation]
      public void should_set_the_simulation_changed_flag_in_the_building_block_info_of_simualtion_to_true()
      {
         _buildingBlockInfo.SimulationHasChanged.ShouldBeTrue();
      }

      [Observation]
      public void should_retrieve_affected_buildingblock_from_simlation()
      {
         A.CallTo(() => _affectedBuildingBlockRetriever.RetrieveFor(_changedQuantity, _simulation)).MustHaveHappened();
      }
   }
}