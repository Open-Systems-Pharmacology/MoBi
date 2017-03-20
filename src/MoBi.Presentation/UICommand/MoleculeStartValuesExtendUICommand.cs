using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.UICommand
{
   internal class MoleculeStartValuesExtendUICommand : AbstractStartValueSubjectRetrieverUICommand<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>
   {
      public MoleculeStartValuesExtendUICommand(IMoleculeStartValuesTask startValueTasks, IActiveSubjectRetriever activeSubjectRetriever)
         : base(startValueTasks, activeSubjectRetriever)
      {
      }

      protected override void PerformExecute()
      {
         _startValueTasks.ExtendStartValues(Subject);
      }
   }
}