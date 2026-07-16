using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands;

public class AddConditionGroupCommand<T> : TagConditionCommandBase<T> where T : class, IObjectBase
{
   private ConditionGroup _conditionGroup;
   private byte[] _serializedConditionGroup;

   public AddConditionGroupCommand(ConditionGroup conditionGroup, TagConditionCommandParameters<T> tagConditionCommandParameters)
      : base(string.Empty, tagConditionCommandParameters)
   {
      _conditionGroup = conditionGroup;
      CommandType = AppConstants.Commands.AddCommand;
      ObjectType = AppConstants.Commands.ConditionGroup;
   }

   protected override void ExecuteWith(IMoBiContext context)
   {
      base.ExecuteWith(context);
      var descriptorCriteria = _descriptorCriteriaRetriever(_taggedObject) ?? _descriptorCriteriaCreator(_taggedObject);
      descriptorCriteria.Add(_conditionGroup);
      _serializedConditionGroup = context.Serialize(_conditionGroup);
      context.PublishEvent(new AddTagConditionEvent(_taggedObject));
      Description = AppConstants.Commands.AddToConditionDescription(ObjectType, _conditionGroup.Condition, _taggedObject.Name);
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
      return new RemoveConditionGroupCommand<T>(_conditionGroup, CreateCommandParameters()).AsInverseFor(this);
   }
}
