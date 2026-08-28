using System;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_RemoveSelectedBuildingBlockFromModuleConfigurationCommand : ContextSpecification<RemoveSelectedBuildingBlockFromModuleConfigurationCommand<InitialConditionsBuildingBlock>>
   {
      protected IMoBiContext _context;
      protected MoBiSimulation _simulation;
      protected Module _module;
      protected InitialConditionsBuildingBlock _buildingBlock;
      protected ModuleConfiguration _moduleConfiguration;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _simulation = new MoBiSimulation().WithId("simulationId");
         _simulation.Configuration = new SimulationConfiguration();
         _module = new Module().WithId("moduleId");
         _buildingBlock = new InitialConditionsBuildingBlock().WithId("parameterValuesBuildingBlockId");
         _module.Add(_buildingBlock);
         _moduleConfiguration = new ModuleConfiguration(_module)
         {
            SelectedInitialConditions = _buildingBlock
         };
         _simulation.Configuration.AddModuleConfiguration(_moduleConfiguration);

         A.CallTo(() => _context.Get<Module>(_module.Id)).Returns(_module);
         A.CallTo(() => _context.Get<InitialConditionsBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
         sut = new RemoveSelectedBuildingBlockFromModuleConfigurationCommand<InitialConditionsBuildingBlock>(_buildingBlock, _moduleConfiguration, _simulation);
      }
   }

   public class When_reverting_the_remove_selected_building_block_command : concern_for_RemoveSelectedBuildingBlockFromModuleConfigurationCommand
   {
      private InitialConditionsBuildingBlock _deserializedBuildingBlock;
      private readonly byte[] _deserializeToken = Array.Empty<byte>();

      protected override void Context()
      {
         base.Context();
         _deserializedBuildingBlock = new InitialConditionsBuildingBlock();
         A.CallTo(() => _context.Serialize(_buildingBlock)).Returns(_deserializeToken);
         A.CallTo(() => _context.Deserialize<InitialConditionsBuildingBlock>(_deserializeToken)).Returns(_deserializedBuildingBlock);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_selected_building_block_should_be_set()
      {
         _moduleConfiguration.SelectedInitialConditions.ShouldBeEqualTo(_deserializedBuildingBlock);
      }

      [Observation]
      public void the_building_block_should_be_added()
      {
         _module.ShouldContain(_deserializedBuildingBlock);
      }
   }

   public class When_removing_the_selected_building_block_module : concern_for_RemoveSelectedBuildingBlockFromModuleConfigurationCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_block_should_be_removed_from_the_module()
      {
         _module.BuildingBlocks.ShouldNotContain(_buildingBlock);
      }

      [Observation]
      public void the_module_configuration_should_not_have_selected_building_block()
      {
         _moduleConfiguration.SelectedInitialConditions.ShouldBeNull();
      }
   }

   public class When_removing_the_selected_building_block_from_a_module_configuration_that_is_not_the_last : concern_for_RemoveSelectedBuildingBlockFromModuleConfigurationCommand
   {
      private ModuleConfiguration _lastModuleConfiguration;

      protected override void Context()
      {
         base.Context();
         _lastModuleConfiguration = new ModuleConfiguration(new Module().WithId("lastModuleId"));
         _simulation.Configuration.AddModuleConfiguration(_lastModuleConfiguration);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_building_block_should_be_removed_from_the_selected_module()
      {
         _module.BuildingBlocks.ShouldNotContain(_buildingBlock);
      }

      [Observation]
      public void the_selected_building_block_should_be_cleared_in_the_selected_module_configuration()
      {
         _moduleConfiguration.SelectedInitialConditions.ShouldBeNull();
      }
   }
}
