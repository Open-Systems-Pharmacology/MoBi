using System;
using System.Linq;
using System.Xml.Linq;
using OSPSuite.Serializer.Xml;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Compression;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Serialization.Converter;
using MoBi.Core.Serialization.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;

namespace MoBi.Core.Serialization.Xml.Services
{
   public interface IXmlSerializationService
   {
      XElement SerializeModelPart<T>(T entityToSerialize);
      T Deserialize<T>(XElement element, IMoBiProject project, int version);
      T Deserialize<T>(string xmlString, IMoBiProject project);
      T Deserialize<T>(byte[] compressedBytes, IMoBiProject project, SerializationContext serializationContext = null);
      string ElementNameFor(Type type);
      string SerializeAsString<T>(T entityToSerialize);
      byte[] SerializeAsBytes<T>(T entityToSerialize);
      int VersionFrom(XElement element);
   }

   public class XmlSerializationService : IXmlSerializationService
   {
      private readonly IMoBiXmlSerializerRepository _repository;
      private readonly ICompression _compression;
      private readonly IMoBiObjectConverterFinder _objectConverterFinder;
      private readonly ISerializationContextFactory _serializationContextFactory;
      private readonly IEventPublisher _eventPublisher;
      private readonly IDeserializedReferenceResolver _deserializedReferenceResolver;
      private readonly IXmlSerializer<SerializationContext> _formulaCacheSerializer;

      public XmlSerializationService(IMoBiXmlSerializerRepository repository, ICompression compression, IMoBiObjectConverterFinder objectConverterFinder,
         ISerializationContextFactory serializationContextFactory, IEventPublisher eventPublisher, IDeserializedReferenceResolver deserializedReferenceResolver)
      {
         _repository = repository;
         _compression = compression;
         _objectConverterFinder = objectConverterFinder;
         _serializationContextFactory = serializationContextFactory;
         _eventPublisher = eventPublisher;
         _deserializedReferenceResolver = deserializedReferenceResolver;
         _formulaCacheSerializer = _repository.SerializerFor<IFormulaCache>();
      }

      public XElement SerializeModelPart<T>(T entityToSerialize)
      {
         using (var serializationContext = _serializationContextFactory.Create())
         {
            var partSerializer = _repository.SerializerFor(entityToSerialize.GetType());
            var xElement = partSerializer.Serialize(entityToSerialize, serializationContext);
            xElement.AddAttribute(Constants.Serialization.Attribute.VERSION, ProjectVersions.CurrentAsString);
            addFormulaCache(entityToSerialize, xElement, serializationContext);
            return xElement;
         }
      }

      private void addFormulaCache(object entityToSerialize, XElement xElement, SerializationContext serializationContext)
      {
         //Formula cache already added for building block
         if (entityToSerialize.IsAnImplementationOf<IBuildingBlock>())
            return;

         if (!serializationContext.Formulas.Any())
            return;

         xElement.Add(serializeFormulas(serializationContext.Formulas, serializationContext));
      }

      private XElement serializeFormulas(IFormulaCache formulas, SerializationContext serializationContext)
      {
         return _formulaCacheSerializer.Serialize(formulas, serializationContext);
      }

      public T Deserialize<T>(XElement element, IMoBiProject project, int version)
      {
         return deserialize(element, project, version, typeof (T)).DowncastTo<T>();
      }

      public T Deserialize<T>(byte[] compressedBytes, IMoBiProject project, SerializationContext serializationContext = null)
      {
         var decompressesBytes = _compression.Decompress(compressedBytes);
         var element = XmlHelper.ElementFromBytes(decompressesBytes);
         return deserialize(element, project, VersionFrom(element), parentSerializationContext: serializationContext).DowncastTo<T>();
      }

      private object deserialize(XElement element, IMoBiProject project, int version, Type type = null, SerializationContext parentSerializationContext = null)
      {
         object deserializedObject;
         using (var serializationContext = _serializationContextFactory.Create(parentSerializationContext))
         {
            convertXml(element, version, project);

            IXmlSerializer<SerializationContext> serializer;
            Type deserializeType;

            if (type == null)
            {
               serializer = _repository.SerializerFor(element);
               deserializeType = serializer.ObjectType;
            }
            else
            {
               serializer = serializeFor(type);
               deserializeType = type;
            }

            var formulaCacheElement = getFormulaCacheElementFor(element, deserializeType);
            convertXml(formulaCacheElement, version, project);
            deserializeFormula(formulaCacheElement, version, project, serializationContext);

            deserializedObject = serializer.Deserialize(element, serializationContext);
         }

