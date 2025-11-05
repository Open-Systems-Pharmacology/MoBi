using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Presentation.Serialization.Extensions;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.Serialization
{
   public class SerializerRegister : Register
   {
      private CoreSerializerRegister _register;

      public override void RegisterInContainer(IContainer container)
      {
         _register = new CoreSerializerRegister();
         container.AddRegister(x => x.FromInstance(_register));
      }

      public void PerformMappingForSerializerIn(IContainer container)
      {
         var coreSerializerRepository = container.Resolve<IOSPSuiteXmlSerializerRepository>();
         coreSerializerRepository.AddPresentationSerializers();
         _register.PerformMappingForSerializerIn(container);

         PerformMappingForMoBiSerializerRepository(container);
      }

      public void PerformMappingForMoBiSerializerRepository(IContainer container)
      {
         var mobiSerializerRepository = container.Resolve<IMoBiXmlSerializerRepository>();
         mobiSerializerRepository.PerformMapping();
      }
   }
}
