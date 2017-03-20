using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class MoBiBuildConfigurationXmlSerializer : BuildConfigurationXmlSerializer<MoBiBuildConfiguration>
   {
      public MoBiBuildConfigurationXmlSerializer() : base(AppConstants.XmlNames.BuildConfiguration)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.SpatialStructureInfo);
         Map(x => x.MoleculesInfo);
         Map(x => x.ReactionsInfo);
         Map(x => x.PassiveTransportsInfo);
         Map(x => x.ObserversInfo);
         Map(x => x.EventGroupsInfo);
         Map(x => x.ParameterStartValuesInfo);
         Map(x => x.MoleculeStartValuesInfo);
         Map(x => x.SimulationSettingsInfo);
         base.PerformMapping();
      }
   }
}