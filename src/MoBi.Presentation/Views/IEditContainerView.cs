using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditContainerView : IView<IEditContainerPresenter>, IActivatableView, IEditViewWithParameters
   {
      void BindTo(ContainerDTO dto);
      void AddTagsView(IView view);
      bool ReadOnly { get; set; }
      bool ContainerPropertiesEditable { get; set; }
      bool IsNewEntity { get; set; }
   }
}