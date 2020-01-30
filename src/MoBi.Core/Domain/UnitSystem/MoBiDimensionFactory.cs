using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.UnitSystem
{
   public interface IMoBiDimensionFactory : IDimensionFactory
   {
      IDimension TryGetDimensionCaseInsensitive(string dimensionName);
   }

   public class MoBiDimensionFactory : DimensionFactory, IMoBiDimensionFactory
   {
      private IEnumerable<IMoBiDimensionMergingInformation> mobiDimensionMergingInformationList => AllMergingInformation.Cast<IMoBiDimensionMergingInformation>();

      public IDimension TryGetDimensionCaseInsensitive(string dimensionName)
      {
         var dimension = Dimensions.FirstOrDefault(d => d.Name.Equals(dimensionName, StringComparison.OrdinalIgnoreCase));
         return dimension ?? NoDimension;
      }

      protected override IDimensionConverterFor CreateConverterFor<T>(IDimension dimension, IDimension dimensionToMerge, T hasDimension)
      {
         var converter = (from mergeInfo in mobiDimensionMergingInformationList
            where mergeInfo.Matches(dimension, dimensionToMerge, hasDimension)
            select mergeInfo.Converter).SingleOrDefault();

         if (converter == null)
            return null;

         var usedConverter = Activator.CreateInstance(converter.GetType(), dimension, dimensionToMerge) as IMoBiDimensionConverterFor<T>;
         if (usedConverter != null)
         {
            usedConverter.SetRefObject(hasDimension);
            return usedConverter;
         }

         return converter;
      }

      public override string MergedDimensionNameFor(IDimension sourceDimension)
      {
         if (isConcentration(sourceDimension))
            return AppConstants.Captions.Concentration;

         return base.MergedDimensionNameFor(sourceDimension);
      }

      private bool isConcentration(IDimension dimension)
      {
         return dimension.Name.Contains(AppConstants.Captions.Concentration);
      }
   }
}