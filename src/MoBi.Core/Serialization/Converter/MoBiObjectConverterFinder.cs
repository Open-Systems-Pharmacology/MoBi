using OSPSuite.Utility.Collections;

namespace MoBi.Core.Serialization.Converter
{
   public interface IMoBiObjectConverterFinder
   {
      IMoBiObjectConverter FindConverterFor(int version);
   }

   public class MoBiObjectConverterFinder: IMoBiObjectConverterFinder
   {
      private readonly IRepository<IMoBiObjectConverter> _objectConverters;

      public MoBiObjectConverterFinder(IRepository<IMoBiObjectConverter> objectConverters)
      {
         _objectConverters = objectConverters;
      }

      public IMoBiObjectConverter FindConverterFor(int version)
      {
         foreach (var objectConverter in _objectConverters.All())
         {
            if (objectConverter.IsSatisfiedBy(version))
               return objectConverter;
         }

         return new MoBiNullConverter();
      }
   }
}