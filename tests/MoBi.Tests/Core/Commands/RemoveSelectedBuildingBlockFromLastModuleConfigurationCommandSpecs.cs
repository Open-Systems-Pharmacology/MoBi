using System;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand : ContextSpecification<RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand<InitialConditionsBuildingBlock>>
   {
      protected IMoBiContext _context;
      protected MoBiSimulation _simulation;
      protected Module _module;
      protected InitialConditionsBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();

         _simulation = new MoBiSimulation().WithId("simulationId");
         _simulation.Configuration = new SimulationConfiguration();
         _module = new Module().WithId("moduleId");
         _buildingBlock = new InitialConditionsBuildingBlock().WithId("parameterValuesBuildingBlockId");
         _module.Add(_buildingBlock);
         var moduleConfiguration = new ModuleConfiguration(_module)
         {
            SelectedInitialConditions = _buildingBlock
         };
         _simulation.Configuration.AddModuleConfiguration(moduleConfiguration);
         
         A.CallTo(() => _context.Get<Module>(_module.Id)).Returns(_module);
         A.CallTo(() => _context.Get<InitialConditionsBuildingBlock>(_buildingBlock.Id)).Returns(_buildingBlock);
         A.CallTo(() => _context.Get<IMoBiSimulation>(_simulation.Id)).Returns(_simulation);
         sut = new RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand<InitialConditionsBuildingBlock>(_buildingBlock, _simulation);
      }
   }

   public class When_reverting_the_remove_selected_building_block_command : concern_for_RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand
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
         _simulation.Configuration.ModuleConfigurations.Last().SelectedInitialConditions.ShouldBeEqualTo(_deserializedBuildingBlock);
      }

      [Observation]
      public void the_building_block_should_be_added()
      {
         _module.ShouldContain(_deserializedBuildingBlock);
      }
   }

   public class When_removing_the_selected_building_block_from_the_simulation : concern_for_RemoveSelectedBuildingBlockFromLastModuleConfigurationCommand
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
         _simulation.Configuration.ModuleConfigurations.Last().SelectedInitialConditions.ShouldBeNull();
      }
   }
}
