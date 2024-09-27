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
         .WithRule((dto, newFormula) => !formulaRefersToAssignmentFormula(dto, newFormula))
         .WithError(AppConstants.Validation.CannotAssignAFormulaThatReferencesTheAssignmentTarget);

      private static bool formulaRefersToAssignmentFormula(EventAssignmentBuilderDTO dto, FormulaBuilderDTO formulaDTO)
      {
         var assignmentBuilder = dto._eventAssignmentBuilder;
         var assignmentPath = assignmentBuilder.ObjectPath;
         var formulaPaths = formulaDTO.ObjectPaths.Select(x => x.FormulaUsablePath).ToList();

         if (!canReferToAssignmentTarget(assignmentBuilder.UseAsValue, assignmentPath, formulaPaths))
            return false;

         return formulaPathsReferToAssignmentTarget(assignmentPath, assignmentBuilder, formulaPaths);
      }

      private IBusinessRule cannotAssignCircularRefRuleAssignment { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.ChangedEntityPath)
         .WithRule((dto, changedEntityPath) => !formulaRefersToAssignmentTarget(dto, changedEntityPath))
         .WithError(AppConstants.Validation.CannotAssignAFormulaThatReferencesTheAssignmentTarget);

      private static bool formulaRefersToAssignmentTarget(EventAssignmentBuilderDTO dto, string changedEntityPath)
      {
         var assignmentBuilder = dto._eventAssignmentBuilder;

         var assignmentPath = new ObjectPath(changedEntityPath.ToPathArray());
         var assignmentFormulaPaths = assignmentBuilder.Formula?.ObjectPaths;

         if (!canReferToAssignmentTarget(assignmentBuilder.UseAsValue, assignmentPath, assignmentFormulaPaths))
            return false;

         return formulaPathsReferToAssignmentTarget(assignmentPath, assignmentBuilder, assignmentFormulaPaths);
      }

      private IBusinessRule cannotAssignCircularRefRuleUseAsValue { get; } = CreateRule.For<EventAssignmentBuilderDTO>()
         .Property(x => x.UseAsValue)
         .WithRule((dto, useAsValue) => !formulaRefersToAssignmentTarget(dto, useAsValue))
         .WithError(AppConstants.Validation.CannotAssignAFormulaThatReferencesTheAssignmentTarget);

      private static bool formulaRefersToAssignmentTarget(EventAssignmentBuilderDTO dto, bool useAsValue)
      {
         var assignmentBuilder = dto._eventAssignmentBuilder;

         var assignmentPath = assignmentBuilder.ObjectPath;
         var assignmentFormulaPaths = assignmentBuilder.Formula?.ObjectPaths;

         if (!canReferToAssignmentTarget(useAsValue, assignmentPath, assignmentFormulaPaths))
            return false;

         return formulaPathsReferToAssignmentTarget(assignmentPath, assignmentBuilder, assignmentFormulaPaths);
      }

      private static bool formulaPathsReferToAssignmentTarget(ObjectPath assignmentPath, EventAssignmentBuilder assignmentBuilder, IReadOnlyList<ObjectPath> formulaPaths)
      {
         var assignmentTarget = assignmentPath.TryResolve<IUsingFormula>(assignmentBuilder);
         return formulaPaths.Any(path => path.TryResolve<IUsingFormula>(assignmentBuilder) == assignmentTarget);
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