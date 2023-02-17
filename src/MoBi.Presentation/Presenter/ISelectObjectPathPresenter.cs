using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectObjectPathPresenter: IDisposablePresenter
   {
      IEnumerable<IObjectBaseDTO> GetChildren(IObjectBaseDTO id);
      bool IsValidSelection(IObjectBaseDTO selectedDTO);
   }
}