using System.Collections.Generic;
using System.Linq;
using MoBi.Core;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Core.Serialization.Converter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

namespace MoBi.IntegrationTests
{
   public class concern_for_LoadProjectIntegrationTest : ContextWithLoadedProject
   {
      protected IMoBiContext _context;
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
         projectModules.FindByName(module.Name).ShouldNotBeNull();
      }
   }

   public class When_Loading_a_Project_with_Duplicated_Building_Block_Types : concern_for_LoadProjectIntegrationTest
   {
      private ObjectTypeResolver _objectTypeResolver;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _objectTypeResolver = new ObjectTypeResolver();
         _context = IoC.Resolve<IMoBiContext>();
         LoadProject("Duplicate_BuildingBlock_Types");
      }

      [Observation]
      public void should_be_able_to_load_project_model()
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

      public class When_loading_a_V11_project_with_untraceable_changes : concern_for_LoadProjectIntegrationTest
      {
         private MoBiProject _project;
         private IProjectConverterLogger _logger;

         public override void GlobalContext()
         {
            base.GlobalContext();
            _project = LoadProject("V11SimulationWithChanges");
            _logger = _context.Resolve<IProjectConverterLogger>();
         }

         [Observation]
         public void project_conversion_should_flag_the_correct_simulations_with_traceable_changes()
         {
            _project.Simulations.First(x => x.Name.Equals("simulationchange")).HasUntraceableChanges.ShouldBeTrue();
            _project.Simulations.First(x => x.Name.Equals("simulationchanges2")).HasUntraceableChanges.ShouldBeTrue();
            _project.Simulations.First(x => x.Name.Equals("bbchange")).HasUntraceableChanges.ShouldBeFalse();
            _project.Simulations.First(x => x.Name.Equals("nochange")).HasUntraceableChanges.ShouldBeFalse();
         }

         [Observation]
         public void project_conversion_should_log_two_warnings_for_appropriate_simulations()
         {
            var warningSimulations = _logger.AllMessages().Where(x => x.Type == NotificationType.Warning).ToList();
            warningSimulations.Count().ShouldBeEqualTo(2);
            _logger.AllMessages().Count(x => isMatchingSimulations(x, warningSimulations.AllNames())).ShouldBeEqualTo(2);
         }

         private bool isMatchingSimulations(NotificationMessage notificationMessage, IReadOnlyList<string> warningSimulations)
         {
            return notificationMessage.Object is IMoBiSimulation moBiSimulation && warningSimulations.Contains(moBiSimulation.Name);
         }
      }
   }
}