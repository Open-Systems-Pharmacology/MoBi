using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class QuantityDTO : ObjectBaseDTO
   {
      public FormulaBuilderDTO Formula { set; get; }
      public ValueEditDTO Value { get; set; }

      public QuantityDTO(IQuantity quantity) : base(quantity)
      {
      }

      public IDimension Dimension
      {
         get => Value.Dimension;
         set => Value.Dimension = value;
      }

      public IReadOnlyCollection<UsedCalculationMethod> UsedCalculationMethods => ObjectBase is MoleculeAmount moleculeAmount ? moleculeAmount.UsedCalculationMethods : [];
      public SimulationEntitySourceReference SourceReference { get; set; }
      public string SourceDisplayName => SourceReference?.BuildingBlock?.DisplayName ?? string.Empty;
   }
}