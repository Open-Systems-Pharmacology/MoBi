using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditApplicationBuilderView : IView<IEditApplicationBuilderPresenter>, IActivatableView
   {
      void BindTo(ApplicationBuilderDTO eventGroupBuilderDTO);
      void SetParametersView(IView subView);
      bool EnableDescriptors { get; set; }
      void ShowParameters();
      void AddDescriptorConditionListView(IDescriptorConditionListView view);
   }
}