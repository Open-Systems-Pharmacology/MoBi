using System.Collections.Generic;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Services
{
   public interface IDimensionValidator : IVisitor<IEntity>, IVisitor<IUsingFormula>
   {
      Task<ValidationResult> Validate(IContainer container, SimulationBuilder simulationBuilder);
      Task<ValidationResult> Validate(IModel model, SimulationBuilder simulationBuilder);
      Task<ValidationResult> Validate(IEnumerable<IContainer> containers, SimulationBuilder simulationBuilder);
   }
}