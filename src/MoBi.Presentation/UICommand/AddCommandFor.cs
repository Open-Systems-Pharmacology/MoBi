using System;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddNewCommandFor<TParent, TChild> : ObjectUICommand<TParent> where TParent : class where TChild : class
   {
      protected readonly IInteractionTasksForChildren<TParent, TChild> _interactionTasks;
      private readonly IMoBiContext _context;
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public AddNewCommandFor(IInteractionTasksForChildren<TParent, TChild> interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _interactionTasks = interactionTasks;
         _activeSubjectRetriever = activeSubjectRetriever;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var buildingBlockWithFormulaCache = _activeSubjectRetriever.Active<IBuildingBlock>();
         if (Subject != null)
            _context.AddToHistory(_interactionTasks.AddNew(Subject, buildingBlockWithFormulaCache));
         else
            _context.AddToHistory(_interactionTasks.AddNew((TParent) buildingBlockWithFormulaCache, buildingBlockWithFormulaCache));
      }
   }

   public class AddExistingCommandFor<TParent, TChild> : AddExistingCommandForBase<TParent> where TParent : class where TChild : class
   {
      public AddExistingCommandFor(IInteractionTasksForChildren<TParent, TChild> interactionTasks, IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context)
         : base(activeSubjectRetriever, context, interactionTasks.AddExisting)
      {
      }
   }

   public class AddExistingFromTemplateCommandFor<TParent, TChild> : AddExistingCommandForBase<TParent> where TParent : class where TChild : class
   {
      public AddExistingFromTemplateCommandFor(IInteractionTasksForChildren<TParent, TChild> interactionTasks, IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context)
         : base(activeSubjectRetriever, context, interactionTasks.AddExistingTemplate)
      {
      }
   }

   public abstract class AddExistingCommandForBase<TParent> : ObjectUICommand<TParent> where TParent : class
   {
      protected readonly IActiveSubjectRetriever _activeSubjectRetriever;
      private readonly IMoBiContext _context;
      private readonly Func<TParent, IBuildingBlock, IMoBiCommand> _addExistingFunc;

      protected AddExistingCommandForBase(IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context, Func<TParent, IBuildingBlock, IMoBiCommand> addExistingFunc)
      {
         _activeSubjectRetriever = activeSubjectRetriever;
         _context = context;
         _addExistingFunc = addExistingFunc;
      }

      protected override void PerformExecute()
      {
         var buildingBlockWithFormulaCache = _activeSubjectRetriever.Active<IBuildingBlock>();
         if (Subject != null)
            _context.AddToHistory(_addExistingFunc(Subject, buildingBlockWithFormulaCache));
         else
            _context.AddToHistory(_addExistingFunc((TParent) buildingBlockWithFormulaCache, buildingBlockWithFormulaCache));
      }
   }

   public class AddNewTopContainerCommand : AddNewCommandFor<MoBiSpatialStructure, IContainer>
   {
      public AddNewTopContainerCommand(IInteractionTasksForTopContainer interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
         : base(interactionTasks, context, activeSubjectRetriever)
      {
      }

      protected override void PerformExecute()
      {
         Subject = _activeSubjectRetriever.Active<MoBiSpatialStructure>();
         base.PerformExecute();
      }
   }

   public class AddNewNeighborhoodCommand : AddNewCommandFor<IContainer, NeighborhoodBuilder>
   {
      public AddNewNeighborhoodCommand(IInteractionTasksForNeighborhood interactionTasks, IMoBiContext context, IActiveSubjectRetriever activeSubjectRetriever)
         : base(interactionTasks, context, activeSubjectRetriever)
      {
      }

      protected override void PerformExecute()
      {
         Subject = _activeSubjectRetriever.Active<MoBiSpatialStructure>().NeighborhoodsContainer;
         base.PerformExecute();
      }
   }

   public class AddExistingTopContainerCommand : AddExistingCommandFor<MoBiSpatialStructure, IContainer>
   {
      public AddExistingTopContainerCommand(IInteractionTasksForChildren<MoBiSpatialStructure, IContainer> interactionTasks, IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context) : base(interactionTasks, activeSubjectRetriever, context)
      {
      }

      protected override void PerformExecute()
      {
         Subject = _activeSubjectRetriever.Active<MoBiSpatialStructure>();
         base.PerformExecute();
      }
   }

   public class AddExistingFromTemplateTopContainerCommand : AddExistingFromTemplateCommandFor<MoBiSpatialStructure, IContainer>
   {
      public AddExistingFromTemplateTopContainerCommand(IInteractionTasksForChildren<MoBiSpatialStructure, IContainer> interactionTasks, IActiveSubjectRetriever activeSubjectRetriever, IMoBiContext context)
         : base(interactionTasks, activeSubjectRetriever, context)
      {
      }

      protected override void PerformExecute()
      {
         Subject = _activeSubjectRetriever.Active<MoBiSpatialStructure>();
         base.PerformExecute();
      }
   }
}