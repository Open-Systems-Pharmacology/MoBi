using System;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
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
         var descriptorCriteria = _descriptorCriteriaRetriever(_taggedObject);
         var newTagCondition = CreateNewTagCondition();
         descriptorCriteria.Add(newTagCondition);
         context.PublishEvent(new AddTagConditionEvent(_taggedObject));
         Description = AppConstants.Commands.AddToConditionDescription(ObjectType, _tag, _taggedObject.Name);
      }
   }

   public class AddMatchAllConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddMatchAllConditionCommand(T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
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
         return new RemoveMatchAllConditionCommand<T>(_taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }

   public class AddMatchTagConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddMatchTagConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
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
         return new RemoveMatchTagConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }

   public class AddNotMatchTagConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddNotMatchTagConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
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
         return new RemoveNotMatchTagConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }

   public class AddInContainerConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddInContainerConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
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
         return new RemoveInContainerConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }

   public class AddNotInContainerConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddNotInContainerConditionCommand(string tag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever)
         : base(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever)
      {
         ObjectType = AppConstants.Commands.NotInContainerCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new NotInContainerCondition(_tag);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveNotInContainerConditionCommand<T>(_tag, _taggedObject, _buildingBlock, _descriptorCriteriaRetriever).AsInverseFor(this);
      }
   }
}