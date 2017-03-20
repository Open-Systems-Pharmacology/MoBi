using System.Drawing;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IMergeConflictResolverView : IModalView<IMergeConflictResolverPresenter>
   {
      void AttachSummaryViews(IView mergeView, IView targetView, int i);
      bool CloneButtonEnabled { get; set; }
      bool MergeOptionEnabled { get; set; }
      void SetFormLayout(Point location, Size size);
   }
}
