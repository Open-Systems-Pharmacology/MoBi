using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Presenter.SpaceDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views.BaseDiagram
{
   public interface IReactionDiagramView : IView<IReactionDiagramPresenter>, IMoBiBaseDiagramView
   {
   }

   public interface ISpatialStructureDiagramView : IView<ISpatialStructureDiagramPresenter>, IMoBiBaseDiagramView
   {
   }

   public interface IDiagramOptionsView : ISimpleEditView<IDiagramOptions>
   {
   }

   public interface IChartOptionsView : ISimpleEditView<ChartOptions>
   {
   }
}