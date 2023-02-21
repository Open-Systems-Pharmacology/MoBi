using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectLocalisationView : IView<ISelectLocalisationPresenter>
   {
      void Show(IEnumerable<SpatialStructureDTO> dtoSpatialStructure);
      ObjectBaseDTO Selected { get;  }
      void Show(List<ObjectBaseDTO> dtos);
   }
}