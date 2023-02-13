using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class concern_for_AddModuleCommand : ContextSpecification<AddModuleCommand>
   {
      protected Module _module;

      protected override void Context()
      {
         _module = new Module();
         sut = new AddModuleCommand(_module);
      }
   }

   public class When_executing_the_add_module_command_and_the_module_has_building_blocks : concern_for_AddModuleCommand
   {
      IMoBiContext _context;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _project = new MoBiProject();
         _context = A.Fake<IMoBiContext>();
         _module.SpatialStructure = new SpatialStructure();
         _module.Molecule = new MoleculeBuildingBlock();
         _module.Reaction= new ReactionBuildingBlock();
         _module.PassiveTransport = new PassiveTransportBuildingBlock();
         _module.Observer = new ObserverBuildingBlock();
         _module.EventGroup = new EventGroupBuildingBlock();
         _module.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         _module.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
         A.CallTo(() => _context.CurrentProject).Returns(_project);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void the_module_and_building_blocks_must_be_registered_in_the_context()
      {
         A.CallTo(() => _context.Register(_module.SpatialStructure)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.Molecule)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.Reaction)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.PassiveTransport)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.Observer)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.EventGroup)).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.MoleculeStartValuesCollection.First())).MustHaveHappened();
         A.CallTo(() => _context.Register(_module.ParameterStartValuesCollection.First())).MustHaveHappened();
         A.CallTo(() => _context.Register(_module)).MustHaveHappened();
      }

      [Observation]
      public void the_module_is_added_to_the_project()
      {
         _project.Modules.ShouldContain(_module);
      }
   }
}
