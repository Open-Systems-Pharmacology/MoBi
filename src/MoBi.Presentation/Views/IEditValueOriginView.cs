using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   //TODO MOVE TO CORE
   public interface IEditValueOriginView : IView<IEditValueOriginPresenter>, IResizableView
   {
      void BindTo(ValueOriginDTO valueOriginDTO);
      bool ShowCaption { get; set; }
   }
}