using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddStartValueToBuildingBlockCommand<T> : BuildingBlockChangeCommandBase<PathAndValueEntityBuildingBlock<T>> where T : PathAndValueEntity
   {
      private T _startValue;
      private readonly ObjectPath _objectPath;
      private byte[] _serializedStartValue;

      public AddStartValueToBuildingBlockCommand(PathAndValueEntityBuildingBlock<T> startValuesBuildingBlock, T startValue)
         : base(startValuesBuildingBlock)
      {
         _startValue = startValue;
         CommandType = AppConstants.Commands.AddCommand;
         Description = AppConstants.Commands.AddedStartValue(startValue, startValuesBuildingBlock.Name);
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         _objectPath = startValue.Path;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _startValue = context.Deserialize<T>(_serializedStartValue);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _startValue = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _serializedStartValue = context.Serialize(_startValue);
         _buildingBlock.Add(_startValue);
         if(_startValue.Formula != null)
            _buildingBlock.AddFormula(_startValue.Formula);

         context.PublishEvent(new StartValuesBuildingBlockChangedEvent(_buildingBlock));
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveStartValueFromBuildingBlockCommand<T>(_buildingBlock, _objectPath)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }

   public class AddParameterValueToBuildingBlockCommand : AddStartValueToBuildingBlockCommand<ParameterValue>
   {
      public AddParameterValueToBuildingBlockCommand(PathAndValueEntityBuildingBlock<ParameterValue> parameterValuesBuildingBlock, ParameterValue startValue) : base(parameterValuesBuildingBlock, startValue)
      {
      }
   }

   public class AddInitialConditionToBuildingBlockCommand : AddStartValueToBuildingBlockCommand<InitialCondition>
   {
      public AddInitialConditionToBuildingBlockCommand(PathAndValueEntityBuildingBlock<InitialCondition> initialConditionsBuildingBlock, InitialCondition startValue)
         : base(initialConditionsBuildingBlock, startValue)
      {
      }
   }
}