using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public abstract class RemoveTagConditionCommandBase<T> : TagConditionCommandBase<T> where T : class, IObjectBase
   {
      protected RemoveTagConditionCommandBase(string tagToRemove, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tagToRemove, tagConditionCommandParameters)
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
      public RemoveMatchAllConditionCommand(TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(string.Empty, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.MatchAllCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMatchAllConditionCommand<T>(CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         var condition = descriptorCriteria.Find(x => x.IsAnImplementationOf<MatchAllCondition>());
         if (condition == null) return;
         descriptorCriteria.Remove(condition);
      }
   }

   public class RemoveInParentConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveInParentConditionCommand(TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(string.Empty, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.InParentCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddInParentConditionCommand<T>(CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         var condition = descriptorCriteria.Find(x => x.IsAnImplementationOf<InParentCondition>());
         if (condition == null) return;
         descriptorCriteria.Remove(condition);
      }
   }

   public class RemoveInChildrenConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveInChildrenConditionCommand(TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(string.Empty, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.InChildrenCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddInChildrenConditionCommand<T>(CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         var condition = descriptorCriteria.Find(x => x.IsAnImplementationOf<InChildrenCondition>());
         if (condition == null) return;
         descriptorCriteria.Remove(condition);
      }
   }

   public class RemoveMatchTagConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveMatchTagConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.MatchTagCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMatchTagConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<MatchTagCondition>(_tag);
      }
   }

   public class RemoveNotMatchTagConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveNotMatchTagConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.NotMatchTagCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddNotMatchTagConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<NotMatchTagCondition>(_tag);
      }
   }

   public class RemoveInContainerConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveInContainerConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.InContainerCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddInContainerConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<InContainerCondition>(_tag);
      }
   }

   public class RemoveNotInContainerConditionCommand<T> : RemoveTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public RemoveNotInContainerConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.NotInContainerCondition;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddNotInContainerConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }

      protected override void RemoveTagCondition(DescriptorCriteria descriptorCriteria)
      {
         descriptorCriteria.RemoveByTag<NotInContainerCondition>(_tag);
      }
   }
}