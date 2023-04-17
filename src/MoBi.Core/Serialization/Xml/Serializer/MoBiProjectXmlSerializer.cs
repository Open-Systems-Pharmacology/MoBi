using System.Xml.Linq;
using OSPSuite.Utility.Container;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class MoBiProjectXmlSerializer : ProjectXmlSerializer<MoBiProject>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.ReactionDimensionMode);
         Map(x => x.SimulationSettings);
         MapEnumerable(x => x.AllObservedData, x => x.AddObservedData);
      }

      public override MoBiProject CreateObject(XElement element, SerializationContext serializationContext)
      {
         return serializationContext.Resolve<MoBiProject>();
      }
   }
}