using OSPSuite.Assets;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditBuildingBlockBaseView : IMdiChildView
   {
      void SetFormulaCacheView(IView view);
      void ShowFormulas();
      void ShowDefault();
      string EditCaption { set; }
      ApplicationIcon EditIcon{ set; }
   }
}