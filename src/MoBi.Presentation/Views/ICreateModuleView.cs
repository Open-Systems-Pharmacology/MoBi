using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateModuleView : IModalView<ICreateModulePresenter>   
   {
      void BindTo(CreateModuleDTO createModuleDTO);
   }
}
