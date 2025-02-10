using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class AddProteinExpressionUICommand : AbstractStartValueSubjectRetrieverUICommand<ParameterValuesBuildingBlock, ParameterValue>
   {
      private readonly IParameterValuesTask _parameterValueTasks;

      public AddProteinExpressionUICommand(IParameterValuesTask valueTasks, IActiveSubjectRetriever activeSubjectRetriever)
         : base(valueTasks, activeSubjectRetriever)
      {
         _parameterValueTasks = valueTasks;
      }

      protected override void PerformExecute() => _parameterValueTasks.AddStartValueExpression(Subject);
   }
}