using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditContainerPresenterSpecs : ContextSpecification<IEditContainerPresenter>
   {
      protected IEditContainerView _view;
      private IContainerToContainerDTOMapper _containerMapper;
      private IEditTaskForContainer _editTasks;
      protected IEditParametersInContainerPresenter _parametersInContainerPresenter;
      private IMoBiContext _context;
      private ITagsPresenter _tagsPresenter;
      private IApplicationController _applicationController;

      protected override void Context()
      {
         _view = A.Fake<IEditContainerView>();
         _containerMapper = A.Fake<IContainerToContainerDTOMapper>();
         _editTasks = A.Fake<IEditTaskForContainer>();
         _parametersInContainerPresenter = A.Fake<IEditParametersInContainerPresenter>();
         _context = A.Fake<IMoBiContext>();
         _tagsPresenter = A.Fake<ITagsPresenter>();
         _applicationController = A.Fake<IApplicationController>();
         sut = new EditContainerPresenter(_view, _containerMapper, _editTasks, _parametersInContainerPresenter, _context, _tagsPresenter,_applicationController);
      }
   }

   internal class When_told_a_container_presenter_to_select_parameters : concern_for_EditContainerPresenterSpecs
   {
      private IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = A.Fake<IParameter>();
      }

      protected override void Because()
      {
         sut.SelectParameter(_parameter);
      }

      [Observation]
      public void should_tell_view_to_show_parameters()
      {
         A.CallTo(() => _view.ShowParameters());
      }

      [Observation]
      public void should_tell_parameter_presenter_to_select_parameter()
      {
         A.CallTo(() => _parametersInContainerPresenter.Select(_parameter));
      }
   }
}