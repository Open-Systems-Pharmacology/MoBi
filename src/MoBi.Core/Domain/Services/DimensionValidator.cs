using System.Collections.Generic;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Services
{
   public interface IDimensionValidator : IVisitor<IEntity>, IVisitor<IUsingFormula>
   {
      Task<ValidationResult> Validate(IContainer container, IBuildConfiguration buildConfiguration);
      Task<ValidationResult> Validate(IModel model, IBuildConfiguration buildConfiguration);
      Task<ValidationResult> Validate(IEnumerable<IContainer> containers, IBuildConfiguration buildConfiguration);
   }
}