using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
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
   internal class concern_for_SelectReferenceAtMoleculeParameterPresenter : ContextSpecification<SelectReferenceAtMoleculeParameterPresenter>
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
         sut = new SelectReferenceAtMoleculeParameterPresenter(_view,
            _objectBaseDTOMapper,
            A.Fake<IMoBiContext>(),
            A.Fake<IUserSettings>(),
            A.Fake<IObjectBaseToDummyMoleculeDTOMapper>(),
            A.Fake<IParameterToDummyParameterDTOMapper>(),
            _referenceMapper,
            A.Fake<IObjectPathCreatorAtMoleculeParameter>(),
            _buildingBlockRepository);
      }
   }

   internal class When_grouping_reaction_builders_by_module_in_molecule_parameter_reference_tree : concern_for_SelectReferenceAtMoleculeParameterPresenter
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
