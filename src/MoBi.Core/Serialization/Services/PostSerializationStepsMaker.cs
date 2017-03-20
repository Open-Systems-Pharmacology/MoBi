using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization;

namespace MoBi.Core.Serialization.Services
{
   public interface IPostSerializationStepsMaker
   {
      void PerformPostDeserializationFor(IMoBiProject project, int originalFileVersion);

      void PerformPostDeserializationFor<T>(IReadOnlyList<T> deserializedObjects, int originalFileVersion, bool resetIds);
   }

   public class PostSerializationStepsMaker : IPostSerializationStepsMaker
   {
      private readonly IObjectIdResetter _objectIdResetter;
      private readonly IAmountToConcentrationConverter _amountToConcentrationConverter;
      private readonly IPassiveTransportConverter _passiveTransportConverter;
      private readonly IProjectClassifiableUpdaterAfterDeserialization _projectClassifiableUpdaterAfterDeserialization;
      private readonly IProjectClassificationConverter _projectClassificationConverter;

      public PostSerializationStepsMaker(IObjectIdResetter objectIdResetter,
         IAmountToConcentrationConverter amountToConcentrationConverter,
         IPassiveTransportConverter passiveTransportConverter, 
         IProjectClassifiableUpdaterAfterDeserialization projectClassifiableUpdaterAfterDeserialization,
         IProjectClassificationConverter projectClassificationConverter)
      {
         _objectIdResetter = objectIdResetter;
         _amountToConcentrationConverter = amountToConcentrationConverter;
         _passiveTransportConverter = passiveTransportConverter;
         _projectClassifiableUpdaterAfterDeserialization = projectClassifiableUpdaterAfterDeserialization;
         _projectClassificationConverter = projectClassificationConverter;
      }

      public void PerformPostDeserializationFor(IMoBiProject project, int originalFileVersion)
      {
         if (originalFileVersion < ProjectVersions.V3_5_1)
            _passiveTransportConverter.Convert(project);


         if (originalFileVersion < ProjectVersions.V6_0_1)
            //this needs to be done after all object were loaded into project
            _projectClassificationConverter.Convert(project);
            
         //this needs to be done once the project was loaded into context
         _projectClassifiableUpdaterAfterDeserialization.Update(project);
      }

      public void PerformPostDeserializationFor<T>(IReadOnlyList<T> deserializedObjects, int originalFileVersion, bool resetIds)
      {
         if (resetIds)
            deserializedObjects.Each(x => _objectIdResetter.ResetIdFor(x));

         //Convert loaded object to concentration if required
         deserializedObjects.Each(o => _amountToConcentrationConverter.Convert(o));

         //Move passive transports defined in moelcules in the passive transport building block. This needs to be done after the deserialization
         //step as we might need to add new buildging block to project. Required only for project older than 3.5
         if (originalFileVersion < ProjectVersions.V3_5_1)
            deserializedObjects.Each(o => _passiveTransportConverter.Convert(o));
      }
   }
}