using System.Linq.Expressions;
using Antlr.Runtime.Misc;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Comparison
{
   public class ExpressionProfileBuildingBlockDiffBuilder : StartValueBuildingBlockDiffBuilder<ExpressionParameter>
   {
      public ExpressionProfileBuildingBlockDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }
   }

   // This is copy/paste with StartValueDiffBuilder from Core.
   // TODO implement with common base when defined
   internal class ExpressionParameterDiffBuilder : DiffBuilder<ExpressionParameter> 
   {
      private readonly IObjectComparer _objectComparer;
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly WithValueOriginComparison<ExpressionParameter> _valueOriginComparison;

      public ExpressionParameterDiffBuilder(IObjectComparer objectComparer, EntityDiffBuilder entityDiffBuilder, WithValueOriginComparison<ExpressionParameter> valueOriginComparison)
      {
         _objectComparer = objectComparer;
         _entityDiffBuilder = entityDiffBuilder;
         _valueOriginComparison = valueOriginComparison;
      }

      public override void Compare(IComparison<ExpressionParameter> comparison)
      {
         _valueOriginComparison.AddValueOriginToComparison(comparison, this, CompareStartValue);
      }

      private string nameFrom<TInput, TOutput>(Expression<Func<TInput, TOutput>> propertyNameExpression)
      {
         return propertyNameExpression.Name().SplitToUpperCase();
      }

      protected virtual void CompareStartValue(IComparison<ExpressionParameter> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, nameFrom<ExpressionParameter, IDimension>(x => x.Dimension), comparison);

         CompareValues(x => x.ContainerPath, nameFrom<ExpressionParameter, IObjectPath>(x => x.ContainerPath), comparison);

         // Always Compare Value and Formula, independent from settings as these are two different properties of a start value
         CompareNullableDoubleValues(x => x.StartValue, x => x.StartValue, comparison, x => x.DisplayUnit);
         _objectComparer.Compare(comparison.FormulaComparison());
      }
   }

}
