using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditExplicitFormulaView : IView<IEditExplicitFormulaPresenter>, IEditTypedFormulaView
   {
      void Show(ExplicitFormulaBuilderDTO dto);
      void SetValidationMessage(string text);
      bool Enabled { get; set; }
      void SetFormulaCaption(string caption);
      void HideFormulaCaption();
      void AddFormulaPathListView(IView view);
   }
}