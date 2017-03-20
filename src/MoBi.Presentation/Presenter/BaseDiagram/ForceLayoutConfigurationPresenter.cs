using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Presentation.Diagram.Elements;

namespace MoBi.Presentation.Presenter.BaseDiagram
{
   public interface IForceLayoutConfigurationPresenter : ISimpleEditPresenter<IForceLayoutConfiguration>
   {
   }

   public class ForceLayoutConfigurationPresenter : SimpleEditPresenter<IForceLayoutConfiguration>, IForceLayoutConfigurationPresenter
   {
      public ForceLayoutConfigurationPresenter(IForceLayoutConfigurationView view) : base(view)
      {
      }
   }
}