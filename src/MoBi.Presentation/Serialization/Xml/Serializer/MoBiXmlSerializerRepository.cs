using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Serialization;
using OSPSuite.Presentation.Serialization.Extensions;
//using OSPSuite.Presentation.Serialization;
//using OSPSuite.Presentation.Serialization.Extensions;
using OSPSuite.Serializer;
using OSPSuite.Serializer.Attributes;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class MoBiXmlSerializerRepository : OSPSuiteXmlSerializerRepository, IMoBiXmlSerializerRepository
   {
      protected override void AddInitialSerializer()
      {
         //To Add Serializer Classes from ModelCore
         base.AddInitialSerializer();

         //Remove serializer that will be overwritten in MoBi
         RemoveSerializer("ReactionBuildingBlock");
         RemoveSerializer("SpatialStructure");
         RemoveSerializer("BuildConfiguration");
         RemoveSerializer("ModelCoreSimulation");

         //To add IOSPSuiteXmlSerializer classes defined in MoBi
         this.AddSerializers(x =>
         {
            x.Implementing<IOSPSuiteXmlSerializer>();
            x.InAssemblyContainingType<IMoBiXmlSerializerRepository>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });

         this.AddSerializers(x =>
         {
            x.Implementing<IOSPSuiteXmlSerializer>();
            x.InAssemblyContainingType<MoBiXmlSerializerRepository>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });

         //To add IPresentationXmlSerializer classes defined in MoBi
         this.AddSerializers(x =>
         {
            x.Implementing<IPresentationXmlSerializer>();
            x.InAssemblyContainingType<MoBiXmlSerializerRepository>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });

         this.AddPresentationSerializers();
      }

      protected override void AddInitialMappers()
      {
         base.AddInitialMappers();
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<NodeSize, SerializationContext>());
         AttributeMapperRepository.AddAttributeMapper(new EnumAttributeMapper<NotificationType, SerializationContext>());
      }
   }
}