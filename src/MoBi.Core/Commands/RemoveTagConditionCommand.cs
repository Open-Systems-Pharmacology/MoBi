using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class RemoveTagConditionCommandBase<T> : TagConditionCommandBase<T> where T : class, IObjectBase
   {
      protected RemoveTagConditionCommandBase(string tagToRemove, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tagToRemove, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         CommandType = AppConstants.Commands.DeleteCommand;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var descriptorCriteria = _descriptorCriteriaRetriever(_taggedObject);
         RemoveTagCondition(descriptorCriteria);
         context.PublishEvent(new RemoveTagConditionEvent(_taggedObject));
         Description = AppConstants.Commands.RemoveTagFromConditionDescription(ObjectType, _tag, _taggedObject.Name);
      }

      protected abstract void RemoveTagCondition(DescriptorCriteria descriptorCriteria);
   }

   public class RemoveMatchAllConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveMatchAllConditionCommand(T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(string.Empty, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.MatchAllCondition;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMatchAllConditionCommand<T>(_taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         var condition = descriptorCriteria.Find(x => x.IsAnImplementationOf<MatchAllCondition>());
         if (condition == null) return;
         descriptorCriteria.Remove(condition);
      }
   }

   public class RemoveMatchTagConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveMatchTagConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.MatchTagCondition;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMatchTagConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<MatchTagCondition>(_tag);
      }
   }

   public class RemoveNotMatchTagConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveNotMatchTagConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.NotMatchTagCondition;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddNotMatchTagConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<NotMatchTagCondition>(_tag);
      }
   }

   public class RemoveInContainerConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveInContainerConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.InContainerCondition;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddInContainerConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<InContainerCondition>(_tag);
      }
   }


   public class RemoveNotInContainerConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveNotInContainerConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.NotInContainerCondition;
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddNotInContainerConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<NotInContainerCondition>(_tag);
      }
   }
}