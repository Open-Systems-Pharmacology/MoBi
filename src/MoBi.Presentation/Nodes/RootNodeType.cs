using MoBi.Assets;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.Nodes
{
   public class MoBiRootNodeTypes
   {
      public static readonly RootNodeType SpatialStructureFolder = new RootNodeType(AppConstants.Captions.SpatialStructures, ApplicationIcons.SpatialStructureFolder);
      public static readonly RootNodeType MoleculeFolder = new RootNodeType(AppConstants.Captions.Molecules, ApplicationIcons.MoleculeFolder);
      public static readonly RootNodeType EventFolder = new RootNodeType(AppConstants.Captions.Events, ApplicationIcons.EventFolder);
      public static readonly RootNodeType ObserverFolder = new RootNodeType(AppConstants.Captions.Observers, ApplicationIcons.ObserverFolder);
      public static readonly RootNodeType PassiveTransportFolder = new RootNodeType(AppConstants.Captions.PassiveTransports, ApplicationIcons.PassiveTransportFolder);
      public static readonly RootNodeType ReactionFolder = new RootNodeType(AppConstants.Captions.Reactions, ApplicationIcons.ReactionFolder);
      public static readonly RootNodeType MoleculeStartValuesFolder = new RootNodeType(AppConstants.Captions.MoleculeStartValues, ApplicationIcons.MoleculeStartValuesFolder);
      public static readonly RootNodeType ParameterStartValuesFolder = new RootNodeType(AppConstants.Captions.ParameterStartValues, ApplicationIcons.ParameterStartValuesFolder);
      public static readonly RootNodeType SimulationSettingsFolder = new RootNodeType(AppConstants.Captions.SimulationSettings, ApplicationIcons.SimulationSettingsFolder);
   }
}