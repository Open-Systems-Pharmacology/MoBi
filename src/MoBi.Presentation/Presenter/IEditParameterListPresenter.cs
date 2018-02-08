using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterListPresenter : IParameterPresenter, IPresenterWithContextMenu<IViewItem>, IPresenter<IEditParameterListView>
   {
      void GoTo(ParameterDTO parameterDTO);
   }
}