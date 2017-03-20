using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Container;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class MergedDimensionFactorySerializer : OSPSuiteXmlSerializer<DimensionFactory>
   {
      private readonly IXmlSerializer<SerializationContext> _serializer;

      public MergedDimensionFactorySerializer()
      {
         var unitSystemRepositoty = IoC.Resolve<IUnitSystemXmlSerializerRepository>();
         _serializer = unitSystemRepositoty.SerializerFor<IDimensionFactory>();
      }

      public override DimensionFactory CreateObject(XElement element, SerializationContext serializationContext)
      {
         return new MoBiMergedDimensionFactory();
      }

      public override void PerformMapping()
      {
         //nothing to do here as the serialization action will be performed in the overload
      }

      protected override XElement TypedSerialize(DimensionFactory objectToSerialize, SerializationContext serializationContext)
      {
         return _serializer.Serialize(objectToSerialize, serializationContext);
      }

      protected override void TypedDeserialize(DimensionFactory objectToDeserialize, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         _serializer.Deserialize(objectToDeserialize, outputToDeserialize, serializationContext);
      }
   }
}