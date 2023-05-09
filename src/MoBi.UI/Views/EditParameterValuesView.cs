using MoBi.Assets;
using OSPSuite.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditParameterValuesView : EditBuildingBlockBaseView, IEditParameterValuesView
   {
      public EditParameterValuesView(IMoBiMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditParameterValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddParameterView(IView view)
      {
         tabEditBuildingBlock.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         EditCaption = AppConstants.Captions.ParameterValues;
         ApplicationIcon = ApplicationIcons.ParameterValues;
      }
   }
}