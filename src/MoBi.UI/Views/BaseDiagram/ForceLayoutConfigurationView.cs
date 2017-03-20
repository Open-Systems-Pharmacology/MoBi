using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views.BaseDiagram
{
   public partial class ForceLayoutConfigurationView : BaseUserControl, IForceLayoutConfigurationView
   {
      private ScreenBinder<IForceLayoutConfiguration> _screenBinder;

      public ForceLayoutConfigurationView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISimpleEditPresenter<IForceLayoutConfiguration> presenter)
      {
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<IForceLayoutConfiguration>();

         _screenBinder.Bind(config => config.MaxIterations).To(_txtMaxIterations);
         _screenBinder.Bind(config => config.Epsilon).To(_txtEpsilon);
         _screenBinder.Bind(config => config.InfinityDistance).To(_txtInfinityDistance);

         _screenBinder.Bind(config => config.ArrangementSpacingWidth).To(_txtArrangementSpacingWidth);
         _screenBinder.Bind(config => config.ArrangementSpacingHeight).To(_txtArrangementSpacingHeight);
         _screenBinder.Bind(config => config.LogPositions).To(_chkLogPositions);

         _screenBinder.Bind(config => config.BaseElectricalCharge).To(_txtBaseElectricalCharge);
         _screenBinder.Bind(config => config.BaseGravitationalMass).To(_txtBaseGravitationalMass);
         _screenBinder.Bind(config => config.BaseSpringLength).To(_txtBaseSpringLength);
         _screenBinder.Bind(config => config.BaseSpringStiffness).To(_txtBaseSpringStiffness);

         _screenBinder.Bind(config => config.RelativeElectricalChargeLinkless).To(_txtRelativeElectricalChargeLinkless);
         _screenBinder.Bind(config => config.RelativeElectricalChargeContainer).To(_txtRelativeElectricalChargeContainer);
         _screenBinder.Bind(config => config.RelativeElectricalChargeNeighborhood).To(_txtRelativeElectricalChargeNeighborhood);
         _screenBinder.Bind(config => config.RelativeElectricalChargeNeighborPort).To(_txtRelativeElectricalChargeNeighborPort);
         _screenBinder.Bind(config => config.RelativeElectricalChargeMolecule).To(_txtRelativeElectricalChargeMolecule);
         _screenBinder.Bind(config => config.RelativeElectricalChargeObserver).To(_txtRelativeElectricalChargeObserver);
         _screenBinder.Bind(config => config.RelativeElectricalChargeReaction).To(_txtRelativeElectricalChargeReaction);

         _screenBinder.Bind(config => config.RelativeGravitationalMassLinkless).To(_txtRelativeGravitationalMassLinkless);
         _screenBinder.Bind(config => config.RelativeGravitationalMassContainer).To(_txtRelativeGravitationalMassContainer);
         _screenBinder.Bind(config => config.RelativeGravitationalMassNeighborhood).To(_txtRelativeGravitationalMassNeighborhood);
         _screenBinder.Bind(config => config.RelativeGravitationalMassNeighborPort).To(_txtRelativeGravitationalMassNeighborPort);
         _screenBinder.Bind(config => config.RelativeGravitationalMassMolecule).To(_txtRelativeGravitationalMassMolecule);
         _screenBinder.Bind(config => config.RelativeGravitationalMassObserver).To(_txtRelativeGravitationalMassObserver);
         _screenBinder.Bind(config => config.RelativeGravitationalMassReaction).To(_txtRelativeGravitationalMassReaction);

         _screenBinder.Bind(config => config.RelativeSpringLengthContainerContainer).To(_txtRelativeSpringLengthContainerContainer);
         _screenBinder.Bind(config => config.RelativeSpringLengthContainerNeighborhood).To(_txtRelativeSpringLengthContainerNeighborhood);
         _screenBinder.Bind(config => config.RelativeSpringLengthNeighborPortNeighborhood).To(_txtRelativeSpringLengthNeighborPortNeighborhood);
         _screenBinder.Bind(config => config.RelativeSpringLengthMoleculeNeighborPort).To(_txtRelativeSpringLengthMoleculeNeighborPort);
         _screenBinder.Bind(config => config.RelativeSpringLengthObserverMolecule).To(_txtRelativeSpringLengthObserverMolecule);
         _screenBinder.Bind(config => config.RelativeSpringLengthReactionMolecule).To(_txtRelativeSpringLengthReactionMolecule);

         _screenBinder.Bind(config => config.RelativeSpringStiffnessContainerContainer).To(_txtRelativeSpringStiffnessContainerContainer);
         _screenBinder.Bind(config => config.RelativeSpringStiffnessContainerNeighborhood).To(_txtRelativeSpringStiffnessContainerNeighborhood);
         _screenBinder.Bind(config => config.RelativeSpringStiffnessNeighborPortNeighborhood).To(_txtRelativeSpringStiffnessNeighborPortNeighborhood);
         _screenBinder.Bind(config => config.RelativeSpringStiffnessMoleculeNeighborPort).To(_txtRelativeSpringStiffnessMoleculeNeighborPort);
         _screenBinder.Bind(config => config.RelativeSpringStiffnessObserverMolecule).To(_txtRelativeSpringStiffnessObserverMolecule);
         _screenBinder.Bind(config => config.RelativeSpringStiffnessReactionMolecule).To(_txtRelativeSpringStiffnessReactionMolecule);

         _screenBinder.Bind(config => config.RelativeElectricalChargeRemote).To(_txtRelativeElectricalChargeRemote);
         _screenBinder.Bind(config => config.RelativeGravitationalMassRemote).To(_txtRelativeGravitationalMassRemote);
         _screenBinder.Bind(config => config.RelativeSpringLengthContainerRemote).To(_txtRelativeSpringLengthContainerRemote);
         _screenBinder.Bind(config => config.RelativeSpringStiffnessContainerRemote).To(_txtRelativeSpringStiffnessContainerRemote);
  
      }

      public void Show(IForceLayoutConfiguration configuration)
      {
         _screenBinder.BindToSource(configuration);
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError; }
      }
   }
}