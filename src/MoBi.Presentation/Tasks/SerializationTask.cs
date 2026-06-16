using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.ORM;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks
{
   public interface ISerializationTask : ICoreSerializationTask
   {
      /// <summary>
      ///    Loads multiple instances of the element type <typeparamref name="T" /> from the <paramref name="fileName" />.
      ///    If more than one element of the type is present in the file, the user will confirm which elements to load by name.
      ///    If <paramref name="resetIds" /> is true, then all element ID's will be set to new values on load.
      /// </summary>
      /// <returns>A new list of elements that were loaded</returns>
      IReadOnlyList<T> LoadMany<T>(string fileName, bool resetIds = false);
   }

   public class SerializationTask : CoreSerializationTask, ISerializationTask
   {
      private readonly IXmlContentSelector _xmlContentSelector;

      public SerializationTask(IXmlSerializationService xmlSerializationService, IContextPersistor contextPersistor,
         IObjectTypeResolver objectTypeResolver, IDialogCreator dialogCreator,
         IXmlContentSelector xmlContentSelector, IProjectConverterLogger projectConverterLogger,
         IMoBiContext context, IPostSerializationStepsMaker postSerializationSteps, IHeavyWorkManager heavyWorkManager)
         : base(xmlSerializationService, contextPersistor, objectTypeResolver, dialogCreator, projectConverterLogger, context, postSerializationSteps, heavyWorkManager)
      {
         _xmlContentSelector = xmlContentSelector;
      }

      public override T Load<T>(string fileName, bool resetIds = false) => LoadMany<T>(fileName, resetIds).FirstOrDefault();

      public IReadOnlyList<T> LoadMany<T>(string fileName, bool resetIds = false) =>
         loadMany<T>(fileName, x => selectToDeserialize(x, _objectTypeResolver.TypeFor<T>()), resetIds).ToList();

      private IReadOnlyList<XElement> selectToDeserialize(IEnumerable<XElement> possibleElements, string searchedEntityType) =>
         _xmlContentSelector.SelectFrom(possibleElements, searchedEntityType).ToList();
   }
}