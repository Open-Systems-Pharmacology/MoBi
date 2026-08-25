using System.Collections.Generic;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Domain.Services
{
   public interface IDimensionValidator
   {
      Task<ValidationResult> Validate(IContainer container, SimulationBuilder simulationBuilder);
      Task<ValidationResult> Validate(IModel model, SimulationBuilder simulationBuilder);
      Task<ValidationResult> Validate(IEnumerable<IContainer> containers, SimulationBuilder simulationBuilder);
   }
}
