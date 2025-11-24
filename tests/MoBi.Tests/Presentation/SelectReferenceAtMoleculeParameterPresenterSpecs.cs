using FakeItEasy;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Settings;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;

namespace MoBi.Presentation
{
   internal class concern_for_SelectReferenceAtMoleculeParameterPresenter : ContextSpecification<SelectReferenceAtMoleculeParameterPresenter>
   {
      protected ISelectReferenceView _view;
      protected IObjectBaseDTOToReferenceNodeMapper _referenceNodeMapper;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _referenceNodeMapper = A.Fake<IObjectBaseDTOToReferenceNodeMapper>();
         sut = new SelectReferenceAtMoleculeParameterPresenter(_view,
            A.Fake<IObjectBaseToObjectBaseDTOMapper>(),
            A.Fake<IMoBiContext>(),
            A.Fake<IUserSettings>(),
            A.Fake<IObjectBaseToDummyMoleculeDTOMapper>(),
            A.Fake<IParameterToDummyParameterDTOMapper>(),
            _referenceNodeMapper,
            A.Fake<IObjectPathCreatorAtMoleculeParameter>(),
            A.Fake<IBuildingBlockRepository>());
      }
   }

   internal class When_initializing_the_SelectReferenceAtMoleculeParameterPresenter : concern_for_SelectReferenceAtMoleculeParameterPresenter
   {
      private IEntity _entity;
      private ITreeNode _timeNode;

      protected override void Context()
      {
         base.Context();
         _timeNode = A.Fake<ITreeNode>();
         _entity = A.Fake<IEntity>();
         _entity.ParentContainer = null;
         A.CallTo(() => _referenceNodeMapper.MapFrom(A<ObjectBaseDTO>.That.Matches(x => x.Id == AppConstants.Time))).Returns(_timeNode);
      }

      protected override void Because()
      {
         sut.Init(_entity, new[] { _entity }, null);
      }

      [Observation]
      public void should_add_time_reference_to_view()
      {
         A.CallTo(() => _view.AddNode(A<ITreeNode>.That.Matches(x => Equals(x, _timeNode)))).MustHaveHappened();
      }
   }
}
