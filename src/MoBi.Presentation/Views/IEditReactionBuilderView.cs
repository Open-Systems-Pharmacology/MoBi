using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditReactionBuilderView :  IView<IEditReactionBuilderPresenter>, IViewWithFormula, IActivatableView
   {
      void BindTo(ReactionBuilderDTO reactionBuilderDTO);
      void SetParameterView(IView view);
      void SetContainerCriteriaView(IView view);
      bool Visible { get; set; }
      bool ShowStoichiometry { set; }
      void ShowParameters();
      bool PlotProcessRateParameterEnabled { set; }
      void SetEductView(IView view);
      void SetProductView(IView view);
      void SetModifierView(IView view);
   }
}