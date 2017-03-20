using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Model
{
   public class ClassifiableSimulation : Classifiable<IMoBiSimulation>
   {
      public ClassifiableSimulation() : base(ClassificationType.Simulation)
      {
      }

      public IMoBiSimulation Simulation
      {
         get { return Subject; }
      }
   }
}