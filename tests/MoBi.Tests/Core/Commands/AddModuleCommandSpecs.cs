using System.Linq;
using DevExpress.Utils.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
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
      protected IMoBiProject _project;
      private RegisterTask _registrationTask;
      protected WithIdRepository _withIdRepository;

      protected override void Context()
      {
         _module = new Module().WithId("moduleId");
         _withIdRepository = new WithIdRepository();
         _registrationTask = new RegisterTask(_withIdRepository);
         sut = new AddModuleCommand(_module);
         _project = new MoBiProject();
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

   public class When_executing_the_add_module_command_and_the_module_has_building_blocks : concern_for_AddModuleCommand
   {
      protected override void Context()
      {
         base.Context();

         _module.SpatialStructure = new SpatialStructure().WithId("SpatialStructure");
         _module.Molecule = new MoleculeBuildingBlock().WithId("Molecule");
         _module.Reaction = new ReactionBuildingBlock().WithId("Reaction");
         _module.PassiveTransport = new PassiveTransportBuildingBlock().WithId("PassiveTransport");
         _module.Observer = new ObserverBuildingBlock().WithId("Observer");
         _module.EventGroup = new EventGroupBuildingBlock().WithId("EventGroup");
         _module.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock().WithId("MoleculeStartValues"));
         _module.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock().WithId("ParameterStartValues"));
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_and_building_blocks_must_be_registered_in_the_context()
      {
         var withIds = _withIdRepository.All();
         withIds.ShouldContain(_module.SpatialStructure);
         withIds.ShouldContain(_module.Molecule);
         withIds.ShouldContain(_module.Reaction);
         withIds.ShouldContain(_module.PassiveTransport);
         withIds.ShouldContain(_module.Observer);
         withIds.ShouldContain(_module.EventGroup);
         withIds.ShouldContain(_module.MoleculeStartValuesCollection.First());
         withIds.ShouldContain(_module.ParameterStartValuesCollection.First());
         withIds.ShouldContain(_module);
      }

      [Observation]
      public void the_module_is_added_to_the_project()
      {
         _project.Modules.ShouldContain(_module);
      }
   }
}
