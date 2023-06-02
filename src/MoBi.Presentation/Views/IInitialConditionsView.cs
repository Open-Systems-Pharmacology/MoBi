using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IBuildingBlockWithInitialConditionsView<TPresenter> : IView<TPresenter>, IPathAndValueEntitiesView<InitialConditionDTO> where TPresenter : IPresenter
   {
      void AddIsPresentSelectionView(IView view);

      void AddNegativeValuesAllowedSelectionView(IView view);
   }

   public interface IInitialConditionsView : IBuildingBlockWithInitialConditionsView<IInitialConditionsPresenter>
   {
   }

   public interface IExpressionProfileInitialConditionsView : IBuildingBlockWithInitialConditionsView<IExpressionProfileInitialConditionsPresenter>
   {
   }
}