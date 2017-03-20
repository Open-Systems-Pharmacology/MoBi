using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public abstract class EditParameterPropertyInBuildingBlockCommand<T> : BuildingBlockChangeCommandBase<IBuildingBlock>
   {
      protected T _newValue;
      protected T _oldValue;
      protected IParameter _parameter;
      private readonly string _parameterId;

      protected EditParameterPropertyInBuildingBlockCommand(T newValue, T oldValue, IParameter parameter, IBuildingBlock buildingBlock) 
         : base(buildingBlock)
      {
         _oldValue = oldValue;
         _newValue = newValue;
         _parameterId = parameter.Id;
         _parameter = parameter;

         ObjectType = ObjectTypes.Parameter;
         CommandType = AppConstants.Commands.EditCommand;
         Description = GetCommandDescription(newValue, oldValue, parameter, buildingBlock);
      }

      protected abstract string GetCommandDescription(T newValue, T oldValue, IParameter parameter, IBuildingBlock buildingBlock);

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new ParameterChangedEvent(_parameter));
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _parameter = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _parameter = context.Get<IParameter>(_parameterId);
      }
   }
}