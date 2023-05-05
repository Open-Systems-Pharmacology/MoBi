using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;

namespace MoBi.Core.Commands
{
   public class concern_for_RemoveModuleCommand : ContextSpecification<RemoveModuleCommand>
   {
      protected Module _module;
      protected IMoBiContext _context;
      protected MoBiProject _project;

      protected override void Context()
      {
         _project = DomainHelperForSpecs.NewProject();
         _context = A.Fake<IMoBiContext>();
         _module = new Module().WithId("moduleId");
         sut = new RemoveModuleCommand(_module);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }
   }

   public class When_reversing_the_remove_module_command : concern_for_RemoveModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Deserialize<Module>(A<byte[]>._)).Returns(new Module().WithId(_module.Id));
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_module_should_be_added_to_the_project()
      {
         _project.Modules.Contains(_module).ShouldBeTrue();
         ReferenceEquals(_project.Modules.First(), _module).ShouldBeFalse();
      }
   }

   public class When_executing_the_remove_module_command_and_the_module_has_building_blocks : concern_for_RemoveModuleCommand
   {
      private RegisterTask _registrationTask;
      protected WithIdRepository _withIdRepository;
      
      protected override void Context()
      {
         base.Context();
         _withIdRepository = new WithIdRepository();
         _registrationTask = new RegisterTask(_withIdRepository);

         _module.Add(new SpatialStructure().WithId("SpatialStructure"));
         _module.Add(new MoleculeBuildingBlock().WithId("Molecule"));
         _module.Add(new ReactionBuildingBlock().WithId("Reaction"));
         _module.Add(new PassiveTransportBuildingBlock().WithId("PassiveTransport"));
         _module.Add(new ObserverBuildingBlock().WithId("Observer"));
         _module.Add(new EventGroupBuildingBlock().WithId("EventGroup"));
         _module.Add(new InitialConditionsBuildingBlock().WithId("MoleculeStartValues"));
         _module.Add(new ParameterStartValuesBuildingBlock().WithId("ParameterStartValues"));

         A.CallTo(() => _context.Register(_module)).Invokes(() => _registrationTask.RegisterAllIn(_module));
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         
         _project.AddModule(_module);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_and_building_blocks_must_be_registered_in_the_context()
      {
         var withIds = _withIdRepository.All().ToList();
         withIds.ShouldNotContain(_module.SpatialStructure);
         withIds.ShouldNotContain(_module.Molecules);
         withIds.ShouldNotContain(_module.Reactions);
         withIds.ShouldNotContain(_module.PassiveTransports);
         withIds.ShouldNotContain(_module.Observers);
         withIds.ShouldNotContain(_module.EventGroups);
         withIds.ShouldNotContain(_module.InitialConditionsCollection.First());
         withIds.ShouldNotContain(_module.ParameterStartValuesCollection.First());
         withIds.ShouldNotContain(_module);
      }

      [Observation]
      public void the_module_is_removed_from_the_project()
      {
         _project.Modules.ShouldNotContain(_module);
      }
   }
}
