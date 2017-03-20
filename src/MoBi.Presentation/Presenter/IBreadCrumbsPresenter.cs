namespace MoBi.Presentation.Presenter
{
   public interface IBreadCrumbsPresenter
   {
      bool HasAtLeastTwoDistinctValues(int pathElementIndex);
   }
}