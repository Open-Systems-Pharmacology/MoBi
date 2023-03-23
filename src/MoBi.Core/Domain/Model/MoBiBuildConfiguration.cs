using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiBuildConfiguration : IBuildConfiguration
   {
      MoleculesInfo MoleculesInfo { get; set; }
      ReactionBuildingBlockInfo ReactionsInfo { get; set; }
      PassiveTransportBuildingBlockInfo PassiveTransportsInfo { get; set; }
      SpatialStructureInfo SpatialStructureInfo { get; set; }
      MoleculeStartValuesBuildingBlockInfo MoleculeStartValuesInfo { get; set; }
      ParameterStartValuesBuildingBlockInfo ParameterStartValuesInfo { get; set; }
      ObserverBuildingBlockInfo ObserversInfo { get; set; }
      EventGroupBuildingBlockInfo EventGroupsInfo { get; set; }
      SimulationSettingsBuildingBlockInfo SimulationSettingsInfo { get; set; }
      IEnumerable<IBuildingBlockInfo> AllBuildingBlockInfos();
      bool HasChangedBuildingBlocks();
      IBuildingBlockInfo BuildingInfoForTemplate(IBuildingBlock templateBuildingBlock);
      IBuildingBlockInfo BuildingInfoForTemplateById(string templateBuildingBlockId);
      IMoBiSpatialStructure MoBiSpatialStructure { get; set; }
      IMoBiReactionBuildingBlock MoBiReactions { get; set; }
   }

   public class MoBiBuildConfiguration : BuildConfiguration, IMoBiBuildConfiguration
   {
      public MoleculesInfo MoleculesInfo { get; set; }
      public ReactionBuildingBlockInfo ReactionsInfo { get; set; }
      public PassiveTransportBuildingBlockInfo PassiveTransportsInfo { get; set; }
      public SpatialStructureInfo SpatialStructureInfo { get; set; }
      public MoleculeStartValuesBuildingBlockInfo MoleculeStartValuesInfo { get; set; }
      public ParameterStartValuesBuildingBlockInfo ParameterStartValuesInfo { get; set; }
      public ObserverBuildingBlockInfo ObserversInfo { get; set; }
      public EventGroupBuildingBlockInfo EventGroupsInfo { get; set; }
      public SimulationSettingsBuildingBlockInfo SimulationSettingsInfo { get; set; }

      public MoBiBuildConfiguration()
      {
         MoleculesInfo = new MoleculesInfo();
         ReactionsInfo = new ReactionBuildingBlockInfo();
         PassiveTransportsInfo = new PassiveTransportBuildingBlockInfo();
         SpatialStructureInfo = new SpatialStructureInfo();
         MoleculeStartValuesInfo = new MoleculeStartValuesBuildingBlockInfo();
         ParameterStartValuesInfo = new ParameterStartValuesBuildingBlockInfo();
         ObserversInfo = new ObserverBuildingBlockInfo();
         EventGroupsInfo = new EventGroupBuildingBlockInfo();
         SimulationSettingsInfo = new SimulationSettingsBuildingBlockInfo();
      }

      public override MoleculeBuildingBlock Molecules
      {
         get => MoleculesInfo.BuildingBlock;
         set => MoleculesInfo.BuildingBlock = value;
      }

      public override IReactionBuildingBlock Reactions
      {
         get => ReactionsInfo.BuildingBlock;
         set => ReactionsInfo.BuildingBlock = value.DowncastTo<IMoBiReactionBuildingBlock>();
      }

      public override IPassiveTransportBuildingBlock PassiveTransports
      {
         get => PassiveTransportsInfo.BuildingBlock;
         set => PassiveTransportsInfo.BuildingBlock = value;
      }

      public override ISpatialStructure SpatialStructure
      {
         get => SpatialStructureInfo.BuildingBlock;
         set => SpatialStructureInfo.BuildingBlock = value.DowncastTo<IMoBiSpatialStructure>();
      }

      public override MoleculeStartValuesBuildingBlock MoleculeStartValues
      {
         get => MoleculeStartValuesInfo.BuildingBlock;
         set => MoleculeStartValuesInfo.BuildingBlock = value;
      }

      public override ParameterStartValuesBuildingBlock ParameterStartValues
      {
         get => ParameterStartValuesInfo.BuildingBlock;
         set => ParameterStartValuesInfo.BuildingBlock = value;
      }

      public override IObserverBuildingBlock Observers
      {
         get => ObserversInfo.BuildingBlock;
         set => ObserversInfo.BuildingBlock = value;
      }

      public override IEventGroupBuildingBlock EventGroups
      {
         get => EventGroupsInfo.BuildingBlock;
         set => EventGroupsInfo.BuildingBlock = value;
      }

      public override SimulationSettings SimulationSettings
      {
         get => SimulationSettingsInfo.BuildingBlock;
         set => SimulationSettingsInfo.BuildingBlock = value;
      }

      public IEnumerable<IBuildingBlockInfo> AllBuildingBlockInfos()
      {
         yield return MoleculesInfo;
         yield return ReactionsInfo;
         yield return SpatialStructureInfo;
         yield return PassiveTransportsInfo;
         yield return ObserversInfo;
         yield return EventGroupsInfo;
         yield return MoleculeStartValuesInfo;
         yield return ParameterStartValuesInfo;
         yield return SimulationSettingsInfo;
      }

      public IBuildingBlockInfo BuildingInfoForTemplate(IBuildingBlock templateBuildingBlock)
      {
         return BuildingInfoForTemplateById(templateBuildingBlock.Id);
      }

      public IBuildingBlockInfo BuildingInfoForTemplateById(string templateBuildingBlockId)
      {
         return AllBuildingBlockInfos().FirstOrDefault(info => Equals(info.TemplateBuildingBlockId, templateBuildingBlockId));
      }

      public bool HasChangedBuildingBlocks()
      {
         return AllBuildingBlockInfos().Any(bb => bb.BuildingBlockChanged);
      }

      public IMoBiSpatialStructure MoBiSpatialStructure
      {
         get => SpatialStructure as IMoBiSpatialStructure;
         set => SpatialStructure = value;
      }

      public IMoBiReactionBuildingBlock MoBiReactions
      {
         get => Reactions as IMoBiReactionBuildingBlock;
         set => Reactions = value;
      }
   }
}