using System;
using Castle.MicroKernel;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Container.Castle;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public abstract class concern_for_BuildingBlockTaskRetrieverIntegration : ContextForIntegration<BuildingBlockTaskRetriever>
   {
      protected IKernel _kernel;
      protected Type _requestedServiceType;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _kernel = IoC.Container.DowncastTo<CastleWindsorContainer>().WindsorContainer.Kernel;
      }

      protected override void Context()
      {
         //The interaction tasks cannot be fully resolved in the integration container (UI services are missing),
         //so we only record the service type requested by the retriever and verify that it is registered
         var container = A.Fake<IContainer>();
         A.CallTo(container)
            .Where(call => call.Method.Name == nameof(IContainer.Resolve) && call.Method.IsGenericMethod && call.Arguments.Count == 0, _ => { })
            .WithNonVoidReturnType()
            .ReturnsLazily(call =>
            {
               _requestedServiceType = call.Method.GetGenericArguments()[0];
               return null;
            });

         sut = new BuildingBlockTaskRetriever(container);
      }
   }

   public class When_retrieving_the_interaction_task_for_every_building_block_shown_under_a_simulation : concern_for_BuildingBlockTaskRetrieverIntegration
   {
      //every building block type that can appear under a simulation in the simulation explorer
      private static readonly Type[] _buildingBlockTypes =
      {
         typeof(MoBiSpatialStructure),
         typeof(MoleculeBuildingBlock),
         typeof(MoBiReactionBuildingBlock),
         typeof(PassiveTransportBuildingBlock),
         typeof(ObserverBuildingBlock),
         typeof(EventGroupBuildingBlock),
         typeof(InitialConditionsBuildingBlock),
         typeof(ParameterValuesBuildingBlock),
         typeof(IndividualBuildingBlock),
         typeof(ExpressionProfileBuildingBlock),
      };

      [Observation]
      [TestCaseSource(nameof(_buildingBlockTypes))]
      public void should_request_a_service_that_is_registered_in_the_container_for(Type buildingBlockType)
      {
         _requestedServiceType = null;
         sut.TaskFor((IBuildingBlock) Activator.CreateInstance(buildingBlockType));

         _requestedServiceType.ShouldNotBeNull();
         _kernel.HasComponent(_requestedServiceType).ShouldBeTrue($"{_requestedServiceType} is not registered");
      }
   }
}
