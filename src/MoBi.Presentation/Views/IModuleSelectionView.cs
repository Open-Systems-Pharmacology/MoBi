using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IModuleSelectionView : IView<IModuleSelectionPresenter>, IResizableView
   {
      void BindTo(ModuleSelectionDTO moduleSelectionDTO);
      bool NewVisible { set; }
   }
}