using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Services;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation
{
   public abstract class concern_for_TreeNodeFactory : ContextSpecification<ITreeNodeFactory>
   {
      private IToolTipPartCreator _toolTipPartCreator;
      private IObservedDataRepository _observedDataRepository;

      protected override void Context()
      {
         _toolTipPartCreator= A.Fake<IToolTipPartCreator>();
         _observedDataRepository= A.Fake<IObservedDataRepository>();
         sut = new Nodes.TreeNodeFactory(_observedDataRepository,_toolTipPartCreator);
      }
   }

   public class When_creating_the_node_for_a_molecule_building_block : concern_for_TreeNodeFactory
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private ITreeNode _node;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock {Id = "1"};
         _moleculeBuildingBlock.Add(A.Fake<MoleculeBuilder>().WithId("2"));
         _moleculeBuildingBlock.Add(A.Fake<MoleculeBuilder>().WithId("3"));
      }

      protected override void Because()
      {
         _node = sut.CreateFor(_moleculeBuildingBlock);
      }

      [Observation]
      public void should_add_a_sub_node_for_each_molecules_defined_in_the_building_block()
      {
         _node.Children.Count().ShouldBeEqualTo(2);
         _node.Children.First(x => x.Id == "2").ShouldNotBeNull();
         _node.Children.First(x => x.Id == "3").ShouldNotBeNull();
      }
   }
}