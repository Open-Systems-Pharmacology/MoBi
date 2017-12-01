using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace MoBi.Core.Domain.Services
{
   public interface IUnregisterTask
   {
      void UnregisterAllIn(IWithId objectToUnregister);
   }

   public class UnregisterTask : AbstractRegistrationTask, IUnregisterTask
   {
      public UnregisterTask(IWithIdRepository withIdRepository) : base(withIdRepository)
      {
      }

      public void UnregisterAllIn(IWithId objectToUnregister)
      {
         var objectBase = objectToUnregister as IObjectBase;
         if (objectBase == null)
            Visit(objectToUnregister);
         else
            objectBase.AcceptVisitor(this);
      }

      private void unregister(IWithId objectToUnregister)
      {
         if (objectToUnregister == null) return;
         _withIdRepository.Unregister(objectToUnregister.Id);
      }

      public override void Visit(IWithId withId)
      {
         unregister(withId);
      }

      protected override void Visit(IFormula formula)
      {
         if (formula.IsCachable()) return;
         unregister(formula);
      }
   }
}