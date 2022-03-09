using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class EditTagCommand<T> : TagConditionCommandBase<T> where T : class, IObjectBase
   {
      private readonly string _oldTag;
      private readonly string _newTag;

      public EditTagCommand(string newTag, string oldTag, TagConditionCommandParameters<T> tagConditionCommandParameters) :
         base(oldTag, tagConditionCommandParameters)
      {
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.TagCondition;
         _oldTag = oldTag;
         _newTag = newTag;
         Description = AppConstants.Commands.EditTagDescription(ObjectType, _oldTag, _newTag, _taggedObject.Name);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         editTagInCriteria(getCriteria());
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditTagCommand<T>(_oldTag, _newTag, CreateCommandParameters()).AsInverseFor(this);
      }

      private DescriptorCriteria getCriteria()
      {
         return _descriptorCriteriaRetriever(_taggedObject);
      }

      private void editTagInCriteria(DescriptorCriteria criteria)
      {
         var tagCondition = criteria.OfType<ITagCondition>().Find(x => string.Equals(x.Tag, _oldTag));
         if (tagCondition == null) return;
         tagCondition.Replace(_oldTag, _newTag);
      }
   }
}