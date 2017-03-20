using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class CreateStartValuesView : BaseModalView, ICreateStartValuesView
   {
      private ICreateStartValuesPresenter _presenter;
      private ScreenBinder<StartValuesDTO> _screenBinder;

      public CreateStartValuesView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ICreateStartValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_MoleculeStartValues;

      protected override void SetActiveControl()
      {
         ActiveControl = txtName;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<StartValuesDTO>();
         _screenBinder.Bind(dto => dto.Name).To(txtName);
         _screenBinder.Bind(dto => dto.Molecules).To(cmbMolecules).WithValues(dto => _presenter.GetMolecules());
         _screenBinder.Bind(dto => dto.SpatialStructrue).To(cmbSpatialStructure).WithValues(dto => _presenter.GetSpatialStructures());

         RegisterValidationFor(_screenBinder);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemMolecules.Text = ObjectTypes.MoleculeBuildingBlock.FormatForLabel();
         layoutControlItemSpatialStructure.Text = ObjectTypes.SpatialStructure.FormatForLabel();
         layoutControlItemName.Text = AppConstants.Captions.Name.FormatForLabel();
      }

      public void Show(StartValuesDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }

      public override ApplicationIcon ApplicationIcon
      {
         set { Icon = value; }
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError; }
      }
   }
}