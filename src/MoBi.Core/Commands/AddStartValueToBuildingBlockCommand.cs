using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddStartValueToBuildingBlockCommand<T> : BuildingBlockChangeCommandBase<IStartValuesBuildingBlock<T>> where T : class, IStartValue
   {
      private T _startValue;
      private readonly ObjectPath _objectPath;
      private byte[] _serializedParameterStartValue;

      public AddStartValueToBuildingBlockCommand(IStartValuesBuildingBlock<T> startValuesBuildingBlock, T startValue)
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
         _startValue = context.Deserialize<T>(_serializedParameterStartValue);
      }

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _startValue = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _serializedParameterStartValue = context.Serialize(_startValue);
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

   public class AddParameterStartValueToBuildingBlockCommand : AddStartValueToBuildingBlockCommand<ParameterValue>
   {
      public AddParameterStartValueToBuildingBlockCommand(IStartValuesBuildingBlock<ParameterValue> parameterStartValuesBuildingBlock, ParameterValue startValue) : base(parameterStartValuesBuildingBlock, startValue)
      {
      }
   }

   public class AddMoleculeStartValueToBuildingBlockCommand : AddStartValueToBuildingBlockCommand<InitialCondition>
   {
      public AddMoleculeStartValueToBuildingBlockCommand(IStartValuesBuildingBlock<InitialCondition> moleculeStartValuesBuildingBlock, InitialCondition startValue)
         : base(moleculeStartValuesBuildingBlock, startValue)
      {
      }
   }
}