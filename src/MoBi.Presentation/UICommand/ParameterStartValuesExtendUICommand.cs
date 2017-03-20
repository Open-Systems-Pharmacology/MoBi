using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class ParameterStartValuesExtendUICommand : AbstractStartValueSubjectRetrieverUICommand<IParameterStartValuesBuildingBlock, IParameterStartValue>
   {
      public ParameterStartValuesExtendUICommand(IParameterStartValuesTask startValueTasks, IActiveSubjectRetriever activeSubjectRetriever)
         : base(startValueTasks, activeSubjectRetriever)
      {
      }

      protected override void PerformExecute()
      {
         _startValueTasks.ExtendStartValues(Subject);
      }
   }
}