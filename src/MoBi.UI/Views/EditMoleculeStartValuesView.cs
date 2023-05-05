using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditMoleculeStartValuesView : EditBuildingBlockBaseView, IEditMoleculeStartValuesView
   {
      public EditMoleculeStartValuesView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditMoleculeStartValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddMoleculeStartValuesView(IView view)
      {
         tabEditBuildingBlock.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         EditCaption = AppConstants.Captions.MoleculeStartValues;
         ApplicationIcon = ApplicationIcons.InitialConditions;
      }
   }
}