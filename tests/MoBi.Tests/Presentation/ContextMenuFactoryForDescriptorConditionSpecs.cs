using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;

namespace MoBi.Presentation
{
   public abstract class concern_for_ContextMenuFactoryForDescriptorCondition : ContextSpecificationWithLocalContainer<IContextMenuFactoryForDescriptorCondition>
   {
      protected override void Context()
      {
         sut = new ContextMenuFactoryForDescriptorCondition();
      }
   }

   public class When_creating_a_context_menu_for_a_descriptor_condition : concern_for_ContextMenuFactoryForDescriptorCondition
   {
      private IDescriptorConditionListPresenter _presenter;
      private IViewItem _viewItem;
      private ContextMenuBase _contextMenu;

      protected override void Context()
      {
         base.Context();
         _presenter = A.Fake<IDescriptorConditionListPresenter>();
         _viewItem = A.Fake<IViewItem>();
         A.CallTo(() => _presenter.Subject).Returns(new ContainerObserverBuilder());
      }

      protected override void Because()
      {
         _contextMenu = sut.CreateFor(_viewItem, _presenter).DowncastTo<ContextMenuBase>();
      }

      [Observation]
      public void should_not_allow_the_match_all_condition_for_a_container_observer()
      {
         _contextMenu.AllMenuItems().Any(x => x.Caption == AppConstants.Captions.AddMatchAllCondition).ShouldBeFalse();
      }
   }
}