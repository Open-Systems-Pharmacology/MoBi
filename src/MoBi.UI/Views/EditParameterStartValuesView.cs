using MoBi.Assets;
using OSPSuite.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditParameterStartValuesView : EditBuildingBlockBaseView, IEditParameterStartValuesView
   {
      public EditParameterStartValuesView(IMoBiMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditParameterStartValuesPresenter presenter)
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
         EditCaption = AppConstants.Captions.ParameterStartValues;
         ApplicationIcon = ApplicationIcons.ParameterStartValues;
      }
   }
}