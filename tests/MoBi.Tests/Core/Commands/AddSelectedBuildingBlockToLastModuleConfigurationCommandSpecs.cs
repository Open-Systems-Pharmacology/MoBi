using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_AddSelectedBuildingBlockToLastModuleConfigurationCommand : ContextSpecification<AddSelectedBuildingBlockToLastModuleConfigurationCommand<ParameterValuesBuildingBlock>>
   {
      protected IMoBiSimulation _simulation;
      protected ParameterValuesBuildingBlock _buildingBlock;
      protected IMoBiContext _context;
      protected Module _module;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _simulation = new MoBiSimulation().WithId("simulationId");
         _simulation.Configuration = new SimulationConfiguration();
         _module = new Module().WithId("moduleId");
         _simulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(_module));
         _buildingBlock = new ParameterValuesBuildingBlock().WithId("parameterValuesBuildingBlockId");
         A.CallTo(() => _context.Get<Module>(_module.Id)).Returns(_module);
         A.CallTo(() => _context.Get<ParameterValuesBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
         sut = new AddSelectedBuildingBlockToLastModuleConfigurationCommand<ParameterValuesBuildingBlock>(_buildingBlock, _simulation);
      }
   }

   public class When_adding_a_selected_parameter_values_to_the_last_module : concern_for_AddSelectedBuildingBlockToLastModuleConfigurationCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_block_should_be_added_to_the_module()
      {
         _simulation.Configuration.ModuleConfigurations.Last().Module.BuildingBlocks.ShouldContain(_buildingBlock);
      }

      [Observation]
      public void the_added_building_block_should_be_selected()
      {
         _simulation.Configuration.ModuleConfigurations.Last().SelectedParameterValues.ShouldBeEqualTo(_buildingBlock);
      }
   }

   public class When_reverting_the_add_selected_parameter_values_command : concern_for_AddSelectedBuildingBlockToLastModuleConfigurationCommand
   {
      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_building_block_should_be_removed_from_the_module()
      {
         _simulation.Configuration.ModuleConfigurations.Last().Module.BuildingBlocks.ShouldNotContain(_buildingBlock);
      }

      [Observation]
      public void the_module_configuration_should_not_have_selected_building_block()
      {
         _simulation.Configuration.ModuleConfigurations.Last().SelectedParameterValues.ShouldBeNull();
      }
   }
}
