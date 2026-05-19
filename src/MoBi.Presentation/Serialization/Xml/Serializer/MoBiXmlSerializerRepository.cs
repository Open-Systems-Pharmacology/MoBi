using MoBi.Core.Serialization.Xml.Serializer;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Serialization;
using OSPSuite.Presentation.Serialization.Extensions;
using OSPSuite.Serializer;

namespace MoBi.Presentation.Serialization.Xml.Serializer
{
   public class MoBiXmlSerializerRepository : MoBiCoreXmlSerializerRepository
   {
      protected override void AddInitialSerializer()
      {
         //Adds OSPSuite.Core + MoBi.Core serializers
         base.AddInitialSerializer();

         //Adds IOSPSuiteXmlSerializer classes defined in MoBi.Presentation
         this.AddSerializers(x =>
         {
            x.Implementing<IOSPSuiteXmlSerializer>();
            x.InAssemblyContainingType<MoBiXmlSerializerRepository>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });

         //Adds IPresentationXmlSerializer classes defined in MoBi.Presentation
         this.AddSerializers(x =>
         {
            x.Implementing<IPresentationXmlSerializer>();
            x.InAssemblyContainingType<MoBiXmlSerializerRepository>();
            x.UsingAttributeRepository(AttributeMapperRepository);
         });

         this.AddPresentationSerializers();
      }
   }
}