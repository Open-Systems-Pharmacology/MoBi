using System;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public abstract class AddTagConditionCommandBase<T> : TagConditionCommandBase<T> where T : class, IObjectBase
   {
      protected AddTagConditionCommandBase(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         CommandType = AppConstants.Commands.AddCommand;
      }

      protected abstract ITagCondition CreateNewTagCondition();

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var descriptorCrieria = _descriptorCriteriaRetriever(_taggedObject);
         var newTagCondition = CreateNewTagCondition();
         descriptorCrieria.Add(newTagCondition);
         context.PublishEvent(new AddTagConditionEvent(_taggedObject));
         Description = AppConstants.Commands.AddToConditionDescription(ObjectType, _tag, _taggedObject.Name);
      }
   }

   public class AddMatchAllConditionCommandBase<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddMatchAllConditionCommandBase(T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(string.Empty, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.MatchAllCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new MatchAllCondition();
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMatchAllConditionCommandBase<T>(_taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }

   public class AddMatchTagConditionCommandBase<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddMatchTagConditionCommandBase(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.MatchTagCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new MatchTagCondition(_tag);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMatchTagConditionCommandBase<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }
    
   public class AddNotMatchTagConditionCommandBase<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddNotMatchTagConditionCommandBase(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.NotMatchTagCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new NotMatchTagCondition(_tag);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveNotMatchTagConditionCommandBase<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }

   public class AddInContainerConditionCommandBase<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddInContainerConditionCommandBase(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.InContainerCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new InContainerCondition(_tag);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveInContainerConditionCommandBase<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }
}