using System.Collections.Generic;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditOutputSchemaView : IView<IEditOutputSchemaPresenter>
   {
      void Show(IEnumerable<OutputIntervalDTO> outputIntervalInfos);
      bool ShowGroupCaption { set; }
   }
}