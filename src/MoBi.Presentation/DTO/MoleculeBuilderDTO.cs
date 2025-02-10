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

      public IEnumerable<UsedCalculationMethodDTO> UsedCalculationMethods { set; get; }
      public IEnumerable<TransporterMoleculeContainerDTO> TransporterMolecules { get; set; }
      public FormulaBuilderDTO DefaultStartFormula { get; set; }
      public bool Stationary { get; set; }
      public IEnumerable<ParameterDTO> Parameters { get; set; }
      public QuantityType MoleculeType { set; get; }
      public IEnumerable<InteractionContainerDTO> InteractionContainerCollection { set; get; }
   }

   public class UsedCalculationMethodDTO
   {
      public string Category { get; set; }
      public string CalculationMethodName { get; set; }
      public string Description { get; set; }
   }
}