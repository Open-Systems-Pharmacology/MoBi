using MoBi.Core.Domain.Model.Diagram;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Presenter.ReactionDiagram;
using MoBi.Presentation.Presenter.SpaceDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views.BaseDiagram
{
   public interface IReactionDiagramView : IView<IReactionDiagramPresenter>, IMoBiBaseDiagramView
   {
   }

   public interface ISpatialStructureDiagramView : IView<ISpatialStructureDiagramPresenter>, IMoBiBaseDiagramView
   {
   }

   public interface ISimulationDiagramView : IView<ISimulationDiagramPresenter>, IMoBiBaseDiagramView
   {
      void DisplayEductsRight(IDiagramModel reactionBlockDiagramModel);
      void ObserverLinksVisible(IDiagramModel diagramModel, bool visible);
   }

   public interface IDiagramOptionsView : ISimpleEditView<IDiagramOptions>
   {
   }

   public interface IForceLayoutConfigurationView : ISimpleEditView<IForceLayoutConfiguration>
   {
   }

   public interface IChartOptionsView : ISimpleEditView<ChartOptions>
   {
   }
}