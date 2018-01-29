using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IEditParameterListPresenter : IParameterPresenter, IPresenterWithContextMenu<IViewItem>
   {
      void GoTo(ParameterDTO parameterDTO);
   }
}