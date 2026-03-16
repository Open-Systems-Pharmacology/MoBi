using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Format;

namespace MoBi.Presentation.Formatters;

public class UsedCalculationMethodCategoryFormatter : IFormatter<string>
{
   public string Format(string category)
   {
      switch (category)
      {
         case Assets.AppConstants.UsedCalculationMethods.Categories.DiffusionIntCell:
            return Assets.AppConstants.Captions.CellularPermeabilities;

         case Assets.AppConstants.UsedCalculationMethods.Categories.DistributionCellular:
            return Assets.AppConstants.Captions.PartitionCoefficients;
      }

      return category.SplitToUpperCase();
   }
}