using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditContainerInSimulationPresenter : ContextSpecification<IEditContainerInSimulationPresenter>
   {
      protected IEditContainerPresenter _editContainerPresenter;
      private IEditContainerInSimulationView _view;

      protected override void Context()
      {
         _editContainerPresenter = A.Fake<IEditContainerPresenter>();
         _view = A.Fake<IEditContainerInSimulationView>();
         sut = new EditContainerInSimulationPresenter(_view, _editContainerPresenter);
      }
   }

   public class When_editing_a_container_in_a_simulation : concern_for_EditContainerInSimulationPresenter
   {
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = A.Fake<IContainer>();
      }

      protected override void Because()
      {
         sut.Edit(_container);
      }

      [Observation]
      public void should_set_the_edit_container_presenter_as_read_only()
      {
         A.CallTo(_editContainerPresenter).Where(x => x.Method.Name.Equals("set_ReadOnly"))
            .WhenArgumentsMatch(x => x.Get<bool>(0).Equals(true)).MustHaveHappened();
      }

      [Observation]
      public void should_only_enable_parameter_value_change()
      {
         A.CallTo(_editContainerPresenter).Where(x => x.Method.Name.Equals("set_EditMode"))
            .WhenArgumentsMatch(x => x.Get<EditParameterMode>(0).Equals(EditParameterMode.ValuesOnly)).MustHaveHappened();
      }

      [Observation]
      public void should_leverage_the_edit_container_presenter_to_edit_the_given_container()
      {
         A.CallTo(() => _editContainerPresenter.Edit(_container)).MustHaveHappened();
      }
   }
}