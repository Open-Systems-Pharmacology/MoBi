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
         var captions = new Dictionary<PathElementId, string> {{ PathElementId.TopContainer, ObjectTypes.Reaction}};
         view.SetCaptions(captions);

         view.SetVisibility(PathElementId.BottomCompartment, isVisible: false);
         view.SetVisibility(PathElementId.Container, isVisible: false);
         view.SetVisibility(PathElementId.Molecule, isVisible: false);
      }

      public static void ConfigureForEvent(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         view.SetVisibility(PathElementId.Molecule, isVisible: false);
      }

      public static void ConfigureForMolecule(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         var captions = new Dictionary<PathElementId, string>
         {
            {PathElementId.TopContainer, AppConstants.Captions.Molecule},
            {PathElementId.Container, $"{ObjectTypes.TransporterMoleculeContainer}/{ObjectTypes.InteractionContainer}"},
            {PathElementId.BottomCompartment, ObjectTypes.ActiveTransport}
         };
         view.SetCaptions(captions);
         view.SetVisibility(PathElementId.Molecule, isVisible: false);
      }

      public static void ShowNameColumn(this IEditParameterListPresenter presenter)
      {
         var view = presenter.View;
         view.SetVisibility(PathElementId.Name, isVisible: true);
      }
   }
}