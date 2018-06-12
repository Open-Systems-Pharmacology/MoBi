using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class UserDefinedParametersView : BaseUserControl, IUserDefinedParametersView
   {
      public UserDefinedParametersView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IUserDefinedParametersPresenter presenter)
      {
         //nothing to do here
      }

      public void AddParametersView(IView view)
      {
         this.FillWith(view);
      }
   }
}