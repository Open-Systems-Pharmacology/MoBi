using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using IoC = OSPSuite.Utility.Container.IContainer;
using FakeItEasy;
using MoBi.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_ShowPresenterFactory : ContextSpecification<IEditInSimulationPresenterFactory>
   {
      private IoC _ioc;
      protected IEditContainerInSimulationPresenter _showContainerPresenter;
      protected IEditQuantityInSimulationPresenter _editQuantityPresenter;
      protected IEditReactionInSimulationPresenter _showReactionPresenter;

      protected override void Context()
      {
         _ioc = A.Fake<IoC>();
         _showContainerPresenter = A.Fake<IEditContainerInSimulationPresenter>();
         _editQuantityPresenter = A.Fake<IEditQuantityInSimulationPresenter>();
         _showReactionPresenter = A.Fake<IEditReactionInSimulationPresenter>();
         A.CallTo(() => _ioc.Resolve<IEditQuantityInSimulationPresenter>()).Returns(_editQuantityPresenter);
         A.CallTo(() => _ioc.Resolve<IEditReactionInSimulationPresenter>()).Returns(_showReactionPresenter);
         A.CallTo(() => _ioc.Resolve<IEditContainerInSimulationPresenter>()).Returns(_showContainerPresenter);
         sut = new EditInSimulationPresenterFactory(_ioc);
      }
   }

   public class When_creating_a_show_presenter : concern_for_ShowPresenterFactory
   {
      [Observation]
      public void should_return_the_registered_presenter_for_a_quantity()
      {
         sut.PresenterFor(new Parameter()).ShouldBeEqualTo(_editQuantityPresenter);
      }

      [Observation]
      public void should_return_the_registered_presenter_for_a_neighbhorhood()
      {
         sut.PresenterFor(new Reaction()).ShouldBeEqualTo(_showReactionPresenter);
      }

      [Observation]
      public void should_return_the_registered_presenter_for_a_container()
      {
         sut.PresenterFor(new Container()).ShouldBeEqualTo(_showContainerPresenter);
      }

      [Observation]
      public void should_return_null_for_an_entity_that_is_not_a_container()
      {
         sut.PresenterFor(new PureEntity()).ShouldBeNull();
      }
   }
}