using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Domain.Services
{
   public abstract class AbstractRegistrationTask : 
      IVisitor<IUsingFormula>,
      IVisitor<MoleculeBuilder>,
      IVisitor<IWithId>
   {
      protected IWithIdRepository _withIdRepository;

      protected AbstractRegistrationTask(IWithIdRepository withIdRepository)
      {
         _withIdRepository = withIdRepository;
      }

      public abstract void Visit(IWithId objectBase);
      protected abstract void Visit(IFormula formula);

      public void Visit(IUsingFormula usingFormula)
      {
         Visit(usingFormula.DowncastTo<IWithId>());
         Visit(usingFormula.Formula);
      }

      public void Visit(MoleculeBuilder moleculeBuilder)
      {
         Visit(moleculeBuilder.DowncastTo<IWithId>());
         Visit(moleculeBuilder.DefaultStartFormula);
      }
   }
}