using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public abstract class AddTagConditionCommandBase<T> : TagConditionCommandBase<T> where T : class, IObjectBase
   {
      protected AddTagConditionCommandBase(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         CommandType = AppConstants.Commands.AddCommand;
      }

      protected abstract ITagCondition CreateNewTagCondition();

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var descriptorCriteria = _descriptorCriteriaRetriever(_taggedObject) ?? _descriptorCriteriaCreator(_taggedObject);
         var newTagCondition = CreateNewTagCondition();
         descriptorCriteria.Add(newTagCondition);
         context.PublishEvent(new AddTagConditionEvent(_taggedObject));
         Description = AppConstants.Commands.AddToConditionDescription(ObjectType, _tag, _taggedObject.Name);
      }
   }

   public class AddMatchAllConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddMatchAllConditionCommand(TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(string.Empty, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.MatchAllCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new MatchAllCondition();
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMatchAllConditionCommand<T>(CreateCommandParameters()).AsInverseFor(this);
      }
   }

   public class AddInParentConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddInParentConditionCommand(TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(string.Empty, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.InParentCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new InParentCondition();
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveInParentConditionCommand<T>(CreateCommandParameters()).AsInverseFor(this);
      }
   }

   public class AddInChildrenConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddInChildrenConditionCommand(TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(string.Empty, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.InParentCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new InChildrenCondition();
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveInChildrenConditionCommand<T>(CreateCommandParameters()).AsInverseFor(this);
      }
   }

   public class AddMatchTagConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddMatchTagConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.MatchTagCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new MatchTagCondition(_tag);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMatchTagConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }
   }

   public class AddNotMatchTagConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddNotMatchTagConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.NotMatchTagCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new NotMatchTagCondition(_tag);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveNotMatchTagConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }
   }

   public class AddInContainerConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddInContainerConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.InContainerCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new InContainerCondition(_tag);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveInContainerConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }
   }

   public class AddNotInContainerConditionCommand<T> : AddTagConditionCommandBase<T> where T : class, IObjectBase
   {
      public AddNotInContainerConditionCommand(string tag, TagConditionCommandParameters<T> tagConditionCommandParameters)
         : base(tag, tagConditionCommandParameters)
      {
         ObjectType = AppConstants.Commands.NotInContainerCondition;
      }

      protected override ITagCondition CreateNewTagCondition()
      {
         return new NotInContainerCondition(_tag);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveNotInContainerConditionCommand<T>(_tag, CreateCommandParameters()).AsInverseFor(this);
      }
   }
}