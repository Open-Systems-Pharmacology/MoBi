using OSPSuite.Assets;
using DevExpress.XtraEditors;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditMoleculesBuildingBlockView : EditBuildingBlockBaseView, IEditMoleculesBuildingBlockView
   {
      public EditMoleculesBuildingBlockView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         splitContainer.CollapsePanel = SplitCollapsePanel.Panel1;
         EditCaption =  AppConstants.Captions.Molecules;
      }

      public void AttachPresenter(IEditMoleculeBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public override bool HasError
      {
         get { return moleculeErrorProvider.HasErrors; }
      }

      public void SetListView(IView view)
      {
         splitContainer.Panel1.FillWith(view);
      }

      public void SetEditView(IView editEventView)
      {
         splitContainer.Panel2.FillWith(editEventView);
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return ApplicationIcons.Molecule; }
      }
   }
}