using System;
using System.Collections.Generic;
using OSPSuite.Core.Commands.Core;
using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using Event = libsbmlcs.Event;
using EventAssignment = libsbmlcs.EventAssignment;
using Model = libsbmlcs.Model;

namespace MoBi.Core.SBML
{
   public class EventImporter : SBMLImporter
   {
      internal EventGroupBuildingBlock EventGroupBuildingBlock;
      private readonly List<IEventAssignment> _eventAssignmentList;

      public List<IEventAssignment> EventAssignmentList
      {
         get { return _eventAssignmentList; }
      }

      internal EventBuilder EventBuilder;
      internal EventGroupBuilder EventGroupBuilder;
      private int _counter;

      public EventImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context) : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _eventAssignmentList = new List<IEventAssignment>();
         _counter = 0;
      }

      /// <summary>
      ///    Imports all Events of the SBML Model.
      /// </summary>
      protected override void Import(Model model)
      {
         _astHandler.NeedAbsolutePath = true;
         CreateEGBandEGBB();

         for (long i = 0; i < model.getNumEvents(); i++)
         {
            CreateEvent(model.getEvent(i));
         }
         _astHandler.NeedAbsolutePath = false;
         AddToProject();
      }

      /// <summary>
      ///    Creates a EventGroupBuildingBlock and a EventGroupBuilder.
      /// </summary>
      internal void CreateEGBandEGBB()
      {
         EventGroupBuilder = new EventGroupBuilder()
            .WithName(SBMLConstants.SBML_EVENTS)
            .WithId(SBMLConstants.SBML_EVENTS);

         var eventsContainer = GetEventsTopContainer();
         if (eventsContainer != null)
         {
            var matchTag = new MatchTagCondition(eventsContainer.Name);
            EventGroupBuilder.SourceCriteria.Add(matchTag);
         }

         EventGroupBuildingBlock = new EventGroupBuildingBlock()
            .WithName(SBMLConstants.SBML_EVENT_BB)
            .WithId(SBMLConstants.SBML_EVENT_BB);
         EventGroupBuildingBlock.Add(EventGroupBuilder);
      }

      /// <summary>
      ///    Creates a MoBi Event by the given SBML Event.
      /// </summary>
      internal void CreateEvent(Event sbmlEvent)
      {
         EventBuilder = new EventBuilder()
            .WithId(sbmlEvent.getId() + SBMLConstants.SBML_EVENTBUILDER)
            .WithName(sbmlEvent.getId())
            .WithDescription(sbmlEvent.getNotesString());

         if (sbmlEvent.isSetTrigger()) CreateCondition(sbmlEvent.getTrigger());
         CreateEventAssignments(sbmlEvent);
         CreateWarningsForUnsupportedFeatures(sbmlEvent);

         EventGroupBuilder.Add(EventBuilder);
      }

      /// <summary>
      ///    Creates warnings to inform the user, that "Delay" and "Priority" of events are not supported/considered.
      /// </summary>
      private void CreateWarningsForUnsupportedFeatures(Event sbmlEvent)
      {
         if (sbmlEvent.isSetDelay())
         {
            var msg = new NotificationMessage(_sbmlProject, MessageOrigin.All, null, NotificationType.Warning)
            {
               Message = SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": Delay of Events is not considered."
            };
            _sbmlInformation.NotificationMessages.Add(msg);
         }

         if (!sbmlEvent.isSetPriority()) return;
         var msg2 = new NotificationMessage(_sbmlProject, MessageOrigin.All, null, NotificationType.Warning)
         {
            Message = SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": Priority of Events is not considered."
         };
         _sbmlInformation.NotificationMessages.Add(msg2);
      }

      /// <summary>
      ///    Creates the Condition of the MoBi Event by the given SBML condition (= trigger).
      /// </summary>
      internal void CreateCondition(Trigger trigger)
      {
         var formula = _astHandler.Parse(trigger.getMath(), trigger.getId(), _sbmlProject, _sbmlInformation) ??
                       ObjectBaseFactory.Create<ExplicitFormula>().WithFormulaString(String.Empty).WithName(SBMLConstants.DEFAULT_FORMULA_NAME);
         EventBuilder.Formula = formula;
      }

      /// <summary>
      ///    Creates the MoBi Event Assignemnts by the given Event Assignments by the
      ///    given SBML Event.
      /// </summary>
      private void CreateEventAssignments(Event sbmlEvent)
      {
         if (sbmlEvent.getNumEventAssignments() <= 0) return;
         for (long i = 0; i < sbmlEvent.getNumEventAssignments(); i++)
         {
            CreateEventAssignment(sbmlEvent.getEventAssignment(i));
         }
      }

      /// <summary>
      ///    Creates the Mobi Event Assignment for one SBML Event Assignment.
      /// </summary>
      internal void CreateEventAssignment(EventAssignment eventAssignment)
      {
         _counter++;
         var assignmentVar = eventAssignment.getVariable();
         var alias = _sbmlInformation.AliasCreator.CreateAliasFrom(assignmentVar) + _counter;

         var description = String.Empty;
         if (eventAssignment.isSetNotes()) description += eventAssignment.getNotesString();
         if (eventAssignment.isSetSBOTerm()) description += (SBMLConstants.SPACE + eventAssignment.getSBOTerm());

         IEventAssignmentBuilder eab = new EventAssignmentBuilder()
            .WithId(eventAssignment.getId() + SBMLConstants.SPACE + eventAssignment.getName() + SBMLConstants.SPACE + alias)
            .WithName(SBMLConstants.SBML_EVENT_ASSIGNMENT + eventAssignment.getId())
            .WithDescription(description);

         var formula = _astHandler.Parse(eventAssignment.getMath(), eab, assignmentVar, _sbmlProject, _sbmlInformation) ??
                       ObjectBaseFactory.Create<ExplicitFormula>().WithFormulaString(String.Empty).WithName(SBMLConstants.DEFAULT_FORMULA_NAME);
         eab.Formula = formula;
         EventGroupBuildingBlock.FormulaCache.Add(formula);
         EventBuilder.AddAssignment(eab);
      }

      /// <summary>
      ///    Adds the EventGroupBuildingBlock to the MoBi Project.
      /// </summary>
      public override void AddToProject()
      {
         _context.HistoryManager.AddCommand(new AddBuildingBlockCommand<IEventGroupBuildingBlock>(EventGroupBuildingBlock).Run(_context));
      }
   }
}