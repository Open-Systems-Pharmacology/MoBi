using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using MoBi.Core.Domain.Services;

namespace MoBi.Core.Commands
{
   public class concern_for_RemoveModuleCommand : ContextSpecification<RemoveModuleCommand>
   {
      protected Module _module;
      protected IMoBiContext _context;
      protected IMoBiProject _project;
      private ModuleUnregisterVisitor _moduleUnregisterVisitor;

      protected override void Context()
      {
         _project = new MoBiProject();
         _context = A.Fake<IMoBiContext>();
         _module = new Module().WithId("moduleId");
         sut = new RemoveModuleCommand(_module);
         _moduleUnregisterVisitor = new ModuleUnregisterVisitor(_context);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Resolve<IModuleUnregisterVisitor>()).Returns(_moduleUnregisterVisitor);
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
      protected override void Context()
      {
         base.Context();

         _module.SpatialStructure = new SpatialStructure();
         _module.Molecule = new MoleculeBuildingBlock();
         _module.Reaction = new ReactionBuildingBlock();
         _module.PassiveTransport = new PassiveTransportBuildingBlock();
         _module.Observer = new ObserverBuildingBlock();
         _module.EventGroup = new EventGroupBuildingBlock();
         _module.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         _module.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
         _project.AddModule(_module);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_and_building_blocks_must_be_unregistered_in_the_context()
      {
         A.CallTo(() => _context.Unregister(_module.SpatialStructure)).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.Molecule)).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.Reaction)).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.PassiveTransport)).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.Observer)).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.EventGroup)).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.MoleculeStartValuesCollection.First())).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module.ParameterStartValuesCollection.First())).MustHaveHappened();
         A.CallTo(() => _context.Unregister(_module)).MustHaveHappened();
      }

      [Observation]
      public void the_module_is_removed_from_the_project()
      {
         _project.Modules.ShouldNotContain(_module);
      }
   }
}
