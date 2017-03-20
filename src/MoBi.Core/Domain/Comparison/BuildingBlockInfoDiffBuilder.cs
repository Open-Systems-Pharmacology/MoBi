using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Comparison;

namespace MoBi.Core.Domain.Comparison
{
   public class BuildingBlockInfoDiffBuilder : DiffBuilder<IBuildingBlockInfo>
   {
      public override void Compare(IComparison<IBuildingBlockInfo> comparison)
      {
         CompareStringValues(x => x.UntypedBuildingBlock.Name, AppConstants.Captions.Name, comparison);
         compareBuildingBlockVersion(comparison);
      }

      private void compareBuildingBlockVersion(IComparison<IBuildingBlockInfo> comparison)
      {
         var buildingBlock1 = comparison.Object1.UntypedBuildingBlock;
         var buildingBlock2 = comparison.Object2.UntypedBuildingBlock;
         var bb1Version = buildingBlock1.Version;
         var bb2Version = buildingBlock2.Version;

         if (bb1Version == bb2Version)
            return;

         if (bb1Version < bb2Version)
            addVersionDiffitem(comparison, AppConstants.Diff.OlderVersion, AppConstants.Diff.NewerVersion);

         if (bb1Version > bb2Version)
            addVersionDiffitem(comparison, AppConstants.Diff.NewerVersion, AppConstants.Diff.OlderVersion);
      }
     
      private static void addVersionDiffitem(IComparison<IBuildingBlockInfo> comparison, string buildingBlock1, string buildingBlock2)
      {
         comparison.Add(new PropertyValueDiffItem()
         {
            Object1 = comparison.Object1,
            Object2 = comparison.Object2,
            CommonAncestor = comparison.CommonAncestor,
            PropertyName = AppConstants.Diff.Version,
            FormattedValue1 = buildingBlock1,
            FormattedValue2 = buildingBlock2,
            Description = AppConstants.Diff.VersionDescription(buildingBlock1)
         });
      }
   }
}