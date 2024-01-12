using MoBi.Presentation.Formatters;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Format;

namespace MoBi.Presentation.DTO
{
   public abstract class SelectableReplacePathAndValueDTO : BreadCrumbsDTO<PathAndValueEntity>
   {
      protected SelectableReplacePathAndValueDTO(PathAndValueEntity replacementEntity) : base(replacementEntity)
      {
         ContainerPath = replacementEntity.ContainerPath;
      }

      public bool Selected { get; set; }

      public abstract string Name { get; }

      public abstract double? OldValue { get; }
      public abstract IFormula OldFormula { get; }
      public abstract Unit OldDisplayUnit { get; }

      public abstract double? NewValue { get; }
      public abstract IFormula NewFormula { get; }
      public abstract Unit NewDisplayUnit { get; }

      public IFormatter<double?> OldValueFormatter()
      {
         return new NullableWithRetrievableDisplayUnitFormatter(() => OldDisplayUnit);
      }

      public IFormatter<double?> NewValueFormatter()
      {
         return new NullableWithRetrievableDisplayUnitFormatter(() => NewDisplayUnit);
      }
   }

   public class SelectableReplacePathAndValueDTO<TPathAndValueEntity> : SelectableReplacePathAndValueDTO where TPathAndValueEntity : PathAndValueEntity
   {
      private readonly TPathAndValueEntity _oldEntity;

      public SelectableReplacePathAndValueDTO(TPathAndValueEntity replacementEntity, TPathAndValueEntity oldEntity) : base(replacementEntity)
      {
         NewEntity = replacementEntity;
         _oldEntity = oldEntity;
      }

      public override string Name => NewEntity.Name;

      public override double? OldValue => _oldEntity.Value;
      public override IFormula OldFormula => _oldEntity.Formula;
      public override Unit OldDisplayUnit => _oldEntity.DisplayUnit;

      public override double? NewValue => NewEntity.Value;
      public override IFormula NewFormula => NewEntity.Formula;
      public override Unit NewDisplayUnit => NewEntity.DisplayUnit;
      public TPathAndValueEntity NewEntity { get; }
   }
}