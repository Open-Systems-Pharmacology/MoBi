using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public class SelectSingleObjectBasePresenter<T> : SelectSinglePresenter<T> where T : IObjectBase
   {
      public SelectSingleObjectBasePresenter(ISelectSingleView<T> view, IItemToListItemMapper<T> mapper) : base(view, mapper)
      {
      }

      public override string GetName(T item)
      {
         return item.Name;
      }
   }
}