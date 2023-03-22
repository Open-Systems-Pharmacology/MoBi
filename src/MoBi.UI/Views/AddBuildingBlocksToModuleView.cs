using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Views;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class AddBuildingBlocksToModuleView : BaseModalView, IAddBuildingBlocksToModuleView
   {
      private readonly ScreenBinder<AddBuildingBlocksToModuleDTO> _screenBinder = new ScreenBinder<AddBuildingBlocksToModuleDTO>();

      public AddBuildingBlocksToModuleView()
      {
         InitializeComponent();
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.Module;

         cbSpatialStructure.Text = SpatialStructure;
         cbEventGroup.Text = Event;
         cbReactions.Text = Reactions;
         cbMolecules.Text = Molecules;
         cbObservers.Text = Observer;
         cbPassiveTransports.Text = PassiveTransports;
         cbMoleculeStartValues.Text = MoleculeStartValues;
         cbParameterStartValues.Text = ParameterStartValues;
         createBuildingBlocksGroup.Text = AddSelectedBuildingBlocks;
      }

      public void AttachPresenter(IAddBuildingBlocksToModulePresenter presenter)
      {
      }

      public override void InitializeBinding()
      {
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

      public void BindTo(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO)
      {
         _screenBinder.BindToSource(addBuildingBlocksToModuleDTO);
      }

      public void DisableExistingBuildingBlocks(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO)
      {
         if (addBuildingBlocksToModuleDTO.AlreadyHasSpatialStructure)
         {
            cbSpatialStructure.Checked = true;
            spatialStructureItem.Enabled = false;
         }

         if (addBuildingBlocksToModuleDTO.AlreadyHasEventGroup)
         {
            cbEventGroup.Checked = true;
            eventGroupItem.Enabled = false;
         }

         if (addBuildingBlocksToModuleDTO.AlreadyHasReaction)
         {
            cbReactions.Checked = true;
            reactionsItem.Enabled = false;
         }

         if (addBuildingBlocksToModuleDTO.AlreadyHasMolecule)
         {
            cbMolecules.Checked = true;
            moleculesItem.Enabled = false;
         }

         if (addBuildingBlocksToModuleDTO.AlreadyHasObserver)
         {
            cbObservers.Checked = true;
            observersItem.Enabled = false;
         }

         if (addBuildingBlocksToModuleDTO.AlreadyHasPassiveTransport)
         {
            cbPassiveTransports.Checked = true;
            passiveTransportsItem.Enabled = false;
         }
      }
   }
}