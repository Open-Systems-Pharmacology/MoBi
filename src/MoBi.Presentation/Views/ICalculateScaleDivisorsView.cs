using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICalculateScaleDivisorsView : IModalView<ICalculateScaleDivisorsPresenter>
   {
      void BindTo(IEnumerable<ScaleDivisorDTO> scaleDivisors);
      bool Calculating { get; set; }
      void RefreshData();
   }
}