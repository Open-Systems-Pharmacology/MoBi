using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Presenter
{
   public static class EditParameterListPresenterExtensions
   {
      public static void ConfigureForReaction(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         var captions = new Dictionary<PathElement, string> {{PathElement.TopContainer, ObjectTypes.Reaction}};
         view.SetCaptions(captions);

         view.SetVisibility(PathElement.BottomCompartment, isVisible: false);
         view.SetVisibility(PathElement.Container, isVisible: false);
         view.SetVisibility(PathElement.Molecule, isVisible: false);
      }

      public static void ConfigureForEvent(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         view.SetVisibility(PathElement.Molecule, isVisible: false);
      }

      public static void ConfigureForMolecule(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         var captions = new Dictionary<PathElement, string>
         {
            {PathElement.TopContainer, AppConstants.Captions.Molecule},
            {PathElement.Container, $"{ObjectTypes.TransporterMoleculeContainer}/{ObjectTypes.InteractionContainer}"},
            {PathElement.BottomCompartment, ObjectTypes.ActiveTransport}
         };
         view.SetCaptions(captions);
         view.SetVisibility(PathElement.Molecule, isVisible: false);
      }

      public static void ShowNameColumn(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         view.SetVisibility(PathElement.Name, isVisible: true);
      }
   }
}