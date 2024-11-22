using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Services
{
   public enum TagType
   {
      Match,
      NotMatch,
      MatchAll,
      InContainer,
      NotInContainer,
      InParent,
      InChildren
   }

   public interface ITagTask
   {
      /// <summary>
      ///    Removes the tag <paramref name="tag" /> of type <paramref name="tagType" />
      /// </summary>
      IMoBiCommand RemoveTagCondition<T>(string tag, TagType tagType, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase;

      /// <summary>
      ///    Adds a tag <paramref name="tag" /> of type <paramref name="tagType" />
      /// </summary>
      IMoBiCommand AddTagCondition<T>(string tag, TagType tagType, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase;

      /// <summary>
      ///    Edits the tag <paramref name="oldTag" />
      /// </summary>
      IMoBiCommand EditTag<T>(string newTag, string oldTag, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase;

      /// <summary>
      ///    Edits the tag <paramref name="newOperator" />
      /// </summary>
      IMoBiCommand EditOperator<T>(CriteriaOperator newOperator, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase;
   }

   public class TagTask : ITagTask
   {
      private readonly IMoBiContext _context;

      public TagTask(IMoBiContext context)
      {
         _context = context;
      }

      public IMoBiCommand RemoveTagCondition<T>(string tag, TagType tagType, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase
      {
         return getRemoveCommand(tag, tagType, tagConditionCommandParameters).RunCommand(_context);
      }

      public IMoBiCommand AddTagCondition<T>(string tag, TagType tagType, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase
      {
         return getAddCommand(tag, tagType, tagConditionCommandParameters).RunCommand(_context);
      }

      public IMoBiCommand EditTag<T>(string newTag, string oldTag, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase
      {
         return new EditTagCommand<T>(newTag, oldTag, tagConditionCommandParameters).RunCommand(_context);
      }

      public IMoBiCommand EditOperator<T>(CriteriaOperator newOperator, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase
      {
         return new EditOperatorCommand<T>(newOperator, tagConditionCommandParameters).RunCommand(_context);
      }

      private IMoBiCommand getAddCommand<T>(string tag, TagType tagType, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase
      {
         switch (tagType)
         {
            case TagType.MatchAll:
               return new AddMatchAllConditionCommand<T>(tagConditionCommandParameters);
            case TagType.Match:
               return new AddMatchTagConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.NotMatch:
               return new AddNotMatchTagConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.InContainer:
               return new AddInContainerConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.NotInContainer:
               return new AddNotInContainerConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.InParent:
               return new AddInParentConditionCommand<T>(tagConditionCommandParameters);
            case TagType.InChildren:
               return new AddInChildrenConditionCommand<T>(tagConditionCommandParameters);
            default:
               throw new ArgumentOutOfRangeException(nameof(tagType));
         }
      }

      private IMoBiCommand getRemoveCommand<T>(string tag, TagType tagType, TagConditionCommandParameters<T> tagConditionCommandParameters) where T : class, IObjectBase
      {
         switch (tagType)
         {
            case TagType.MatchAll:
               return new RemoveMatchAllConditionCommand<T>(tagConditionCommandParameters);
            case TagType.Match:
               return new RemoveMatchTagConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.NotMatch:
               return new RemoveNotMatchTagConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.InContainer:
               return new RemoveInContainerConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.NotInContainer:
               return new RemoveNotInContainerConditionCommand<T>(tag, tagConditionCommandParameters);
            case TagType.InParent:
               return new RemoveInParentConditionCommand<T>(tagConditionCommandParameters);
            case TagType.InChildren:
               return new RemoveInChildrenConditionCommand<T>(tagConditionCommandParameters);
            default:
               throw new ArgumentOutOfRangeException(nameof(tagType));
         }
      }
   }
}