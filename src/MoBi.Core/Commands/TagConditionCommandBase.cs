using System;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public abstract class TagConditionCommandBase<T> : BuildingBlockChangeCommandBase<IBuildingBlock> where T : class, IObjectBase
   {
      protected T _taggedObject;
      protected readonly Func<T,DescriptorCriteria> _descriptorCriteriaRetriever;
      protected string _tag;
      private readonly string _taggedObjectId;

      protected TagConditionCommandBase(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(buildingBlock)
      {
         _tag = tag;
         _taggedObjectId = taggedObject.Id;
         _taggedObject = taggedObject;
         _descriptorCriteriaRetriever = descriptorCriteriaRetriever;
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
   }
}