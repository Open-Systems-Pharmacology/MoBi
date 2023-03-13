using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Helpers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_RenameContainerCommand : ContextSpecification<RenameContainerCommand>
   {
      protected ISpatialStructure _spatialStructure;
      protected IContainer _container;
      protected NeighborhoodBuilder _neighborhood1;
      protected NeighborhoodBuilder _neighborhood2;
      private Container _parent;
      private Container _child1;
      private Container _child2;
      private ObjectPathFactoryForSpecs _objectPathFactory;
      private Container _otherContainer;
      private Container _otherChild1;
      private Container _otherChild2;
      protected Container _child3;
      protected IMoBiContext _context;
      protected Container _similarContainer;

      protected override void Context()
      {
         //Making all of them having the same name to check that we are renaming the right part of the path
         _parent = new Container().WithName("A");
         _container = new Container().WithName("AAA").WithParentContainer(_parent);
         _similarContainer = new Container().WithName("A").WithParentContainer(_parent);
         _child1 = new Container().WithName("A").WithParentContainer(_container);
         _child2 = new Container().WithName("B").WithParentContainer(_container);
         _child3 = new Container().WithName("C").WithParentContainer(_container);
         _otherContainer = new Container().WithName("B").WithParentContainer(_parent);
         _otherChild1 = new Container().WithName("A").WithParentContainer(_otherContainer);
         _otherChild2 = new Container().WithName("B").WithParentContainer(_otherContainer);

         _objectPathFactory = new ObjectPathFactoryForSpecs();
         _spatialStructure = new SpatialStructure
         {
            NeighborhoodsContainer = new Container().WithName(Constants.NEIGHBORHOODS)
         };
         _neighborhood1 = new NeighborhoodBuilder
         {
            FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_child1),
            SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_child2),
            Name = "N1"
         };

         _neighborhood2 = new NeighborhoodBuilder
         {
            FirstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_otherChild1),
            SecondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(_otherChild2),
            Name = "N2"
         };

         _spatialStructure.AddNeighborhood(_neighborhood1);
         _spatialStructure.AddNeighborhood(_neighborhood2);
         _spatialStructure.AddTopContainer(_parent);

         _context = A.Fake<IMoBiContext>();
         A.CallTo(() => _context.ObjectPathFactory).Returns(new ObjectPathFactoryForSpecs());
      }
   }

   public class When_renaming_a_container_that_is_not_used_in_any_neighborhood : concern_for_RenameContainerCommand
   {
      protected override void Because()
      {
         sut = new RenameContainerCommand(_child3, "NEW_NAME", _spatialStructure).Run(_context);
      }

      [Observation]
      public void should_not_rename_the_path_in_the_neighborhoods()
      {
         _neighborhood1.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|AAA|A");
         _neighborhood1.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|AAA|B");

         _neighborhood2.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|B|A");
         _neighborhood2.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|B|B");
      }
   }

   public class When_renaming_a_container_that_is_used_in_multiple_neighborhood : concern_for_RenameContainerCommand
   {
      protected override void Because()
      {
         sut = new RenameContainerCommand(_container, "NEW_NAME", _spatialStructure).Run(_context);
      }

      [Observation]
      public void should_rename_the_path_in_the_neighborhoods_not_referencing_this_container()
      {
         _neighborhood1.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|NEW_NAME|A");
         _neighborhood1.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|NEW_NAME|B");
      }

      [Observation]
      public void should_not_rename_the_path_in_the_neighborhoods_not_referencing_this_container()
      {
         _neighborhood2.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|B|A");
         _neighborhood2.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|B|B");
      }
   }

   public class When_renaming_a_container_that_has_a_similar_path_as_another_container_but_is_not_used_neighborhood : concern_for_RenameContainerCommand
   {
      protected override void Because()
      {
         sut = new RenameContainerCommand(_similarContainer, "NEW_NAME", _spatialStructure).Run(_context);
      }
      
      [Observation]
      public void should_not_rename_the_path_in_the_neighborhoods_not_referencing_this_container()
      {
         _neighborhood1.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|AAA|A");
         _neighborhood1.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|AAA|B");
         _neighborhood2.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|B|A");
         _neighborhood2.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|B|B");
      }
   }


   public class When_renaming_a_container_and_neighborhoods_and_executing_the_inverse_command : concern_for_RenameContainerCommand
   {
      protected override void Context()
      {
         base.Context();
         sut = new RenameContainerCommand(_container, "NEW_NAME", _spatialStructure);
         A.CallTo(() => _context.Get<IContainer>(_container.Id)).Returns(_container);
         A.CallTo(() => _context.Get<ISpatialStructure>(_spatialStructure.Id)).Returns(_spatialStructure);
      }

      protected override void Because()
      {
         sut.ExecuteAndInvokeInverse(_context);
      }

      [Observation]
      public void should_rename_the_path_in_the_neighborhoods_that_were_changed_during_the_first_execution()
      {
         _neighborhood1.FirstNeighborPath.PathAsString.ShouldBeEqualTo("A|AAA|A");
         _neighborhood1.SecondNeighborPath.PathAsString.ShouldBeEqualTo("A|AAA|B");
      }
   }
}