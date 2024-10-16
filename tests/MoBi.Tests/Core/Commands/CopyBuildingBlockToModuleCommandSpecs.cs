using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_CopyBuildingBlockToModuleCommand : ContextSpecification<CopyBuildingBlockToModuleCommand<IBuildingBlock>>
   {
      protected MoleculeBuildingBlock _bb;
      protected Module _sourceModule;
      protected Module _targetModule;
      protected IMoBiContext _context;
      protected IMoBiSimulation _affectedSimulation;
      private MoBiProject _project;

      protected override void Context()
      {
         _affectedSimulation = new MoBiSimulation();
         _affectedSimulation.Configuration = new SimulationConfiguration();
         _project = new MoBiProject();
         _sourceModule = new Module().WithId("sourceModuleId").WithName("Source Module");
         _targetModule = new Module().WithId("targetModuleId").WithName("Target Module"); ;
         _affectedSimulation.Configuration.AddModuleConfiguration(new ModuleConfiguration(_sourceModule));
         _context = A.Fake<IMoBiContext>();
         _project.AddSimulation(_affectedSimulation);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         _bb = new MoleculeBuildingBlock().WithId("newMoleculeBuildingBlockId");
         _bb.Module = _sourceModule;
         _sourceModule.Add(_bb);
         sut = new CopyBuildingBlockToModuleCommand<IBuildingBlock>(_bb, _targetModule);
      }
   }

   internal class When_Copying_a_buildingBlock_group_to_module_command : concern_for_CopyBuildingBlockToModuleCommand
   {
      private AddedEvent _event;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _context.PublishEvent(A<AddedEvent<IBuildingBlock>>._))
            .Invokes(x => _event = x.GetArgument<AddedEvent>(0));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_add_building_block_to_project()
      {
         _targetModule.BuildingBlocks.ShouldContain(_bb);
         _sourceModule.BuildingBlocks.ShouldContain(_bb);
      }

      [Observation]
      public void should_publish_added_event()
      {
         _event.AddedObject.ShouldBeEqualTo(_bb);
         _event.Parent.ShouldBeEqualTo(_targetModule);
      }
   }
}