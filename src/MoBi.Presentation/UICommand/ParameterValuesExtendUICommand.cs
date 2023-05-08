using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class ParameterValuesExtendUICommand : AbstractStartValueSubjectRetrieverUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      public ParameterValuesExtendUICommand(IParameterValuesTask valueTasks, IActiveSubjectRetriever activeSubjectRetriever)
         : base(valueTasks, activeSubjectRetriever)
      {
      }

      protected override void PerformExecute()
      {
         _startValueTasks.ExtendStartValues(Subject);
      }
   }
}