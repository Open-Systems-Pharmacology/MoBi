using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddPKSimMoleculeCommand : ActiveObjectUICommand<IMoleculeBuildingBlock>
   {
      private readonly IInteractionTasksForMoleculeBuilder _interactionTasksForMoleculeBuilder;

      public AddPKSimMoleculeCommand(IInteractionTasksForMoleculeBuilder interactionTasksForMoleculeBuilder, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _interactionTasksForMoleculeBuilder = interactionTasksForMoleculeBuilder;
      }

      protected override void PerformExecute()
      {
         _interactionTasksForMoleculeBuilder.AddPKSimMoleculeTo(Subject);
      }
   }
}