using System.Linq;
using DevExpress.DataProcessing.InMemoryDataProcessor;
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
      protected IModuleRegisterVisitor _moduleRegisterVisitor;

      protected override void Context()
      {
         _module = new Module().WithId("moduleId");
         sut = new AddModuleCommand(_module);
         _project = new MoBiProject();
         _context = A.Fake<IMoBiContext>();
         _moduleRegisterVisitor = new ModuleRegisterVisitor(_context);
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         A.CallTo(() => _context.Resolve<IModuleRegisterVisitor>()).Returns(_moduleRegisterVisitor);
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

         _module.SpatialStructure = new SpatialStructure();
         _module.Molecule = new MoleculeBuildingBlock();
         _module.Reaction= new ReactionBuildingBlock();
         _module.PassiveTransport = new PassiveTransportBuildingBlock();
         _module.Observer = new ObserverBuildingBlock();
         _module.EventGroup = new EventGroupBuildingBlock();
         _module.AddMoleculeStartValueBlock(new MoleculeStartValuesBuildingBlock());
         _module.AddParameterStartValueBlock(new ParameterStartValuesBuildingBlock());
         
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
