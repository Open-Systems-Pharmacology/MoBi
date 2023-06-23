using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class InitialConditionsExtendUICommand : AbstractStartValueSubjectRetrieverUICommand<InitialConditionsBuildingBlock, InitialCondition>
   {
      public InitialConditionsExtendUICommand(IInitialConditionsTask<InitialConditionsBuildingBlock> startValueTasks, IActiveSubjectRetriever activeSubjectRetriever)
         : base(startValueTasks, activeSubjectRetriever)
      {
      }

      protected override void PerformExecute()
      {
         //TODO OSMOSES
         // _startValueTasks.ExtendStartValueBuildingBlock(Subject);
      }
   }
}