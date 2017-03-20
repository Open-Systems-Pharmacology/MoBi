using FakeItEasy;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation
{
   public static class SubPresenterHelper
   {
      public static T CreateFake<T, U>(this ISubPresenterItemManager<U> subPresenterManager, SubPresenterItem<T> subPresenterItem)
         where U : ISubPresenter
         where T : U
      {
         var subPresenter = A.Fake<T>();
         A.CallTo(() => subPresenterManager.PresenterAt(subPresenterItem)).Returns(subPresenter);
         return subPresenter;
      }
   }
}