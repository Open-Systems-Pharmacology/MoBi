using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using NHibernate.Util;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Serializer.Attributes;

namespace MoBi.Presentation
{
   public class concern_for_CreateObjectPathsFromReferencesPresenter : ContextSpecification<CreateObjectPathsFromReferencesPresenter>
   {
      protected ICreateObjectPathsFromReferencesView _view;
      protected ISelectReferenceAtParameterValuePresenter _selectReferencePresenter;
      protected IMoBiContext _context;
      private IObjectBaseToObjectBaseDTOMapper _objectBaseDTOMapper;
      private IObjectBaseToDummyMoleculeDTOMapper _moleculeMapper;
      private IParameterToDummyParameterDTOMapper _parameterMapper;
      private IUserSettings _userSettings;
      private IObjectPathCreatorAtParameter _objectPathCreator;
      private IObjectBaseDTOToReferenceNodeMapper _referenceMapper;
      private IBuildingBlockRepository _buildingBlockRepository;
      protected ISelectReferenceView _referenceView;

      protected override void Context()
      {
         _view = A.Fake<ICreateObjectPathsFromReferencesView>();
         _referenceView = A.Fake<ISelectReferenceView>();
         _context = A.Fake<IMoBiContext>();
         _objectBaseDTOMapper = A.Fake<IObjectBaseToObjectBaseDTOMapper>();
         _moleculeMapper = A.Fake<IObjectBaseToDummyMoleculeDTOMapper>();
         _parameterMapper = A.Fake<IParameterToDummyParameterDTOMapper>();
         _userSettings = A.Fake<IUserSettings>();
         _objectPathCreator = A.Fake<IObjectPathCreatorAtParameter>();
         _referenceMapper = A.Fake<IObjectBaseDTOToReferenceNodeMapper>();
         _buildingBlockRepository = A.Fake<IBuildingBlockRepository>();
         A.CallTo(() => _referenceView.Shows(A<IEntity>.Ignored)).Returns(true);
         _selectReferencePresenter = new SelectReferenceAtParameterValuePresenter(_referenceView, _objectBaseDTOMapper, _context, _userSettings,
            _moleculeMapper, _parameterMapper, _referenceMapper, _objectPathCreator, _buildingBlockRepository);

         sut = new CreateObjectPathsFromReferencesPresenter(_view, _selectReferencePresenter);
      }
   }

   public class When_getting_the_paths_to_be_added : concern_for_CreateObjectPathsFromReferencesPresenter
   {
      private IReadOnlyList<ObjectPath> _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.AllPaths).Returns(new[] { "A|Path" });
      }

      protected override void Because()
      {
         _result = sut.GetAllSelections();
      }

      [Observation]
      public void the_paths_are_converted()
      {
         _result[0].ElementAt(0).ShouldBeEqualTo("A");
         _result[0].ElementAt(1).ShouldBeEqualTo("Path");
      }
   }

   public class When_adding_new_reference_selections : concern_for_CreateObjectPathsFromReferencesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _referenceView.AllSelectedDTOs).Returns(new List<ObjectBaseDTO> { new ObjectBaseDTO() });
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).Returns(new Parameter());
      }

      protected override void Because()
      {
         sut.AddSelection();
      }

      [Observation]
      public void the_add_button_is_enabled()
      {
         A.CallTo(() => _view.AddSelectedPaths(A<IReadOnlyList<string>>._)).MustHaveHappened();
      }
   }

   public class When_the_selected_reference_is_not_valid : concern_for_CreateObjectPathsFromReferencesPresenter
   {
      protected override void Because()
      {
          _selectReferencePresenter.SelectionChanged();
      }

      [Observation]
      public void the_add_button_is_disabled()
      {
         A.CallTo(() => _view.CanAdd(false)).MustHaveHappened();
      }
   }

   public class When_the_selected_reference_is_valid : concern_for_CreateObjectPathsFromReferencesPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _referenceView.AllSelectedDTOs).Returns(new List<ObjectBaseDTO> { new ObjectBaseDTO() });
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).Returns(new Parameter());
      }

      protected override void Because()
      {
         _selectReferencePresenter.SelectionChanged();
      }

      [Observation]
      public void the_add_button_is_enabled()
      {
         A.CallTo(() => _view.CanAdd(true)).MustHaveHappened();
      }
   }
}
