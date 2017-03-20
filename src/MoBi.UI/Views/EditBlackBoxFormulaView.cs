using MoBi.Assets;
using OSPSuite.UI.Extensions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditBlackBoxFormulaView : BaseUserControl, IEditBlackBoxFormulaView
   {
      private IEditBlackBoxFormulaPresenter _presenter;
      public bool ReadOnly { get; set; }

      public EditBlackBoxFormulaView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditBlackBoxFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         lblDescription.AsDescription();
         lblDescription.Text = AppConstants.Captions.BlackBoxFormula;
      }
   }
}