using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddStartValueToBuildingBlockInSimulationCommand<T> : StartValueBuildingBlockInSimulationCommandBase<T> where T : class, IStartValue
   {
      private readonly T _startValue;
      private IObjectPath _objectPath;

      public AddStartValueToBuildingBlockInSimulationCommand(T startValue, IStartValuesBuildingBlock<T> startValuesBuildingBlock)
         : base(startValuesBuildingBlock)
      {
         _startValue = startValue;
         _objectPath = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         if (_startValue == null)
            return;

         _objectPath = _startValue.Path;
         if (_startValuesBuildingBlock[_objectPath] != null)
            return;

         _startValuesBuildingBlock.Add(_startValue);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveStartValueFromBuildingBlockInSimulationCommand<T>(_objectPath, _startValuesBuildingBlock)
         {
            Visible = Visible
         }.AsInverseFor(this);
      }
   }
}