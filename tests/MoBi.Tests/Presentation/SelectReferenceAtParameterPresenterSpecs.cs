using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Nodes;

namespace MoBi.Presentation
{
   internal class concern_for_SelectReferenceAtParameterPresenter : ContextSpecification<SelectReferenceAtParameterPresenter>
   {
      protected ISelectReferenceView _view;

      protected override void Context()
      {
         _view = A.Fake<ISelectReferenceView>();
         sut = new SelectReferenceAtParameterPresenter(_view,
            A.Fake<IObjectBaseToObjectBaseDTOMapper>(),
            A.Fake<IMoBiContext>(),
            A.Fake<IUserSettings>(),
            A.Fake<IObjectBaseToDummyMoleculeDTOMapper>(),
            A.Fake<IParameterToDummyParameterDTOMapper>(),
            A.Fake<IObjectBaseDTOToReferenceNodeMapper>(),
            A.Fake<IObjectPathCreatorAtParameter>(),
            A.Fake<IBuildingBlockRepository>());
      }
   }


   internal class When_time_selection_is_disabled : concern_for_SelectReferenceAtParameterPresenter
   {
      private IEntity _localReferencePoint;
      private IEnumerable<IObjectBase> _contextSpecificEntitiesToAddToReferenceTree;
      private IUsingFormula _editedObject;

      protected override void Context()
      {
         base.Context();
         sut.DisableTimeSelection();
         _localReferencePoint = new Container();
         _contextSpecificEntitiesToAddToReferenceTree = A.CollectionOfFake<IObjectBase>(3);
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
}
