using System;
using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views.BaseDiagram
{
   public partial class DiagramOptionsView : BaseUserControl, IDiagramOptionsView
   {
      private ScreenBinder<IDiagramOptions> _screenBinder;
      private ScreenBinder<IDiagramColors> _colorBinder;

      public DiagramOptionsView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         defaultSizeOfNewReactionControlItem.Text = AppConstants.Captions.DefaultSizeOfNewReaction.FormatForLabel();
         defaultSizeOfNewMoleculeControlItem.Text = AppConstants.Captions.DefaultSizeOfNewMolecule.FormatForLabel();
         defaultSizeOfNewObserverControlItem.Text = AppConstants.Captions.DefaultSizeOfNewObserver.FormatForLabel();

         colorsLayoutControlGroup.Text = AppConstants.Captions.ChartItemDefaultColors;

         containerLogicalControlItem.Text = AppConstants.Captions.ContainerLogical.FormatForLabel();
         containerPhysicalControlItem.Text = AppConstants.Captions.ContainerPhysical.FormatForLabel();
         containerOpacityControlItem.Text = AppConstants.Captions.ContainerOpacity.FormatForLabel();

         neighborhoodLinkControlItem.Text = AppConstants.Captions.NeighborhoodLink.FormatForLabel();
         neighborhoodNodeControlItem.Text = AppConstants.Captions.NeighborhoodNode.FormatForLabel();
         neighborhoodPortControlItem.Text = AppConstants.Captions.NeighborhoodPort.FormatForLabel();

         transportLinkControlItem.Text = AppConstants.Captions.TransportLink.FormatForLabel();
         observerNodeControlItem.Text = AppConstants.Captions.ObserverNode.FormatForLabel();
         observerLinkControlItem.Text = AppConstants.Captions.ObserverLink.FormatForLabel();

         moleculeNodeControlItem.Text = AppConstants.Captions.MoleculeNode.FormatForLabel();

         reactionNodeControlItem.Text = AppConstants.Captions.ReactionNode.FormatForLabel();
         reactionPortEductControlItem.Text = AppConstants.Captions.ReactionPortEduct.FormatForLabel();
         reactionLinkEductControlItem.Text = AppConstants.Captions.ReactionLinkEduct.FormatForLabel();

         reactionPortProductControlItem.Text = AppConstants.Captions.ReactionPortProduct.FormatForLabel();
         reactionLinkProductControlItem.Text = AppConstants.Captions.ReactionLinkProduct.FormatForLabel();

         reactionPortModifierControlItem.Text = AppConstants.Captions.ReactionPortModifier.FormatForLabel();
         reactionLinkModifierControlItem.Text = AppConstants.Captions.ReactionLinkModifier.FormatForLabel();
      }

      public void AttachPresenter(ISimpleEditPresenter<IDiagramOptions> presenter)
      {
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<IDiagramOptions>();
         _screenBinder.Bind(options => options.SnapGridVisible).To(chkSnapGridVisible);
         _screenBinder.Bind(options => options.MoleculePropertiesVisible).To(chkMoleculePropertiesVisible);
         _screenBinder.Bind(options => options.ObserverLinksVisible).To(chkObserverLinksVisible);
         _screenBinder.Bind(options => options.UnusedMoleculesVisibleInModelDiagram).To(chkUnusedMoleculesVisibleInModelDiagram);
         var items = Enum.GetValues(typeof (NodeSize)) as NodeSize[];
         if (items != null)
            _screenBinder.Bind(options => options.DefaultNodeSizeReaction).To(cbeDefaultNodeSizeReaction).WithValues(items);
         else
            _screenBinder.Bind(options => options.DefaultNodeSizeReaction).To(cbeDefaultNodeSizeReaction).WithValues(getNodeSizes);

         _screenBinder.Bind(options => options.DefaultNodeSizeMolecule).To(cbeDefaultNodeSizeMolecule).WithValues(getNodeSizes);
         _screenBinder.Bind(options => options.DefaultNodeSizeObserver).To(cbeDefaultNodeSizeObserver).WithValues(getNodeSizes);

         _colorBinder = new ScreenBinder<IDiagramColors>();

         _colorBinder.Bind(colors => colors.ContainerLogical).To(coeContainerLogical);
         _colorBinder.Bind(colors => colors.ContainerPhysical).To(coeContainerPhysical);
         _colorBinder.Bind(colors => colors.ContainerOpacity).To(txtContainerOpacity);
         _colorBinder.Bind(colors => colors.NeighborhoodLink).To(coeNeighborhoodLink);
         _colorBinder.Bind(colors => colors.NeighborhoodNode).To(coeNeighborhoodNode);
         _colorBinder.Bind(colors => colors.NeighborhoodPort).To(coeNeighborhoodPort);
         _colorBinder.Bind(colors => colors.TransportLink).To(coeTransportLink);
         _colorBinder.Bind(colors => colors.ObserverNode).To(coeObserverNode);
         _colorBinder.Bind(colors => colors.ObserverLink).To(coeObserverLink);
         _colorBinder.Bind(colors => colors.MoleculeNode).To(coeMoleculeNode);
         _colorBinder.Bind(colors => colors.ReactionNode).To(coeReactionNode);
         _colorBinder.Bind(colors => colors.ReactionPortEduct).To(coeReactionPortEduct);
         _colorBinder.Bind(colors => colors.ReactionLinkEduct).To(coeReactionLinkEduct);
         _colorBinder.Bind(colors => colors.ReactionPortProduct).To(coeReactionPortProduct);
         _colorBinder.Bind(colors => colors.ReactionLinkProduct).To(coeReactionLinkProduct);
         _colorBinder.Bind(colors => colors.ReactionPortModifier).To(coeReactionPortModifier);
         _colorBinder.Bind(colors => colors.ReactionLinkModifier).To(coeReactionLinkModifier);
      }

      private IEnumerable<NodeSize> getNodeSizes(IDiagramOptions diagramOptions)
      {
         return EnumHelper.AllValuesFor<NodeSize>();
      }

      public void Show(IDiagramOptions diagramOptions)
      {
         _screenBinder.BindToSource(diagramOptions);
         _colorBinder.BindToSource(diagramOptions.DiagramColors);
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError || _colorBinder.HasError; }
      }
   }
}