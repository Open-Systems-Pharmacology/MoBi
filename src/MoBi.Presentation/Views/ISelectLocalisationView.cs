using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectLocalisationView : IView<ISelectLocalisationPresenter>
   {
      void Show(IEnumerable<SpatialStructureDTO> dtoSpatialStructure);
      IObjectBaseDTO Selected { get;  }
      void Show(List<IObjectBaseDTO> dtos);
   }
}