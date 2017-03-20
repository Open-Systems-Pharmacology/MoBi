using MoBi.Core.Domain.Model;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public abstract class BuildingBlockInfoXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : class, IBuildingBlockInfo
   {
      public override void PerformMapping()
      {
         Map(x => x.TemplateBuildingBlockId);
         Map(x => x.SimulationChanges);
      }
   }

   public class SpatialStructureInfoXmlSerializer : BuildingBlockInfoXmlSerializer<SpatialStructureInfo>
   {
   }

   public class MoleculesInfoXmlSerializer : BuildingBlockInfoXmlSerializer<MoleculesInfo>
   {
   }

   public class ReactionsInfoXmlSerializer : BuildingBlockInfoXmlSerializer<ReactionBuildingBlockInfo>
   {
   }

   public class PassiveTransportsInfoXmlSerializer : BuildingBlockInfoXmlSerializer<PassiveTransportBuildingBlockInfo>
   {
   }

   public class ObserverInfoXmlSerializer : BuildingBlockInfoXmlSerializer<ObserverBuildingBlockInfo>
   {
   }

   public class EventGroupsInfoXmlSerializer : BuildingBlockInfoXmlSerializer<EventGroupBuildingBlockInfo>
   {
   }

   public class ParameterStartValuesInfoXmlSerializer : BuildingBlockInfoXmlSerializer<ParameterStartValuesBuildingBlockInfo>
   {
   }

   public class MoleculeStartValuesInfoXmlSerializer : BuildingBlockInfoXmlSerializer<MoleculeStartValuesBuildingBlockInfo>
   {
   }

   public class SimulationSettingsBuildingBlockInfoXmlSerializer : BuildingBlockInfoXmlSerializer<SimulationSettingsBuildingBlockInfo>
   {
   }
}