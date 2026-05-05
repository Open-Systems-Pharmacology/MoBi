using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands;

public class RemoveConditionGroupCommand<T> : TagConditionCommandBase<T> where T : class, IObjectBase
{
   private ConditionGroup _conditionGroup;
   private byte[] _serializedConditionGroup;

   public RemoveConditionGroupCommand(ConditionGroup conditionGroup, TagConditionCommandParameters<T> tagConditionCommandParameters)
      : base(string.Empty, tagConditionCommandParameters)
   {
      _conditionGroup = conditionGroup;
      CommandType = AppConstants.Commands.DeleteCommand;
      ObjectType = AppConstants.Commands.ConditionGroup;
   }

   protected override void ExecuteWith(IMoBiContext context)
   {
      base.ExecuteWith(context);
      _serializedConditionGroup = context.Serialize(_conditionGroup);

      var descriptorCriteria = _descriptorCriteriaRetriever(_taggedObject);

      descriptorCriteria.Remove(_conditionGroup);

      context.PublishEvent(new RemoveTagConditionEvent(_taggedObject));
      Description = AppConstants.Commands.RemoveTagFromConditionDescription(ObjectType, _conditionGroup.Condition, _taggedObject.Name);
   }

   protected override void ClearReferences()
   {
      base.ClearReferences();
      _conditionGroup = null;
   }

   public override void RestoreExecutionData(IMoBiContext context)
   {
      base.RestoreExecutionData(context);
      _conditionGroup = context.Deserialize<ConditionGroup>(_serializedConditionGroup);
   }

   protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
   {
      return new AddConditionGroupCommand<T>(_conditionGroup, CreateCommandParameters()).AsInverseFor(this);
   }
}
