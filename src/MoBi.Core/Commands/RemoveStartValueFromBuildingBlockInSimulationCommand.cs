using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveStartValueFromBuildingBlockInSimulationCommand<T> : StartValueBuildingBlockInSimulationCommandBase<T> where T : class, IStartValue
   {
      private readonly IObjectPath _objectPath;
      private T _startValue;

      public RemoveStartValueFromBuildingBlockInSimulationCommand(IObjectPath objectPath, IStartValuesBuildingBlock<T> startValuesBuildingBlock) : base(startValuesBuildingBlock)
      {
         _objectPath = objectPath;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_objectPath == null)
            return;

         _startValue = _startValuesBuildingBlock[_objectPath];
         if (_startValue == null)
            return;

         _startValuesBuildingBlock.Remove(_startValue);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddStartValueToBuildingBlockCommand<T>(_startValuesBuildingBlock, _startValue)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }
}