using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Views;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class CreateModuleView : BaseModalView, ICreateModuleView
   {
      private readonly ScreenBinder<CreateModuleDTO> _screenBinder = new ScreenBinder<CreateModuleDTO>();

      public CreateModuleView()
      {
         InitializeComponent();
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.Module;
         moduleNameItem.Text = $"{Module} {Captions.Name}".FormatForLabel();
         
         cbSpatialStructure.Text = SpatialStructure;
         cbEventGroup.Text = Event;
         cbReactions.Text = Reactions;
         cbMolecules.Text = Molecules;
         cbObservers.Text = Observer;
         cbPassiveTransports.Text = PassiveTransports;
         cbMoleculeStartValues.Text = MoleculeStartValues;
         cbParameterStartValues.Text = ParameterStartValues;
         createBuildingBlocksGroup.Text = CreateBuildingBlocks;
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(dto => dto.Name).To(tbModuleName);
         _screenBinder.Bind(dto => dto.WithSpatialStructure).To(cbSpatialStructure);
         _screenBinder.Bind(dto => dto.WithEventGroup).To(cbEventGroup);
         _screenBinder.Bind(dto => dto.WithMolecule).To(cbMolecules);
         _screenBinder.Bind(dto => dto.WithObserver).To(cbObservers);
         _screenBinder.Bind(dto => dto.WithPassiveTransport).To(cbPassiveTransports);
         _screenBinder.Bind(dto => dto.WithReaction).To(cbReactions);
         _screenBinder.Bind(dto => dto.WithMoleculeStartValues).To(cbMoleculeStartValues);
         _screenBinder.Bind(dto => dto.WithParameterStartValues).To(cbParameterStartValues);

         RegisterValidationFor(_screenBinder);
      }

      public void AttachPresenter(ICreateModulePresenter presenter)
      {
         
      }

      public void BindTo(CreateModuleDTO createModuleDTO)
      {
         _screenBinder.BindToSource(createModuleDTO);
      }
   }
}
