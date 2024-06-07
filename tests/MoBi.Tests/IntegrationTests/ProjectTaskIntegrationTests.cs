using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public class concern_for_LoadProjectIntegrationTest : ContextWithLoadedProject
   {
      private IMoBiContext _context;
      private ObjectTypeResolver _objectTypeResolver;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _objectTypeResolver = new ObjectTypeResolver();
         _context = IoC.Resolve<IMoBiContext>();
      }
   }

   public class When_Loading_a_Project_with_Unique_Building_Block_Types : concern_for_LoadProjectIntegrationTest
   {
      private IMoBiContext _context;
      private ObjectTypeResolver _objectTypeResolver;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _objectTypeResolver = new ObjectTypeResolver();
         _context = IoC.Resolve<IMoBiContext>();
         LoadProject("PK_Manual_Diclofenac");
      }

      [Observation]
      public void should_be_able_to_load_Diclofenac_model()
      {
         _context.CurrentProject.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_set_the_name_of_the_project_to_the_name_of_the_file()
      {
         _context.CurrentProject.Name.ShouldBeEqualTo("PK_Manual_Diclofenac");
      }

      [Observation]
      public void loaded_building_blocks_should_each_be_contained_in_their_own_module()
      {
         _context.CurrentProject.Modules.Count.ShouldBeEqualTo(1);
         _context.CurrentProject.Modules.Each(x => x.BuildingBlocks.Count.ShouldBeEqualTo(8));
      }

      [Observation]
      public void simulations_should_use_modules_from_the_project_to_build_configurations()
      {
         _context.CurrentProject.Simulations.Each(matchSimulationAndProjectModules);
      }

      private void matchSimulationAndProjectModules(IMoBiSimulation simulation)
      {
         simulation.Modules.Each(x => matchSimulationAndProjectModule(x, _context.CurrentProject.Modules));
      }

      private void matchSimulationAndProjectModule(Module module, IReadOnlyList<Module> projectModules)
      {
         projectModules.SingleOrDefault(x => x.IsNamed(module.Name)).ShouldNotBeNull();
      }
   }

   public class When_Loading_a_Project_with_Duplicated_Building_Block_Types : concern_for_LoadProjectIntegrationTest
   {
      private IMoBiContext _context;
      private ObjectTypeResolver _objectTypeResolver;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _objectTypeResolver = new ObjectTypeResolver();
         _context = IoC.Resolve<IMoBiContext>();
         LoadProject("Duplicate_BuildingBlock_Types");
      }

      [Observation]
      public void should_be_able_to_load_Diclofenac_model()
      {
         _context.CurrentProject.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_set_the_name_of_the_project_to_the_name_of_the_file()
      {
         _context.CurrentProject.Name.ShouldBeEqualTo("Duplicate_BuildingBlock_Types");
      }

      [Observation]
      public void loaded_building_blocks_should_each_be_contained_in_their_own_module()
      {
         _context.CurrentProject.Modules.Count.ShouldBeEqualTo(9);
         _context.CurrentProject.Modules.Each(x => x.BuildingBlocks.Count.ShouldBeEqualTo(1));
      }

      [Observation]
      public void simulations_should_use_modules_from_the_project_to_build_configurations()
      {
         _context.CurrentProject.Simulations.Each(matchSimulationAndProjectModules);
      }

      private void matchSimulationAndProjectModules(IMoBiSimulation simulation)
      {
         simulation.Modules.Each(x => matchSimulationAndProjectModule(x, _context.CurrentProject.Modules));
      }

      private void matchSimulationAndProjectModule(Module module, IReadOnlyList<Module> projectModules)
      {
         projectModules.SingleOrDefault(x => x.IsNamed(module.Name)).ShouldNotBeNull();
      }


      [Observation]
      public void each_module_should_be_named_for_the_building_block_and_building_block_type()
      {
         _context.CurrentProject.Modules.Each(x => x.Name.Contains(x.BuildingBlocks[0].Name).ShouldBeTrue());
         _context.CurrentProject.Modules.Each(x => x.Name.Contains(_objectTypeResolver.TypeFor(x.BuildingBlocks[0]).Replace("Building Block", string.Empty)).ShouldBeTrue());
      }

   }


}