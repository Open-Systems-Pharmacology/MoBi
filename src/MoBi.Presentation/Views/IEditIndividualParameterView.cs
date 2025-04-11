using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditIndividualParameterView : IView<IEditIndividualParameterPresenter>
   {
      void BindTo(IndividualParameterDTO individualParameterDTO);
      void ShowWarningFor(string buildingBlockName);
   }
}