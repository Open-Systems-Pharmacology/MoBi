using System.Collections.Generic;
using FakeItEasy;
using FakeItEasy.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;

namespace MoBi.Presentation
{
   public abstract class concern_for_SelectReferenceAtParameterValuePresenter : ContextSpecification<ISelectReferenceAtParameterValuePresenter>
   {
      protected ISelectReferenceView _view;
      protected IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      protected IObjectBaseToDummyMoleculeDTOMapper _moleculeMapper;
      protected IParameterToDummyParameterDTOMapper _parameterMapper;
      protected IUserSettings _userSettings;
      protected IObjectPathCreatorAtParameter _objectPathCreator;
      protected IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      protected IBuildingBlockRepository _buildingBlockRepository;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _moleculeMapper = A.Fake<IObjectBaseToDummyMoleculeDTOMapper>();
         _parameterMapper = A.Fake<IParameterToDummyParameterDTOMapper>();
         _userSettings = A.Fake<IUserSettings>();
         _objectPathCreator = A.Fake<IObjectPathCreatorAtParameter>();
         _referenceMapper = A.Fake<IObjectBaseDTOToReferenceNodeMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         A.CallTo(() => _view.Shows(A<IEntity>.Ignored)).Returns(true);
         sut = new SelectReferenceAtParameterValuePresenter(_view, _objectBaseDTOMapper, _context, _userSettings,
            _moleculeMapper, _parameterMapper, _referenceMapper, _objectPathCreator, _buildingBlockRepository);
      }
   }

   internal class When_selecting_multiple_references : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _parameterDTO2;
      private ObjectBaseDTO _parameterDTO1;

      protected override void Context()
      {
         base.Context();
         _parameterDTO1 = new ObjectBaseDTO (new Parameter().WithId("1"));
         _parameterDTO2 = new ObjectBaseDTO (new Parameter().WithId("2"));
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).ReturnsLazily(objectBaseForId);
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { _parameterDTO1, _parameterDTO2 });
      }

      private IObjectBase objectBaseForId(IFakeObjectCall x)
      {
         var id = x.Arguments.Get<string>(0);
         if (id == _parameterDTO1.Id)
            return _parameterDTO1.ObjectBase;

         if (id == _parameterDTO2.Id)
            return _parameterDTO2.ObjectBase;

         return null;
      }

      [Observation]
      public void the_presenter_can_close()
      {
         sut.CanClose.ShouldBeTrue();
      }
   }

   internal class When_selecting_multiple_references_and_not_all_are_parameters : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private ObjectBaseDTO _parameterDTO2;
      private ObjectBaseDTO _reactionDTO1;

      protected override void Context()
      {
         base.Context();
         _reactionDTO1 = new ObjectBaseDTO(new Reaction().WithId("1"));
         _parameterDTO2 = new ObjectBaseDTO(new Parameter().WithId("2"));
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).ReturnsLazily(objectBaseForId);
         A.CallTo(() => _view.AllSelectedDTOs).Returns(new[] { _reactionDTO1, _parameterDTO2 });
      }

      private IObjectBase objectBaseForId(IFakeObjectCall x)
      {
         var id = x.Arguments.Get<string>(0);
         if (id == _reactionDTO1.Id)
            return _reactionDTO1.ObjectBase;

         if (id == _parameterDTO2.Id)
            return _parameterDTO2.ObjectBase;

         return null;
      }

      [Observation]
      public void the_presenter_can_close()
      {
         sut.CanClose.ShouldBeFalse();
      }
   }

   internal class When_initializing_parameterValuePresenter : concern_for_SelectReferenceAtParameterValuePresenter
   {
      private IEntity _localReferencePoint;
      private IEnumerable<IObjectBase> _contextSpecificEntities;
      private IUsingFormula _editedObject;

      protected override void Context()
      {
         base.Context();
         _localReferencePoint = A.Fake<IEntity>();
         _contextSpecificEntities = A.CollectionOfFake<IObjectBase>(5);
         _editedObject = A.Fake<IUsingFormula>();
      }

      protected override void Because()
      {
         sut.Init(_localReferencePoint, _contextSpecificEntities, _editedObject);
      }

      [Observation]
      public void should_add_spatial_structures()
      {
         // Verify that AddSpatialStructures method is called indirectly via Init
         A.CallTo(() => _view.AddNodes(A<IEnumerable<ITreeNode>>._)).MustHaveHappened();
      }

      [Observation]
      public void should_not_allow_localisation_change()
      {
         _view.ChangeLocalisationAllowed.ShouldBeFalse();
      }
   }
}