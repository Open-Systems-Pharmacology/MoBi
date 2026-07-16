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
   internal class concern_for_AddContainerToSpatialStructureCommand : ContextSpecification<AddContainerToSpatialStructureCommand>
   {
      protected IContainer _parent;
      protected IContainer _containerToAdd;
      protected MoBiSpatialStructure _spatialStructure;
      protected readonly string _parentId = "parent";
      private ISpatialStructureDiagramManager _spatialStructureDiagramManager;
      protected IMoBiContext _context;
      private ObjectPathFactory _objectPathFactory;
      protected IRegisterTask _registerTask;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _spatialStructureDiagramManager = A.Fake<ISpatialStructureDiagramManager>();
         _spatialStructure = new MoBiSpatialStructure { DiagramManager = _spatialStructureDiagramManager }.WithName("SpSt");
         _spatialStructure.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);
         _parent = new Container().WithName("Top").WithMode(ContainerMode.Logical).WithId(_parentId);
         _containerToAdd = new Container().WithName("A").WithMode(ContainerMode.Physical);
         _spatialStructure.AddTopContainer(_parent);

         _context = A.Fake<IMoBiContext>();
         _registerTask = A.Fake<IRegisterTask>();

         A.CallTo(() => _context.ObjectPathFactory).Returns(_objectPathFactory);
         A.CallTo(() => _context.Resolve<IRegisterTask>()).Returns(_registerTask);

         sut = new AddContainerToSpatialStructureCommand(_parent, _containerToAdd, _spatialStructure);
      }
   }

   internal class When_adding_the_container_to_the_spatial_structure : concern_for_AddContainerToSpatialStructureCommand
   {
      protected override void Because()
      {
         sut.RunCommand(_context);
      }

      [Observation]
      public void the_container_should_be_registered_using_the_task()
      {
         A.CallTo(() => _registerTask.RegisterAllIn(_containerToAdd)).MustHaveHappened();
      }

      [Observation]
      public void the_container_is_added_to_the_spatial_structure()
      {
         _parent.ShouldContain(_containerToAdd);
      }
   }

   internal class When_revert_an_add_container_to_spatial_structure_command : concern_for_AddContainerToSpatialStructureCommand
   {
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<MoBiSpatialStructure>(A<string>._)).Returns(_spatialStructure);
         A.CallTo(() => _context.Get<IContainer>(_parentId)).Returns(_parent);
         A.CallTo(() => _context.Get<IContainer>(_containerToAdd.Id)).Returns(_containerToAdd);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_remove_the_container()
      {
         _parent.Children.ShouldNotContain(_containerToAdd);
      }
   }
}