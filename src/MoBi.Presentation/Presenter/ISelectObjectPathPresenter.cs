using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectObjectPathPresenter: IDisposablePresenter
   {
      IEnumerable<ObjectBaseDTO> GetChildren(ObjectBaseDTO id);
      bool IsValidSelection(ObjectBaseDTO selectedDTO);
   }
}