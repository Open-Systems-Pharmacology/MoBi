using System;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public class TagConditionCommandParameters<T>
   {
      /// <summary>
      /// action required to retrieve a descriptor criteria 
      /// </summary>
      public Func<T, DescriptorCriteria> DescriptorCriteriaRetriever { get; set; }

      /// <summary>
      /// action required to create a descriptor criteria (just in case they are not created automatically)
      /// </summary>
      public Func<T, DescriptorCriteria> DescriptorCriteriaCreator { get; set; }

      public T TaggedObject { get; set; }

      public IBuildingBlock BuildingBlock { get; set; }
   }

   public abstract class TagConditionCommandBase<T> : BuildingBlockChangeCommandBase<IBuildingBlock> where T : class, IObjectBase
   {
      protected T _taggedObject;
      protected readonly Func<T,DescriptorCriteria> _descriptorCriteriaRetriever;
      protected string _tag;
      private readonly string _taggedObjectId;
      protected readonly Func<T, DescriptorCriteria> _descriptorCriteriaCreator;

      protected TagConditionCommandBase(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tagConditionCommandParameters.BuildingBlock)
      {
         _tag = tag;
         _taggedObject = tagConditionCommandParameters.TaggedObject;
         _taggedObjectId = _taggedObject.Id;
         _descriptorCriteriaRetriever = tagConditionCommandParameters.DescriptorCriteriaRetriever;
         _descriptorCriteriaCreator = tagConditionCommandParameters.DescriptorCriteriaCreator;
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _taggedObject = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _taggedObject = context.Get<T>(_taggedObjectId);
      }

      protected TagConditionCommandParameters<T> CreateCommandParameters() => new TagConditionCommandParameters<T>
      {
         TaggedObject = _taggedObject,
         BuildingBlock = _buildingBlock,
         DescriptorCriteriaCreator = _descriptorCriteriaCreator,
         DescriptorCriteriaRetriever = _descriptorCriteriaRetriever,
      };
   }
}