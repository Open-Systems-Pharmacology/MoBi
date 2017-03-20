using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditEventGroupView : IView<IEditEventGroupPresenter>, IActivatableView
   {
      void AddParametersView(IView subView);
      void BindTo(EventGroupBuilderDTO eventGroupBuilderDTO);
      bool EnableDescriptors { get; set; }
      void ShowParameters();
      void AddDescriptorConditionListView(IView view);
   }
}