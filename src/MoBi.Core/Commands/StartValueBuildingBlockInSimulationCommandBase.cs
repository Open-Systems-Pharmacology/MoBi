using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class StartValueBuildingBlockInSimulationCommandBase<T> : MoBiReversibleCommand where T : class, IStartValue
   {
      protected IStartValuesBuildingBlock<T> _startValuesBuildingBlock;
      private readonly string _startValuesBuildingBlockId;

      protected StartValueBuildingBlockInSimulationCommandBase(IStartValuesBuildingBlock<T> startValuesBuildingBlock)
      {
         _startValuesBuildingBlock = startValuesBuildingBlock;
         _startValuesBuildingBlockId = startValuesBuildingBlock.Id;
      }

      protected override void ClearReferences()
      {
         _startValuesBuildingBlock = null;
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _startValuesBuildingBlock = context.Get<IStartValuesBuildingBlock<T>>(_startValuesBuildingBlockId);
      }
   }
}