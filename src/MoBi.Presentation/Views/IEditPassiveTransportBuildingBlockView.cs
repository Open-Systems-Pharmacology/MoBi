using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditPassiveTransportBuildingBlockView : IView<IEditPassiveTransportBuildingBlockPresenter>, IEditBuildingBlockBaseView
   {
      void Show(IEnumerable<TransportBuilderDTO> dtoPassiveTransportBuilders);
      void SetEditView(IView view);
      void ClearEditView();
   }
}