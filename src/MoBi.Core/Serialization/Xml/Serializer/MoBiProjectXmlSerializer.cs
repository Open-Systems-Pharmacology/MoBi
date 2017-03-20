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
         Map(x => x.DimensionFactory);
         Map(x => x.ReactionDimensionMode);
         MapEnumerable(x => x.AllObservedData, x => x.AddObservedData);
      }

      protected override void TypedDeserialize(MoBiProject project, XElement outputToDeserialize, SerializationContext serializationContext)
      {
         base.TypedDeserialize(project, outputToDeserialize,serializationContext);
         var dimFactory = IoC.Resolve<IMoBiContext>().DimensionFactory;
         dimFactory.ProjectFactory = project.DimensionFactory;
      }

      public override MoBiProject CreateObject(XElement element, SerializationContext serializationContext)
      {
         return IoC.Resolve<IMoBiProject>() as MoBiProject;
      }
   }
}