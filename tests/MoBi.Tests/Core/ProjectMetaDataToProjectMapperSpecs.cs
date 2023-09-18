using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Serialization.ORM.Mappers;
using MoBi.Core.Serialization.ORM.MetaData;
using MoBi.Core.Serialization.Services;
using MoBi.Core.Serialization.Xml.Services;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Utility;

namespace MoBi.Core
{
   public class concern_for_ProjectMetaDataToProjectMapper : ContextSpecification<ProjectMetaDataToProjectMapper>
   {
      private IDeserializedReferenceResolver _deserializedReferenceResolver;
      protected IXmlSerializationService _serializationService;
      private ISerializationContextFactory _serializationContextFactory;
      protected MoBiProject _project;
      protected ProjectMetaData _projectMetaData;
      private IModuleFactory _moduleFactory;

      protected override void Context()
      {
         _deserializedReferenceResolver = A.Fake<IDeserializedReferenceResolver>();
         _serializationService = A.Fake<IXmlSerializationService>();
         _serializationContextFactory = A.Fake<ISerializationContextFactory>();
         _project = new MoBiProject();
         _projectMetaData = new ProjectMetaData();
         A.CallTo(() => _serializationService.Deserialize<MoBiProject>(_projectMetaData.Content.Data, A<MoBiProject>._, A<SerializationContext>._)).Returns(_project);

         _moduleFactory = A.Fake<IModuleFactory>();
         sut = new ProjectMetaDataToProjectMapper(_serializationService, _serializationContextFactory, _deserializedReferenceResolver, _moduleFactory);
      }
   }

   public class When_mapping_metadata_without_any_orphan_building_blocks : concern_for_ProjectMetaDataToProjectMapper
   {
      protected override void Context()
      {
         base.Context();
         _projectMetaData.AddChild(new EntityMetaData { Id = ShortGuid.NewGuid() });
         A.CallTo(() => _serializationService.Deserialize<object>(_projectMetaData.Children.First().Content.Data, A<MoBiProject>._, A<SerializationContext>._)).Returns(new Module());
      }

      protected override void Because()
      {
         sut.MapFrom(_projectMetaData);
      }

      [Observation]
      public void there_should_only_be_one_module_loaded()
      {
         _project.Modules.Count.ShouldBeEqualTo(1);
      }
   }

   public class When_mapping_metadata_with_an_orphan_building_block : concern_for_ProjectMetaDataToProjectMapper
   {
      protected override void Context()
      {
         base.Context();
         _projectMetaData.AddChild(new EntityMetaData { Id = ShortGuid.NewGuid() });
         _projectMetaData.AddChild(new EntityMetaData { Id = ShortGuid.NewGuid() });
         _projectMetaData.Children.ElementAt(0).Content.Data = new byte[] { 0 };
         _projectMetaData.Children.ElementAt(1).Content.Data = new byte[] { 0 };
         A.CallTo(() => _serializationService.Deserialize<object>(_projectMetaData.Children.ElementAt(0).Content.Data, A<MoBiProject>._, A<SerializationContext>._)).Returns(new Module().WithName("module"));
         A.CallTo(() => _serializationService.Deserialize<object>(_projectMetaData.Children.ElementAt(1).Content.Data, A<MoBiProject>._, A<SerializationContext>._)).Returns(new MoBiSpatialStructure().WithName("spatialstructure"));
      }

      protected override void Because()
      {
         sut.MapFrom(_projectMetaData);
      }

      [Observation]
      public void there_should_be_two_modules()
      {
         _project.Modules.Count.ShouldBeEqualTo(2);
      }
   }
}