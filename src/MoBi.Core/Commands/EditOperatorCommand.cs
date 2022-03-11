using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace MoBi.Core.Commands
{
   public class EditOperatorCommand<T> : TagConditionCommandBase<T> where T : class, IObjectBase
   {
      private readonly CriteriaOperator _newOperator;
      private CriteriaOperator _oldOperator;

      public EditOperatorCommand(CriteriaOperator newOperator, TagConditionCommandParameters<T> tagConditionCommandParameters) :
         base(string.Empty, tagConditionCommandParameters)
      {
         CommandType = AppConstants.Commands.EditCommand;
         ObjectType = ObjectTypes.TagCondition;
         _newOperator = newOperator;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var criteria = getCriteria();
         _oldOperator = criteria.Operator;
         criteria.Operator = _newOperator;
         Description = AppConstants.Commands.EditOperatorDescription(ObjectType, _oldOperator.ToString(), _newOperator.ToString(), _taggedObject.Name);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditOperatorCommand<T>(_oldOperator, CreateCommandParameters()).AsInverseFor(this);
      }

      private DescriptorCriteria getCriteria()
      {
         return _descriptorCriteriaRetriever(_taggedObject) ?? _descriptorCriteriaCreator(_taggedObject);
      }
   }
}