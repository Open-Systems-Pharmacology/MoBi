using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Helpers;

namespace MoBi.Core
{
   public abstract class concern_for_MoBiProject : ContextSpecification<MoBiProject>
   {
      protected override void Context()
      {
         sut = DomainHelperForSpecs.NewProject();
      }
   }

  


   public class When_checking_if_a_project_is_empty : concern_for_MoBiProject
   {
      [Observation]
      public void should_return_true_if_there_are_no_simulation_and_building_block()
      {
         sut.IsEmpty.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.AddIndividualBuildingBlock(A.Fake<IndividualBuildingBlock>());
         sut.IsEmpty.ShouldBeFalse();

         sut = DomainHelperForSpecs.NewProject();
         sut.AddSimulation(A.Fake<IMoBiSimulation>());
         sut.IsEmpty.ShouldBeFalse();
      }
   }

   public class When_resolving_the_list_of_simulations_created_using_a_given_building_block : concern_for_MoBiProject
   {
      private IMoBiSimulation _sim1;
      private IMoBiSimulation _sim2;
      private IBuildingBlock _templateBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _templateBuildingBlock = A.Fake<IBuildingBlock>();
         _sim1 = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _sim1.IsCreatedBy(_templateBuildingBlock)).Returns(true);
         _sim2 = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _sim2.IsCreatedBy(_templateBuildingBlock)).Returns(false);
         sut.AddSimulation(_sim1);
         sut.AddSimulation(_sim2);
      }

      [Observation]
      public void should_return_only_the_simulation_created_with_this_reference()
      {
         sut.SimulationsCreatedUsing(_templateBuildingBlock).ShouldOnlyContain(_sim1);
      }
   }

   public class When_removing_a_simulation : concern_for_MoBiProject
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation().WithId("Id");
         sut.AddSimulation(_simulation);
         sut.GetOrCreateClassifiableFor<ClassifiableSimulation, IMoBiSimulation>(_simulation);
         sut.AllClassifiables.Count.ShouldBeEqualTo(1);
      }

      protected override void Because()
      {
         sut.RemoveSimulation(_simulation);
      }

      [Observation]
      public void should_also_remove_the_associated_classifiable_wrapper()
      {
         sut.AllClassifiables.Count.ShouldBeEqualTo(0);
      }
   }
}