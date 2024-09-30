using FakeItEasy;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   internal class concern_for_EventAssignmentBuilderDTO : ContextSpecification<EventAssignmentBuilderDTO>
   {
      private EventAssignmentBuilderDTO _assignmentDTO;
      private Parameter _forbiddenObject;
      private EventBuilder _eventBuilder;

      protected override void Context()
      {
         var eventAssignmentBuilder = new EventAssignmentBuilder
         {
            UseAsValue = false,
            Formula = A.Fake<ExplicitFormula>(),
            ObjectPath = new ObjectPath("..", "..", "Second"),
         };

         A.CallTo(() => eventAssignmentBuilder.Formula.ObjectPaths).Returns(new List<FormulaUsablePath> { new FormulaUsablePath("..", "..", "Second") });
         _assignmentDTO = new EventAssignmentBuilderDTO(eventAssignmentBuilder)
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
         _assignmentDTO.Validate().Count.ShouldBeEqualTo(3);
      }
   }
}
