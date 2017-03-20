using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.UnitSystem
{
   public class MoBiMergedDimensionFactory : DimensionFactory
   {
      private IEnumerable<IMoBiDimensionMergingInformation> mobiDimensionMergingInformations
      {
         get { return AllMergingInformation.Cast<IMoBiDimensionMergingInformation>(); }
      }

      protected override IDimensionConverterFor CreateConverterFor<T>(IDimension dimension, IDimension dimensionToMerge, T hasDimension)
      {
         var converter = (from mergeInfo in mobiDimensionMergingInformations
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

   public interface IMoBiDimensionMergingInformation : IDimensionMergingInformation
   {
      IMoBiDimensionConverterFor Converter { get; }
      bool Matches(IDimension sourceDimension, IDimension targedDimension, IWithDimension withDimension);
   }

   public class MoBiDimensionMergingInformation<T> : IMoBiDimensionMergingInformation where T : IWithDimension
   {
      public IDimension SourceDimension { get; private set; }
      public IDimension TargetDimension { get; private set; }
      public IMoBiDimensionConverterFor Converter { get; private set; }
      public bool Matches(IDimension sourceDimension, IDimension targedDimension, IWithDimension withDimension)
      {
         return SourceDimension.Equals(sourceDimension) &&
                TargetDimension.Equals(targedDimension) &&
                Converter.CanBeUsedFor(withDimension);

      }

      public MoBiDimensionMergingInformation(IDimension sourceDimension, IDimension targetDimension, IMoBiDimensionConverterFor<T> converter)
      {
         SourceDimension = sourceDimension;
         TargetDimension = targetDimension;
         Converter = converter;
      }
   }
}