using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   internal class When_the_formula_refers_to_the_assignment_target : ContextSpecification<EventAssignmentBuilderDTO>
   {
      private Parameter _forbiddenObject;
      private EventBuilder _eventBuilder;

      protected override void Context()
      {
         base.Context();
         var eventAssignmentBuilder = new EventAssignmentBuilder
         {
            UseAsValue = false,
            Formula = A.Fake<ExplicitFormula>(),
            ObjectPath = new ObjectPath("..", "..", "Second"),
         };

         A.CallTo(() => eventAssignmentBuilder.Formula.ObjectPaths).Returns(new List<FormulaUsablePath> { new FormulaUsablePath("..", "..", "Second") });
         sut = new EventAssignmentBuilderDTO(eventAssignmentBuilder)
         {
            ChangedEntityPath = "string",
            NewFormula = new FormulaBuilderDTO(eventAssignmentBuilder.Formula)
            {
               ObjectPaths = eventAssignmentBuilder.Formula.ObjectPaths.Select(x => new FormulaUsablePathDTO(x, eventAssignmentBuilder.Formula)).ToList()
            }
         };

         _forbiddenObject = new Parameter().WithName("Second");
         _eventBuilder = new EventBuilder();
         _eventBuilder.AddAssignment(eventAssignmentBuilder);
         new Event
         {
            _eventBuilder,
            _forbiddenObject
         };
      }

      [Observation]
      public void the_dto_is_invalid()
      {
         sut.Validate().Count.ShouldBeEqualTo(3);
      }
   }

   internal class When_the_assignment_target_is_not_resolved : ContextSpecification<EventAssignmentBuilderDTO>
   {
      private Parameter _forbiddenObject;
      private EventBuilder _eventBuilder;

      protected override void Context()
      {
         base.Context();
         var eventAssignmentBuilder = new EventAssignmentBuilder
         {
            UseAsValue = false,
            Formula = A.Fake<ExplicitFormula>(),
            ObjectPath = new ObjectPath("..", "..", "Second")
         };

         A.CallTo(() => eventAssignmentBuilder.Formula.ObjectPaths).Returns(new List<FormulaUsablePath> { new FormulaUsablePath("..", "..", "Second") });
         sut = new EventAssignmentBuilderDTO(eventAssignmentBuilder)
         {
            ChangedEntityPath = "string",
            NewFormula = new FormulaBuilderDTO(eventAssignmentBuilder.Formula)
            {
               ObjectPaths = eventAssignmentBuilder.Formula.ObjectPaths.Select(x => new FormulaUsablePathDTO(x, eventAssignmentBuilder.Formula)).ToList()
            },
            Name = "name"
         };

         _forbiddenObject = new Parameter().WithName("Third");
         _eventBuilder = new EventBuilder();
         _eventBuilder.AddAssignment(eventAssignmentBuilder);
         new Event
         {
            _eventBuilder,
            _forbiddenObject
         };
      }

      [Observation]
      public void the_dto_is_invalid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }
}