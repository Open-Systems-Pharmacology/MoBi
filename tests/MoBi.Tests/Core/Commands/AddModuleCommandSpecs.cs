using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_AddModuleCommand : ContextSpecification<AddModuleCommand>
   {
      protected Module _module;
      protected IMoBiContext _context;
      protected MoBiProject _project;
      private RegisterTask _registrationTask;
      protected WithIdRepository _withIdRepository;

      protected override void Context()
      {
         _module = new Module().WithId("moduleId");
         _withIdRepository = new WithIdRepository();
         _registrationTask = new RegisterTask(_withIdRepository);
         sut = new AddModuleCommand(_module);
         _project = DomainHelperForSpecs.NewProject();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Register(_module)).Invokes(() => _registrationTask.RegisterAllIn(_module));
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }
   }

   public class When_reversing_the_add_module_command : concern_for_AddModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<Module>(_module.Id)).Returns(_module);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void the_module_is_added_to_the_project()
      {
         _project.Modules.Contains(_module).ShouldBeFalse();
      }
   }

   public class When_executing_the_add_module_command_and_the_module_is_from_pk_sim_and_has_not_been_added_to_any_projects_before : concern_for_AddModuleCommand
   {
      protected override void Context()
      {
         base.Context();
         _module.PKSimVersion= "1";
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_is_a_pk_sim_module()
      {
         _module.IsPKSimModule.ShouldBeTrue();
      }

      [Observation]
      public void the_module_has_its_import_version_added_to_extended_properties()
      {
         _module.ModuleImportVersion.ShouldNotBeNull();
      }
   }

   public class When_executing_the_add_module_command_and_the_module_has_building_blocks : concern_for_AddModuleCommand
   {
      protected override void Context()
      {
         base.Context();

         _module.Add(new SpatialStructure().WithId("SpatialStructure"));
         _module.Add(new MoleculeBuildingBlock().WithId("Molecule"));
         _module.Add(new ReactionBuildingBlock().WithId("Reaction"));
         _module.Add(new PassiveTransportBuildingBlock().WithId("PassiveTransport"));
         _module.Add(new ObserverBuildingBlock().WithId("Observer"));
         _module.Add(new EventGroupBuildingBlock().WithId("EventGroup"));
         _module.Add(new InitialConditionsBuildingBlock().WithId("InitialConditions"));
         _module.Add(new ParameterValuesBuildingBlock().WithId("ParameterValues"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_is_not_pk_sim_module()
      {
         _module.IsPKSimModule.ShouldBeFalse();
      }

      [Observation]
      public void the_module_and_building_blocks_must_be_registered_in_the_context()
      {
         var withIds = _withIdRepository.All().ToList();
         withIds.ShouldContain(_module.SpatialStructure);
         withIds.ShouldContain(_module.Molecules);
         withIds.ShouldContain(_module.Reactions);
         withIds.ShouldContain(_module.PassiveTransports);
         withIds.ShouldContain(_module.Observers);
         withIds.ShouldContain(_module.EventGroups);
         withIds.ShouldContain(_module.InitialConditionsCollection.First());
         withIds.ShouldContain(_module.ParameterValuesCollection.First());
         withIds.ShouldContain(_module);
      }

      [Observation]
      public void the_module_is_added_to_the_project()
      {
         _project.Modules.ShouldContain(_module);
      }
   }
}