using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectCommitTargetView : IModalView<ISelectCommitTargetPresenter>
   {
      void BindTo(CommitTargetDTO commitTargetDTO);
      void SetDescription(string description);
   }
}
