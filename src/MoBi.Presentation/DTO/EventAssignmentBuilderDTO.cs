using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class EventAssignmentBuilderDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO NewFormula { get; set; }
      public bool UseAsValue { get; set; }
      private string _changedEntityPath;
      private readonly EventAssignmentBuilder _eventAssignmentBuilder;

      public EventAssignmentBuilderDTO(EventAssignmentBuilder eventAssignmentBuilder) : base(eventAssignmentBuilder)
      {
         _eventAssignmentBuilder = eventAssignmentBuilder;
         Rules.Add(createTargetHasToBeSetRule);
         Rules.Add(cannotAssignCircularRefRuleUseAsValue);
         Rules.Add(cannotAssignCircularRefRuleAssignment);
         Rules.Add(cannotAssignCircularRefRuleFormula);
      }

      private IBusinessRule cannotAssignCircularRefRuleFormula { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.NewFormula)
         .WithRule((dto, newFormula) => !formulaRefersToAssignmentFormula(dto._eventAssignmentBuilder, newFormula))
         .WithError(AppConstants.Validation.CannotAssignAFormulaThatReferencesTheAssignmentTarget);

      private static bool formulaRefersToAssignmentFormula(EventAssignmentBuilder assignmentBuilder, FormulaBuilderDTO formulaDTO)
      {
         var assignmentPath = assignmentBuilder.ObjectPath;
         var formulaPaths = formulaDTO.ObjectPaths.Select(x => x.FormulaUsablePath).ToList();

         return formulaPathsReferToAssignmentTarget(assignmentBuilder, assignmentBuilder.UseAsValue, assignmentPath, formulaPaths);
      }

      private IBusinessRule cannotAssignCircularRefRuleAssignment { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.ChangedEntityPath)
         .WithRule((dto, changedEntityPath) => !formulaRefersToAssignmentTarget(dto._eventAssignmentBuilder, changedEntityPath))
         .WithError(AppConstants.Validation.CannotAssignAFormulaThatReferencesTheAssignmentTarget);

      private static bool formulaRefersToAssignmentTarget(EventAssignmentBuilder assignmentBuilder, string changedEntityPath)
      {
         return formulaPathsReferToAssignmentTarget(assignmentBuilder, assignmentBuilder.UseAsValue, new ObjectPath(changedEntityPath.ToPathArray()), assignmentBuilder.Formula?.ObjectPaths);
      }

      private IBusinessRule cannotAssignCircularRefRuleUseAsValue { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.UseAsValue)
         .WithRule((dto, useAsValue) => !formulaRefersToAssignmentTarget(dto._eventAssignmentBuilder, useAsValue))
         .WithError(AppConstants.Validation.CannotAssignAFormulaThatReferencesTheAssignmentTarget);

      private static bool formulaRefersToAssignmentTarget(EventAssignmentBuilder assignmentBuilder, bool useAsValue)
      {
         return formulaPathsReferToAssignmentTarget(assignmentBuilder, useAsValue, assignmentBuilder.ObjectPath, assignmentBuilder.Formula?.ObjectPaths);
      }

      private static bool formulaPathsReferToAssignmentTarget(EventAssignmentBuilder assignmentBuilder, bool useAsValue, ObjectPath assignmentPath, IReadOnlyList<FormulaUsablePath> assignmentFormulaPaths)
      {
         if (!canReferToAssignmentTarget(useAsValue, assignmentPath, assignmentFormulaPaths))
            return false;

         var assignmentTarget = assignmentPath.TryResolve<IUsingFormula>(assignmentBuilder);
         return assignmentTarget != null && assignmentFormulaPaths.Any(path => path.TryResolve<IUsingFormula>(assignmentBuilder) == assignmentTarget);
      }

      private static bool canReferToAssignmentTarget(bool useAsValue, ObjectPath objectPath, IReadOnlyList<ObjectPath> formulaPaths)
      {
         return !useAsValue && objectPath != null && formulaPaths != null && formulaPaths.Any();
      }

      private IBusinessRule createTargetHasToBeSetRule { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.ChangedEntityPath)
         .WithRule((dto, path) => !path.IsNullOrEmpty())
         .WithError(AppConstants.Validation.ChangedEntityNotSet);

      public virtual string ChangedEntityPath
      {
         get => _changedEntityPath;
         set => SetProperty(ref _changedEntityPath, value);
      }

      public IDimension Dimension
      {
         get => _eventAssignmentBuilder.Dimension;
         set => _eventAssignmentBuilder.Dimension = value;
      }
   }
}