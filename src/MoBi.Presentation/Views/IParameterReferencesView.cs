using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IParameterReferencesView : IModalView<IParameterReferencesPresenter>
   {
      void BindTo(IReadOnlyList<ParameterReferenceDTO> references);

      /// <summary>
      ///    Updates the breadcrumb showing the trail of drilled-in targets and the enabled state of the back button
      /// </summary>
      void UpdateNavigation(string breadcrumb, bool canGoBack);
   }
}
