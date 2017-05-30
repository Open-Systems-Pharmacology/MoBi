using System.Collections.Generic;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditFavoritesView : IView<IEditFavoritesPresenter>
   {
      void Show(IEnumerable<FavoriteParameterDTO> favorites);
      void Select(FavoriteParameterDTO parameterDTO);
      void SetCaptions(IDictionary<PathElement, string> captions);
      void SetVisibility(PathElement pathElement, bool isVisible);
      void Rebind();
   }
}