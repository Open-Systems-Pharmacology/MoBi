using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IMoleculeDependentBuilderView : IView<IMoleculeDependentBuilderPresenter>
   {
      void BindTo(MoleculeList moleculeList);
      string BuilderType { get; set; }
   }
}