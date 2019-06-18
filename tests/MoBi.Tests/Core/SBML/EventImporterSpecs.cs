using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using FakeItEasy;
using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Engine.Sbml;
using MoBi.IntegrationTests;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using EventAssignment = libsbmlcs.EventAssignment;
using Model = libsbmlcs.Model;

namespace MoBi.Core.SBML
{
   public abstract class ConcernForEventImporter : ContextForIntegration<EventImporter>
   {
      protected override void Context()
      {
         base.Context();
         var context = IoC.Resolve<IMoBiContext>();
         context.NewProject();
      }
   }

   public class EventImporterTests : ConcernForEventImporter
   {
      protected override void Because()
      {
         var sbmlModel = new Model(3, 1);

         //Event
         var sbmlEvent = sbmlModel.createEvent();
         sbmlEvent.setId("e1");
         sbmlEvent.setName("e1_name");
         sbmlEvent.setNotes("eventNotes");

         //Trigger
         var trigger = sbmlModel.createTrigger();
         trigger.setMath(libsbml.parseFormula("1 > 0 "));
         sbmlEvent.setTrigger(trigger);
         //Event Assignment
         var assign = new EventAssignment(3, 1);
         assign.setId("ea1");
         assign.setName("ea1_name");
         assign.setVariable("x1");
         assign.setMath(libsbml.parseFormula("1+2"));
         sbmlEvent.addEventAssignment(assign);

         sbmlModel.addEvent(sbmlEvent);

         sut.DoImport(sbmlModel, new MoBiProject(), A.Fake<SBMLInformation>(), new MoBiMacroCommand());
      }

      [Observation]
      public void EGBAndEGBBCreationTest()
      {
         sut.EventGroupBuilder.ShouldNotBeNull();
         sut.EventGroupBuildingBlock.ShouldNotBeNull();
      }

      [Observation]
      public void EventCreationTest()
      {
         sut.EventGroupBuilder.Children.ShouldNotBeNull();
         sut.EventGroupBuilder.Children.ExistsByName("e1").ShouldBeTrue();
         var event1 = ObjectBaseExtensions.FindByName(sut.EventGroupBuilder.Children, "e1");
         event1.Id.ShouldBeEqualTo("e1" + SBMLConstants.SBML_EVENTBUILDER);
         event1.Name.ShouldBeEqualTo("e1");
      }

      [Observation]
      public void TriggerCreationTest()
      {
         sut.EventGroupBuilder.Children.ExistsByName("e1").ShouldBeTrue();
         var event1 = (EventBuilder) sut.EventGroupBuilder.Children.FindByName("e1");
         event1.Formula.ShouldNotBeNull();
      }

      [Observation]
      public void AssignmentCreationTest()
      {
         sut.EventBuilder.Assignments.ShouldNotBeNull();
         sut.EventBuilder.Assignments.ExistsByName(SBMLConstants.SBML_EVENT_ASSIGNMENT + "x1")
            .ShouldBeTrue();
         var assignment = sut.EventBuilder.Assignments.FindByName(SBMLConstants.SBML_EVENT_ASSIGNMENT + "x1");
         assignment.Formula.ShouldNotBeNull();
      }
   }
}