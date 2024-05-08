using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public class SelectManyObjectBasePresenter<T> : SelectManyPresenter<T> where T : IObjectBase
   {
      public SelectManyObjectBasePresenter(ISelectManyView<T> view, IItemToListItemMapper<T> mapper) : base(view, mapper)
      {
      }

      public override string GetName(T item) => item.Name;
   }
}