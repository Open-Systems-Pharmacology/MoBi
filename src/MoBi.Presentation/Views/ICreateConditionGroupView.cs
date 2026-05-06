using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ICreateConditionGroupView : IModalView<ICreateConditionGroupPresenter>
   {
      void BindTo(EditConditionGroupDTO dto);
      void InitializeTagTypes();
   }
}
