using MoBi.Core.Domain.Model;
using OSPSuite.Core.Comparison;

namespace MoBi.Core.Domain.Comparison
{
   class MoBiBuildConfigurationDiffBuilder:DiffBuilder<IMoBiBuildConfiguration>
   {
      private readonly IObjectComparer _objectComparer;

      public MoBiBuildConfigurationDiffBuilder(IObjectComparer objectComparer)
      {
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<IMoBiBuildConfiguration> comparison)
      {
         _objectComparer.Compare(comparison.ChildComparison(x => x.SpatialStructureInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.MoleculesInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.ReactionsInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.PassiveTransportsInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.EventGroupsInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.ObserversInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.SimulationSettingsInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.ParameterStartValuesInfo, comparison.CommonAncestor));
         _objectComparer.Compare(comparison.ChildComparison(x => x.MoleculeStartValuesInfo, comparison.CommonAncestor));
      }
   }
}