using MoBi.Core.Domain;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;

namespace MoBi.Core.Serialization.Xml.Serializer
{
   public class MoBiCoreXmlSerializerRepository : OSPSuiteXmlSerializerRepository, IMoBiXmlSerializerRepository
   {
      protected override void AddInitialSerializer()
      {
         //To Add Serializer Classes from ModelCore
         base.AddInitialSerializer();

         //Remove serializer that will be overwritten in MoBi
         RemoveSerializer("ReactionBuildingBlock");
         RemoveSerializer("SpatialStructure");
         RemoveSerializer("ModelCoreSimulation");

         //To add IOSPSuiteXmlSerializer classes defined in MoBi.Core
         this.AddSerializers(x =>
         {
            x.Implementing<IOSPSuiteXmlSerializer>();
            x.InAssemblyContainingType<IMoBiXmlSerializerRepository>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });
      }

      protected override void AddInitialMappers()
      {
         base.AddInitialMappers();
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<NodeSize, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<NotificationType, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<OriginalQuantityValue.Types, SerializationContext>());
      }
   }
}
