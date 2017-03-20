using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class AddNewApplicationMoleculeBuilderUICommand : ObjectUICommand<IApplicationBuilder>
   {
      private readonly IInteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder> _interactionTasks;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public AddNewApplicationMoleculeBuilderUICommand(IInteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _activeSubjectRetriever = activeSubjectRetriever;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_interactionTasks.AddNew(Subject, _activeSubjectRetriever.Active<IBuildingBlock>()));
      }
   }

   internal class RemoveApplicationMoleculeBuilderUICommand : IUICommand
   {
      private readonly IInteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder> _interactionTasks;
      private IApplicationMoleculeBuilder _applicationMoleculeBuilder;
      private readonly IMoBiContext _context;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private IApplicationBuilder _parent;

      public RemoveApplicationMoleculeBuilderUICommand(IInteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _context = context;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public void Execute()
      {
         var buildingBlock = _activeSubjectRetriever.Active<IBuildingBlock>();
         _context.AddToHistory(_interactionTasks.Remove(_applicationMoleculeBuilder,_parent, buildingBlock));
      }

      public IUICommand Initialze(IApplicationMoleculeBuilder applicationMoleculeBuilder, IApplicationBuilder parent)
      {
         _applicationMoleculeBuilder = applicationMoleculeBuilder;
        _parent = parent;
         return this;
      }
   }
}