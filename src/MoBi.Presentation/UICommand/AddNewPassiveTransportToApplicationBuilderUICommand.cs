using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class AddNewPassiveTransportToApplicationBuilderUICommand : ObjectUICommand<ApplicationBuilder>
   {
      private readonly IInteractionTasksForChildren<ApplicationBuilder, TransportBuilder> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public AddNewPassiveTransportToApplicationBuilderUICommand(IInteractionTasksForChildren<ApplicationBuilder, TransportBuilder> interactionTasks,
         IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_interactionTasks.AddNew(Subject, _activeSubjectRetriever.Active<IBuildingBlock>()));
      }
   }

   internal class AddExistingPassiveTransportToApplicationBuilderUICommand : ObjectUICommand<ApplicationBuilder>
   {
      private readonly IInteractionTasksForChildren<ApplicationBuilder, TransportBuilder> _interactionTasks;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly IMoBiContext _context;

      public AddExistingPassiveTransportToApplicationBuilderUICommand(IInteractionTasksForChildren<ApplicationBuilder, TransportBuilder> interactionTasks, IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context)
      {
         _interactionTasks = interactionTasks;
         _activeSubjectRetriever = activeSubjectRetriever;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_interactionTasks.AddExisting(Subject, _activeSubjectRetriever.Active<IBuildingBlock>()));
      }
   }
}