using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using System.Collections.Generic;
using System.Linq;


namespace MoBi.Presentation
{
   internal class concern_for_SelectReferenceAtParameterPresenter : ContextSpecification<SelectReferenceAtParameterPresenter>
   {
      protected ISelectReferenceView _view;
      protected IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _referenceMapper = A.Fake<IObjectBaseDTOToReferenceNodeMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         sut = new SelectReferenceAtParameterPresenter(_view,
            A.Fake<IObjectBaseToObjectBaseDTOMapper>(),
            A.Fake<IMoBiContext>(),
            A.Fake<IUserSettings>(),
            A.Fake<IObjectBaseToDummyMoleculeDTOMapper>(),
            A.Fake<IParameterToDummyParameterDTOMapper>(),
            _referenceMapper,
            A.Fake<IObjectPathCreatorAtParameter>(),
            _buildingBlockRepository);
      }
   }


   internal class When_time_selection_is_disabled : concern_for_SelectReferenceAtParameterPresenter
   {
      private IEntity _localReferencePoint;
      private IReadOnlyList<IObjectBase> _contextSpecificEntitiesToAddToReferenceTree;
      private IUsingFormula _editedObject;

      protected override void Context()
      {
         base.Context();
         sut.DisableTimeSelection();
         _localReferencePoint = new Container();
         _contextSpecificEntitiesToAddToReferenceTree = A.CollectionOfFake<IObjectBase>(3).ToList();
         _editedObject = new Parameter();
      }

      protected override void Because()
      {
         sut.Init(_localReferencePoint, _contextSpecificEntitiesToAddToReferenceTree, _editedObject);
      }

      [Observation]
      public void time_reference_should_not_be_added_to_the_view()
      {
         A.CallTo(() => _view.AddNode(A<ITreeNode>.That.Matches(x => isTime(x)))).MustNotHaveHappened();
      }

      private bool isTime(ITreeNode treeNode)
      {
         return (treeNode.TagAsObject as ObjectBaseDTO).Id == AppConstants.Time;
      }
   }

   internal class When_adding_reactions_grouped_by_module : concern_for_SelectReferenceAtParameterPresenter
   {
      private MoBiReactionBuildingBlock _reactionBlock1;
      private MoBiReactionBuildingBlock _reactionBlock2;
      private IEntity _reference;
      private Parameter _parameter;
      private ITreeNode _reactionBlockNode1;
      private ITreeNode _reactionBlockNode2;

      protected override void Context()
      {
         base.Context();
         _reference = new Parameter();
         _parameter = new Parameter();

         _reactionBlockNode1 = A.Fake<ITreeNode>();
         _reactionBlockNode2 = A.Fake<ITreeNode>();


         _reactionBlock1 = new MoBiReactionBuildingBlock().WithName("RB1");
         _reactionBlock2 = new MoBiReactionBuildingBlock().WithName("RB2");

         A.CallTo(() => _buildingBlockRepository.ReactionBlockCollection)
            .Returns(new[] { _reactionBlock1, _reactionBlock2 });

         A.CallTo(() => _referenceMapper.MapFrom(_reactionBlock1))
            .Returns(_reactionBlockNode2);

         A.CallTo(() => _referenceMapper.MapFrom(_reactionBlock2))
            .Returns(_reactionBlockNode1);
      }

      protected override void Because()
      {
         sut.Init(_reference, new List<IObjectBase>(), _parameter);
      }

      [Observation]
      public void should_add_nodes_to_the_view()
      {
         A.CallTo(() => _view.AddNodes(A<IEnumerable<ITreeNode>>.That.Matches(x => x.Contains(_reactionBlockNode1) && x.Contains(_reactionBlockNode2)))).MustHaveHappened();
      }
   }
}
