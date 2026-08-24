using FakeItEasy;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.UICommand;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using IProjectTask = MoBi.Presentation.Tasks.IProjectTask;

namespace MoBi.Presentation
{
   public abstract class concern_for_MoBiMainViewPresenter : ContextSpecification<MoBiMainViewPresenter>
   {
      protected IUserSettings _userSettings;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _userSettings = A.Fake<IUserSettings>();
         _dialogCreator = A.Fake<IDialogCreator>();

         sut = new MoBiMainViewPresenter(
            A.Fake<IMoBiMainView>(),
            A.Fake<IRepository<IMainViewItemPresenter>>(),
            A.Fake<IProjectTask>(),
            A.Fake<ISkinManager>(),
            A.Fake<IExitCommand>(),
            A.Fake<IEventPublisher>(),
            _userSettings,
            A.Fake<ITabbedMdiChildViewContextMenuFactory>(),
            A.Fake<IMoBiConfiguration>(),
            A.Fake<IWatermarkStatusChecker>(),
            _dialogCreator);
      }
   }

   public class When_running_the_main_view_presenter_and_the_v13_conversion_notice_has_not_been_shown : concern_for_MoBiMainViewPresenter
   {
      protected override void Context()
      {
         base.Context();
         _userSettings.V13ConversionInfoShown = false;
      }

      protected override void Because()
      {
         sut.Run();
      }

      [Observation]
      public void should_show_the_v13_conversion_notice()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(AppConstants.Captions.V13ConversionNoticeDescription)).MustHaveHappened();
      }

      [Observation]
      public void should_remember_that_the_notice_was_shown()
      {
         _userSettings.V13ConversionInfoShown.ShouldBeTrue();
      }
   }

   public class When_running_the_main_view_presenter_and_the_v13_conversion_notice_was_already_shown : concern_for_MoBiMainViewPresenter
   {
      protected override void Context()
      {
         base.Context();
         _userSettings.V13ConversionInfoShown = true;
      }

      protected override void Because()
      {
         sut.Run();
      }

      [Observation]
      public void should_not_show_the_v13_conversion_notice_again()
      {
         A.CallTo(() => _dialogCreator.MessageBoxInfo(A<string>._)).MustNotHaveHappened();
      }
   }
}
