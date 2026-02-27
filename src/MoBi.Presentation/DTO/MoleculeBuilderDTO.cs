using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class MoleculeBuilderDTO : ObjectBaseDTO
   {
      public MoleculeBuilderDTO(MoleculeBuilder moleculeBuilder) : base(moleculeBuilder)
      {
      }

      public IReadOnlyList<UsedCalculationMethodDTO> UsedCalculationMethods { set; get; }
      public IReadOnlyList<TransporterMoleculeContainerDTO> TransporterMolecules { get; set; }
      public FormulaBuilderDTO DefaultStartFormula { get; set; }
      public bool Stationary { get; set; }
      public IReadOnlyList<ParameterDTO> Parameters { get; set; }
      public QuantityType MoleculeType { set; get; }
      public IReadOnlyList<InteractionContainerDTO> InteractionContainerCollection { set; get; }
   }

   public class UsedCalculationMethodDTO
   {
      public string Category { get; set; }
      public string CalculationMethodName { get; set; }
      public string Description { get; set; }
   }
}