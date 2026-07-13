using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Nodes;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectReferenceAtReactionPresenter : ContextSpecification<ISelectReferenceAtReactionPresenter>
   {
      protected ISelectReferenceView _view;
      protected IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      protected IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected ReactionBuilder _reaction;
      protected ITreeNode _reactionNode;
      protected List<ITreeNode> _renderedNodes;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseDTOMapper = new ObjectBaseToObjectBaseDTOMapper();
         _referenceMapper = new ObjectBaseDTOToReferenceNodeMapper(_objectBaseDTOMapper);
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();

         _reaction = new ReactionBuilder().WithName("newReaction");
         _reaction.Id = "reactionId";

         var repo = A.Fake<IWithIdRepository>();
         A.CallTo(() => _context.ObjectRepository).Returns(repo);
         A.CallTo(() => repo.ContainsObjectWithId(_reaction.Id)).Returns(true);
         A.CallTo(() => _context.Get<IObjectBase>(_reaction.Id)).Returns(_reaction);

         sut = new SelectReferenceAtReactionPresenter(_view, _objectBaseDTOMapper, _context, A.Fake<IUserSettings>(),
            A.Fake<IObjectBaseToDummyMoleculeDTOMapper>(), A.Fake<IParameterToDummyParameterDTOMapper>(),
            _referenceMapper, A.Fake<IObjectPathCreatorAtReaction>(), _buildingBlockRepository);

         _reactionNode = _referenceMapper.MapFrom(_reaction);
         A.CallTo(() => _view.Shows(_reaction)).Returns(true);
         A.CallTo(() => _view.GetNodes(_reaction)).Returns(new List<ITreeNode> { _reactionNode });

         _renderedNodes = new List<ITreeNode>();
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).Invokes(x => _renderedNodes.Add(x.GetArgument<ITreeNode>(0)));
      }

      protected void AddParameterAndRaiseEvent(string name = "lastParam", string id = "pNew")
      {
         var newParameter = new Parameter().WithName(name).WithId(id);
         _reaction.Add(newParameter);
         sut.Handle(new AddedEvent<IParameter>(newParameter, _reaction));
      }

      protected void EnumerateReactionNodeChildren() => _reactionNode.Children.ToList();
   }

   public class When_a_parameter_is_added_to_a_reaction_whose_children_are_not_yet_enumerated : concern_for_SelectReferenceAtReactionPresenter
   {
      protected override void Context()
      {
         base.Context();
         _reaction.Add(new Parameter().WithName("k_on").WithId("p1"));
         _reaction.Add(new Parameter().WithName("k_off").WithId("p2"));
      }

      protected override void Because()
      {
         AddParameterAndRaiseEvent();
      }

      [Observation]
      public void should_not_add_the_node_so_the_tree_still_enumerates_all_parameters_when_the_reaction_is_expanded()
      {
         _renderedNodes.ShouldBeEmpty();
      }
   }

   public class When_a_parameter_is_added_to_a_reaction_whose_children_are_already_enumerated : concern_for_SelectReferenceAtReactionPresenter
   {
      protected override void Context()
      {
         base.Context();
         _reaction.Add(new Parameter().WithName("k_on").WithId("p1"));
         _reaction.Add(new Parameter().WithName("k_off").WithId("p2"));
         EnumerateReactionNodeChildren();
      }

      protected override void Because()
      {
         AddParameterAndRaiseEvent();
      }

      [Observation]
      public void should_add_the_newly_created_parameter_node()
      {
         _renderedNodes.Select(x => x.Text).ShouldContain("lastParam");
      }
   }

   public class When_the_first_parameter_is_added_to_an_already_enumerated_empty_reaction : concern_for_SelectReferenceAtReactionPresenter
   {
      protected override void Context()
      {
         base.Context();
         EnumerateReactionNodeChildren();
      }

      protected override void Because()
      {
         AddParameterAndRaiseEvent("firstParam", "p1");
      }

      [Observation]
      public void should_add_the_first_parameter_node()
      {
         _renderedNodes.Select(x => x.Text).ShouldContain("firstParam");
      }
   }
}
