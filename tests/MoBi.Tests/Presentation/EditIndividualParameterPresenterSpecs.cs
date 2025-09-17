using FakeItEasy;
using MoBi.Core.Events;
using MoBi.Core.Mappers;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation
{
   internal class concern_for_EditIndividualParameterPresenter : ContextSpecification<EditIndividualParameterPresenter>
   {
      private IEditIndividualParameterView _view;
      private IIndividualParameterToIndividualParameterDTOMapper _individualParameterToIndividualParameterDTOMapper;
      protected IndividualParameter _individualParameter;
      protected IndividualBuildingBlock _buildingBlock;
      private IPathAndValueEntityToDistributedParameterMapper _pathAndValueEntityToDistributedParameterMapper;
      protected IEditTaskFor<IndividualBuildingBlock> _editTask;
      protected IEventPublisher _eventPublisher;

      protected override void Context()
      {
         _view = A.Fake<IEditIndividualParameterView>();
         _individualParameterToIndividualParameterDTOMapper = A.Fake<IIndividualParameterToIndividualParameterDTOMapper>();
         _pathAndValueEntityToDistributedParameterMapper = A.Fake<IPathAndValueEntityToDistributedParameterMapper>();
         _editTask = A.Fake<IEditTaskFor<IndividualBuildingBlock>>();
         _eventPublisher = A.Fake<IEventPublisher>();

         _individualParameter = new IndividualParameter();
         _buildingBlock = new IndividualBuildingBlock { _individualParameter };

         A.CallTo(() => _individualParameterToIndividualParameterDTOMapper.MapFrom(A<IndividualParameter>._)).ReturnsLazily(x => new IndividualParameterDTO(x.Arguments.Get<IndividualParameter>(0)));
         sut = new EditIndividualParameterPresenter(_view, _individualParameterToIndividualParameterDTOMapper, _pathAndValueEntityToDistributedParameterMapper, _editTask, _eventPublisher);
         sut.InitializeWith(A.Fake<ICommandCollector>());
      }
   }

   internal class When_navigating_to_the_individual_parameter : concern_for_EditIndividualParameterPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_individualParameter, _buildingBlock);
      }

      protected override void Because()
      {
         sut.NavigateToParameter();
      }

      [Observation]
      public void the_edit_task_opens_the_editor()
      {
         A.CallTo(() => _editTask.Edit(_buildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void the_event_publisher_publishes_selection_event()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<EntitySelectedEvent>.That.Matches(x => x.ObjectBase.Equals(_individualParameter)))).MustHaveHappened();
      }
   }
}