using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using IBuildingBlockRepository = MoBi.Core.Domain.Repository.IBuildingBlockRepository;


namespace MoBi.Presentation
{
   internal class concern_for_SelectReferenceAtParameterPresenter : ContextSpecification<SelectReferenceAtParameterPresenter>
   {
      protected ISelectReferenceView _view;
      protected IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _referenceMapper = A.Fake<IObjectBaseDTOToReferenceNodeMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         _objectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         sut = new SelectReferenceAtParameterPresenter(_view,
            _objectBaseDTOMapper,
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

   internal class When_grouping_reaction_builders_by_module_in_parameter_reference_tree : concern_for_SelectReferenceAtParameterPresenter
   {
      private ITreeNode _moduleNode;

      protected override void Context()
      {
         base.Context();
         var reactionBlock = new MoBiReactionBuildingBlock().WithName("RB1");

         A.CallTo(() => _buildingBlockRepository.ReactionBlockCollection)
            .Returns(new[] { reactionBlock });

         _moduleNode = A.Fake<ITreeNode>();
         A.CallTo(() => _referenceMapper.MapFrom(reactionBlock)).Returns(_moduleNode);
      }

      protected override void Because()
      {
         sut.Init(new Parameter(), new List<IObjectBase>(), new Parameter());
      }

      [Observation]
      public void should_add_module_nodes_to_the_view()
      {
         A.CallTo(() => _view.AddNodes(A<IEnumerable<ITreeNode>>.That.Matches(x => x.Contains(_moduleNode)))).MustHaveHappened();
      }
   }
}
