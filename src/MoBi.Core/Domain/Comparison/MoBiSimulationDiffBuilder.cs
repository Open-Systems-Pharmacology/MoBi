using MoBi.Core.Domain.Model;
using OSPSuite.Core.Comparison;

namespace MoBi.Core.Domain.Comparison
{
   public class MoBiSimulationDiffBuilder : DiffBuilder<IMoBiSimulation>
   {
      private readonly IObjectComparer _objectComparer;

      public MoBiSimulationDiffBuilder(IObjectComparer objectComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IMoBiSimulation> comparison)
      {
         _objectComparer.Compare(comparison.ChildComparison(x => x.Model));
         _objectComparer.Compare(comparison.ChildComparison(x => x.MoBiBuildConfiguration));
      }
   }
}