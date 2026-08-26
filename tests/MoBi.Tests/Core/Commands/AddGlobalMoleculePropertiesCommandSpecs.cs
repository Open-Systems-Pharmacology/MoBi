using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Domain.Services;
using MoBi.Core.Extensions;
using MoBi.HelpersForTests;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Commands
{
   internal class concern_for_AddGlobalMoleculePropertiesCommand : ContextSpecification<AddGlobalMoleculePropertiesCommand>
   {
      protected MoBiSpatialStructure _spatialStructure;
      protected IContainer _moleculeProperties;
      protected IMoBiContext _context;
      protected IRegisterTask _registerTask;

      protected override void Context()
      {
         _spatialStructure = new MoBiSpatialStructure { DiagramManager = A.Fake<ISpatialStructureDiagramManager>() }.WithName("SpSt");
         _moleculeProperties = new Container().WithName(Constants.MOLECULE_PROPERTIES).WithMode(ContainerMode.Logical);

         _context = A.Fake<IMoBiContext>();
         _registerTask = A.Fake<IRegisterTask>();

         A.CallTo(() => _context.ObjectPathFactory).Returns(new ObjectPathFactoryForSpecs());
         A.CallTo(() => _context.Resolve<IRegisterTask>()).Returns(_registerTask);

         sut = new AddGlobalMoleculePropertiesCommand(_spatialStructure, _moleculeProperties);
      }
   }

   internal class When_adding_the_global_molecule_properties_to_the_spatial_structure : concern_for_AddGlobalMoleculePropertiesCommand
   {
      protected override void Because()
      {
         sut.RunCommand(_context);
      }

      [Observation]
      public void the_container_should_be_registered_using_the_task()
      {
         A.CallTo(() => _registerTask.RegisterAllIn(_moleculeProperties)).MustHaveHappened();
      }

      [Observation]
      public void the_container_should_be_set_as_the_global_molecule_dependent_properties()
      {
         _spatialStructure.GlobalMoleculeDependentProperties.ShouldBeEqualTo(_moleculeProperties);
      }
   }

   internal class When_reverting_the_add_global_molecule_properties_command : concern_for_AddGlobalMoleculePropertiesCommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _context.Get<MoBiSpatialStructure>(A<string>._)).Returns(_spatialStructure);
         A.CallTo(() => _context.Get<IContainer>(_moleculeProperties.Id)).Returns(_moleculeProperties);
         A.CallTo(() => _context.Resolve<IUnregisterTask>()).Returns(A.Fake<IUnregisterTask>());
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_remove_the_global_molecule_properties_from_the_spatial_structure()
      {
         _spatialStructure.GlobalMoleculeDependentProperties.ShouldBeNull();
      }
   }
}