         //Performs the conversion to the latest project version
         var conversionHappened = convert(deserializedObject, version, project);
         //Once the project was converted, update all formula references
         _deserializedReferenceResolver.ResolveFormulaAndTemplateReferences(deserializedObject, project);

         if (conversionHappened)
            _eventPublisher.PublishEvent(new ObjectConvertedEvent(deserializedObject, ProjectVersions.FindBy(version)));

         return deserializedObject;
      }

      private void convertXml(XElement sourceElement, int version, IMoBiProject project)
      {
         if (sourceElement == null) return;
         //set version to avoid double conversion in the case of multiple load
         convert(sourceElement, project, version, x => x.ConvertXml);
         sourceElement.SetAttributeValue(Constants.Serialization.Attribute.VERSION, ProjectVersions.CurrentAsString);
      }

      /// <summary>
      ///    Converts the TObject to convert after the deserialization was made.
      /// </summary>
      /// <returns><c>true</c> if a conversion was performed otherwise <c>false</c></returns>
      private bool convert(object deserializedObject, int objectVersion, IMoBiProject project)
      {
         var hasChanged = convert(deserializedObject, project, objectVersion, x => x.Convert);
         var simulation = deserializedObject as IMoBiSimulation;

         //Ensure that a simulation is marked as changed so that converted changes will also be persisted when the project is saved
         if (simulation != null)
            simulation.HasChanged = hasChanged;

         return hasChanged;
      }

      private bool convert<T>(T objectToConvert, IMoBiProject project, int objectVersion, Func<IMoBiObjectConverter, Func<T, IMoBiProject, int>> converterAction)
      {
         int version = objectVersion;
         while (version != ProjectVersions.Current)
         {
            var converter = _objectConverterFinder.FindConverterFor(version);
            version = converterAction(converter).Invoke(objectToConvert, project);
         }

         return objectVersion != ProjectVersions.Current;
      }

      private bool areFormulasAlreadyHandled(Type deserializeType)
      {
         return deserializeType.IsAnImplementationOf<IModelCoreSimulation>()
                || deserializeType.IsAnImplementationOf<IBuildingBlock>()
                || deserializeType.IsAnImplementationOf<IBuildConfiguration>()
                || deserializeType.IsAnImplementationOf<IMoBiProject>()
                || deserializeType.IsAnImplementationOf<IModel>()
                || deserializeType.IsAnImplementationOf<SimulationTransfer>();
      }

      private XElement getFormulaCacheElementFor(XElement element, Type deserializeType)
      {
         if (element == null || areFormulasAlreadyHandled(deserializeType))
            return null;

         var formulaCacheElement = element.Element(_formulaCacheSerializer.ElementName);
         return formulaCacheElement ?? getFormulaCacheElementFor(element.Parent, deserializeType);
      }

      private void deserializeFormula(XElement formulaCacheElement, int version, IMoBiProject project, SerializationContext serializationContext)
      {
         if (formulaCacheElement == null) return;
         _formulaCacheSerializer.Deserialize(formulaCacheElement, serializationContext);
         convert(serializationContext.Formulas, version, project);
      }

      public int VersionFrom(XElement element)
      {
         string versionString = element.GetAttribute(Constants.Serialization.Attribute.VERSION);
         if (string.IsNullOrEmpty(versionString))
            return ProjectVersions.V3_0_1_to_3;

         return versionString.ConvertedTo<int>();
      }

      public T Deserialize<T>(string xmlString, IMoBiProject project)
      {
         var element = XmlHelper.RootElementFromString(xmlString);
         return Deserialize<T>(element, project, VersionFrom(element));
      }

      public string ElementNameFor(Type type)
      {
         return serializeFor(type).ElementName;
      }

      private IXmlSerializer<SerializationContext> serializeFor(Type type)
      {
         return _repository.SerializerFor(type);
      }

      public string SerializeAsString<T>(T entityToSerialize)
      {
         return SerializeModelPart(entityToSerialize).ToString(SaveOptions.DisableFormatting);
      }

      public byte[] SerializeAsBytes<T>(T entityToSerialize)
      {
         var element = SerializeModelPart(entityToSerialize);
         return _compression.Compress(XmlHelper.XmlContentToByte(element));
      }
   }
}