using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditContainerPresenterSpecs : ContextSpecification<IEditContainerPresenter>
   {
      private IEntityTask _enityTask;
      protected IEditContainerView _view;
      private IContainerToContainerDTOMapper _containerMapper;
      private IEditTaskForContainer _editTasks;
      protected IEditParameterListPresenter _parameterListPresenter;
      private IMoBiContext _context;

      protected override void Context()
      {
         _view = A.Fake<IEditContainerView>();
         _containerMapper = A.Fake<IContainerToContainerDTOMapper>();
         _editTasks = A.Fake<IEditTaskForContainer>();
         _parameterListPresenter = A.Fake<IEditParameterListPresenter>();
         _context = A.Fake<IMoBiContext>();
         _enityTask = A.Fake<IEntityTask>();
         sut = new EditContainerPresenter(_view, _containerMapper, _editTasks, _parameterListPresenter, _context, _enityTask);
      }
   }

   internal class When_told_a_containerpresenter_to_select_parameters : concern_for_EditContainerPresenterSpecs
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
      public void should_tell_view_to_show_parmaeters()
      {
         A.CallTo(() => _view.ShowParameters());
      }

      [Observation]
      public void should_tell_parameter_presenter_to_select_parameter()
      {
         A.CallTo(() => _parameterListPresenter.Select(_parameter));
      }
   }
}