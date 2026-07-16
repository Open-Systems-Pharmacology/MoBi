using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Views
{
   public interface ICreateModuleView : IAddContentToModuleView<ICreateModulePresenter>   
   {
      void BindTo(ModuleContentDTO createModuleDTO);
   }
}
