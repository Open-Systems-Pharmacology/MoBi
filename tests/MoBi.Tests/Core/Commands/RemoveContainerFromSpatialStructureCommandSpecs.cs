using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Helpers;
using NPOI.SS.Formula.Functions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RemoveContainerFromSpatialStructureCommand : ContextSpecification<RemoveContainerFromSpatialStructureCommand>
   {
      protected IContainer _parent;
      protected IContainer _containerToRemove;
      protected MoBiSpatialStructure _spatialStructure;
      protected IContainer _otherContainer;
      protected NeighborhoodBuilder _neighborhood;
      protected string _parentId = "parent";
      private ISpatialStructureDiagramManager _spatialStructureDiagramManager;
      protected IMoBiContext _context;
      private ObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _spatialStructureDiagramManager = A.Fake<ISpatialStructureDiagramManager>();
         _spatialStructure = new MoBiSpatialStructure {DiagramManager = _spatialStructureDiagramManager}.WithName("SpSt");
         _spatialStructure.NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS);
         _parent = new Container().WithName("Top").WithMode(ContainerMode.Logical).WithId(_parentId);
         _containerToRemove = new Container().WithName("A").WithMode(ContainerMode.Physical).WithParentContainer(_parent);
         _otherContainer = new Container().WithName("B").WithMode(ContainerMode.Physical).WithParentContainer(_parent);
         _spatialStructure.AddTopContainer(_parent);
         _neighborhood = new NeighborhoodBuilder
         {
            FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_containerToRemove),
            SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_otherContainer),
            Name = "A2B"
         };
         _spatialStructure.AddNeighborhood(_neighborhood);
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.ObjectPathFactory).Returns(_objectPathFactory);
         sut = new RemoveContainerFromSpatialStructureCommand(_parent, _containerToRemove, _spatialStructure);
      }
   }

   internal class When_executing_a_remove_container_from_spatial_structure_command : concern_for_RemoveContainerFromSpatialStructureCommand
   {
      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_container()
      {
         _parent.ShouldOnlyContain((IEntity) _otherContainer);
      }

      [Observation]
      public void should_also_remove_the_neighborhood_connected_to_the_container()
      {
         _spatialStructure.Neighborhoods.ShouldBeEmpty();
      }
   }

   internal class When_revert_a_remove_container_from_spatial_structure_command : concern_for_RemoveContainerFromSpatialStructureCommand
   {
      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.Get<MoBiSpatialStructure>(A<string>._)).Returns(_spatialStructure);
         A.CallTo(() => _context.Get<IContainer>(_parentId)).Returns(_parent);
         A.CallTo(() => _context.Deserialize<IContainer>(A<byte[]>._)).Returns(_containerToRemove);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_restore_the_container()
      {
         _parent.Children.ShouldOnlyContain(_otherContainer, _containerToRemove);
      }

      [Observation]
      public void should_also_Restore_the_neighborhood_connected_to_the_container()
      {
         _spatialStructure.Neighborhoods.ShouldContain(_neighborhood);
      }
   }
}