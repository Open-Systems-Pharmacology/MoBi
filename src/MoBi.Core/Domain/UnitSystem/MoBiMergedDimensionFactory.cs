using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain.UnitSystem
{
   public interface IMoBiDimensionMergingInformation : IDimensionMergingInformation
   {
      IMoBiDimensionConverter Converter { get; }
      bool Matches(IDimension sourceDimension, IDimension targetDimension, IWithDimension withDimension);
   }

   public class MoBiDimensionMergingInformation<T> : IMoBiDimensionMergingInformation where T : IWithDimension
   {
      public IDimension SourceDimension { get; private set; }
      public IDimension TargetDimension { get; private set; }
      public IMoBiDimensionConverter Converter { get; private set; }

      public bool Matches(IDimension sourceDimension, IDimension targetDimension, IWithDimension withDimension)
      {
         return SourceDimension.Equals(sourceDimension) &&
                TargetDimension.Equals(targetDimension) &&
                Converter.CanBeUsedFor(withDimension);
      }

      public MoBiDimensionMergingInformation(IDimension sourceDimension, IDimension targetDimension, IMoBiDimensionConverter<T> converter)
      {
         SourceDimension = sourceDimension;
         TargetDimension = targetDimension;
         Converter = converter;
      }
   }
}