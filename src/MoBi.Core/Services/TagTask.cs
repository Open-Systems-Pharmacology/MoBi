using System;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Services
{
   public enum TagType
   {
      Match,
      NotMatch,
      MatchAll
   }

   public interface ITagTask
   {
      /// <summary>
      /// Removes the tag <paramref name="tag"/> of type <paramref name="tagType"/> defined in the <paramref name="taggedObject"/> belonging to the <paramref name="buildingBlock"/>. 
      /// The <paramref name="descriptorCriteriaRetriever"/> is used to know how to retrieve the list of tags to remove from.
      /// </summary>
      IMoBiCommand RemoveTagCondition<T>(string tag, TagType tagType, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase;

      /// <summary>
      /// Adds a tag <paramref name="tag"/> of type <paramref name="tagType"/> defined in the <paramref name="taggedObject"/> belonging to the <paramref name="buildingBlock"/>. 
      /// The <paramref name="descriptorCriteriaRetriever"/> is used to know how to retrieve the list of tags to add to.
      /// </summary>
      IMoBiCommand AddTagCondition<T>(string tag, TagType tagType, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase;

      /// <summary>
      /// Edits the tag <paramref name="oldTag"/> defined in the <paramref name="taggedObject"/> belonging to the <paramref name="buildingBlock"/> and sets its new value to <paramref name="newTag"/>.
      /// The <paramref name="descriptorCriteriaRetriever"/> is used to know how to retrieve the actual tag to edit.
      /// </summary>
      IMoBiCommand EditTag<T>(string newTag, string oldTag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase;
   }

   public class TagTask : ITagTask
   {
      private readonly IMoBiContext _context;

      public TagTask(IMoBiContext context)
      {
         _context = context;
      }

      public IMoBiCommand RemoveTagCondition<T>(string tag, TagType tagType, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         return getRemoveCommand(tag, tagType, taggedObject, buildingBlock, descriptorCriteriaRetriever).Run(_context);
      }

      public IMoBiCommand AddTagCondition<T>(string tag, TagType tagType, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         return getAddCommand(tag, tagType, taggedObject, buildingBlock, descriptorCriteriaRetriever).Run(_context);
      }

      public IMoBiCommand EditTag<T>(string newTag, string oldTag, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         return new EditTagCommand<T>(newTag, oldTag, taggedObject, buildingBlock, descriptorCriteriaRetriever).Run(_context);
      }

      private IMoBiCommand getAddCommand<T>(string tag, TagType tagType, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         switch (tagType)
         {
            case TagType.Match:
               return new AddMatchTagConditionCommandBase<T>(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever);
            case TagType.NotMatch:
               return new AddNotMatchTagConditionCommandBase<T>(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever);
            case TagType.MatchAll:
               return new AddMatchAllConditionCommandBase<T>(taggedObject, buildingBlock, descriptorCriteriaRetriever);
            default:
               throw new ArgumentOutOfRangeException("tagType");
         }
      }

      private IMoBiCommand getRemoveCommand<T>(string tag, TagType tagType, T taggedObject, IBuildingBlock buildingBlock, Func<T, DescriptorCriteria> descriptorCriteriaRetriever) where T : class, IObjectBase
      {
         switch (tagType)
         {
            case TagType.Match:
               return new RemoveMatchTagConditionCommandBase<T>(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever);
            case TagType.NotMatch:
               return new RemoveNotMatchTagConditionCommandBase<T>(tag, taggedObject, buildingBlock, descriptorCriteriaRetriever);
            case TagType.MatchAll:
               return new RemoveMatchAllConditionCommandBase<T>(taggedObject, buildingBlock, descriptorCriteriaRetriever);
            default:
               throw new ArgumentOutOfRangeException("tagType");
         } 
      }
   }
}