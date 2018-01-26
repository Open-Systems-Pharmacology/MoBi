using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditParameterView : IView<IEditParameterPresenter>, IViewWithFormula, IActivatableView
   {
      void Show(ParameterDTO parameterDTO);
      void AddRHSView(IView rhsView);
      bool ShowBuildMode { set; }
      void AddValueOriginView(IView view);
   }
}