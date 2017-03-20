using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_AddReactionPartnerToEductCollection : ContextSpecification<AddReactionPartnerToEductCollection>
   {
      protected IReactionBuilder _reaction;
      private IMoBiReactionBuildingBlock _reactionBuildingBlock;
      private IReactionPartnerBuilder _reactionPartner;
      private IMoBiContext _context;
      protected IReactionDimensionRetriever _dimensionRetriever;
      protected ExplicitFormula _kinetic;
      protected IDimension _moleculeDimension;

      protected override void Context()
      {
         _moleculeDimension= A.Fake<IDimension>();
         _kinetic = new ExplicitFormula();
         _reaction = new ReactionBuilder {Formula = _kinetic};
         _reactionBuildingBlock = A.Fake<IMoBiReactionBuildingBlock>();
         _reactionBuildingBlock.DiagramManager= A.Fake<IMoBiReactionDiagramManager>();
         _reactionPartner = new ReactionPartnerBuilder("A", 1);
         sut = new AddReactionPartnerToEductCollection(_reactionBuildingBlock, _reactionPartner, _reaction);

         _context= A.Fake<IMoBiContext>();
         _dimensionRetriever= A.Fake<IReactionDimensionRetriever>();
         A.CallTo(() => _context.Resolve<IAliasCreator>()).Returns(new AliasCreator());
         A.CallTo(() => _context.Resolve<IReactionDimensionRetriever>()).Returns(_dimensionRetriever);
         A.CallTo(() => _context.Resolve<IObjectPathFactory>()).Returns(new ObjectPathFactory(new AliasCreator()));
         A.CallTo(() => _dimensionRetriever.MoleculeDimension).Returns(_moleculeDimension);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }
   }

   public class When_adding_a_molecule_as_educt_to_a_reaction_using_amount_based_rate : concern_for_AddReactionPartnerToEductCollection
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.AmountBased);
      }

      [Observation]
      public void should_add_the_molecule_in_the_list_of_educts()
      {
         _reaction.Educts.Select(x=>x.MoleculeName).ShouldOnlyContain("A");
      }

      [Observation]
      public void should_add_a_reference_to_the_molecule_amount()
      {
         var objectPath = new ObjectPath(ObjectPath.PARENT_CONTAINER, "A");
         var usedPath= _kinetic.ObjectPaths.First(x => x.PathAsString == objectPath.PathAsString);
         usedPath.Dimension.ShouldBeEqualTo(_moleculeDimension);

      }
   }


   public class When_adding_a_molecule_as_educt_to_a_reaction_using_concentration_based_rate : concern_for_AddReactionPartnerToEductCollection
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dimensionRetriever.SelectedDimensionMode).Returns(ReactionDimensionMode.ConcentrationBased);
      }

      [Observation]
      public void should_add_the_molecule_in_the_list_of_educts()
      {
         _reaction.Educts.Select(x => x.MoleculeName).ShouldOnlyContain("A");
      }

      [Observation]
      public void should_add_a_reference_to_the_molecule_concentration_parameter()
      {
         var objectPath = new ObjectPath(ObjectPath.PARENT_CONTAINER, "A",AppConstants.Parameters.CONCENTRATION);
         var usedPath = _kinetic.ObjectPaths.First(x => x.PathAsString == objectPath.PathAsString);
         usedPath.Dimension.ShouldBeEqualTo(_moleculeDimension);
      }
   }
}