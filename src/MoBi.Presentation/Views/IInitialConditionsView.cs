using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IInitialConditionsView : IView<IBuildingBlockWithInitialConditionsPresenter>, IPathAndValueEntitiesView<InitialConditionDTO>
   {
   }
}