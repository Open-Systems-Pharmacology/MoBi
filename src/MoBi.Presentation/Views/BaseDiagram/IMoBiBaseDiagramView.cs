using System.Windows.Forms;
using MoBi.Presentation.Presenter.BaseDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Diagram;

namespace MoBi.Presentation.Views.BaseDiagram
{
   public interface IMoBiBaseDiagramView : IView<IMoBiBaseDiagramPresenter>, IBaseDiagramView
   {
      void ExpandParents(IBaseNode baseNode);
      bool IsMoleculeNode(IBaseNode baseNode);
      Control Overview { set; }
   }
}