using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

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

   public class When_finding_simulations_that_use_a_module : concern_for_MoBiProject
   {
      private IReadOnlyList<IMoBiSimulation> _result;
      private IMoBiSimulation _moBiSimulation1;
      private IMoBiSimulation _moBiSimulation2;
      private IMoBiSimulation _moBiSimulation3;

      protected override void Context()
      {
         base.Context();
         _moBiSimulation1 = createSimulationWithModuleNamed("module1").WithName("simulation1");
         sut.AddSimulation(_moBiSimulation1);
         _moBiSimulation2 = createSimulationWithModuleNamed("module1").WithName("simulation2");
         sut.AddSimulation(_moBiSimulation2);
         _moBiSimulation3 = createSimulationWithModuleNamed("module2").WithName("simulation2");
         sut.AddSimulation(_moBiSimulation3);
      }

      private IMoBiSimulation createSimulationWithModuleNamed(string moduleName)
      {
         var simulation = new MoBiSimulation
         {
            Configuration = new SimulationConfiguration()
         };
         simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(new Module().WithName(moduleName).WithId("id")));
         return simulation;
      }

      protected override void Because()
      {
         _result = sut.SimulationsUsing(new Module().WithName("module1"));
      }

      [Observation]
      public void the_simulation_list_includes_all_simulations_using_a_module_with_the_same_name()
      {
         _result.ShouldContain(_moBiSimulation1);
         _result.ShouldContain(_moBiSimulation2);
         _result.ShouldNotContain(_moBiSimulation3);
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
         A.CallTo(() => _sim1.Uses(_templateBuildingBlock)).Returns(true);
         _sim2 = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _sim2.Uses(_templateBuildingBlock)).Returns(false);
         sut.AddSimulation(_sim1);
         sut.AddSimulation(_sim2);
      }

      [Observation]
      public void should_return_only_the_simulation_created_with_this_reference()
      {
         sut.SimulationsUsing(_templateBuildingBlock).ShouldOnlyContain(_sim1);
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