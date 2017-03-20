using MoBi.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Core.Domain.Extensions
{
   public static class DataRepositoryExtensions
   {
      public static bool IsPersistable(this DataRepository dataRepository)
      {
         if (!dataRepository.ExtendedProperties.Contains(AppConstants.Persitable))
            return false;

         var persitableProperty = dataRepository.ExtendedProperties[AppConstants.Persitable] as ExtendedProperty<bool>;
         if (persitableProperty == null)
            return false;

         return persitableProperty.Value;
      }

      public static void SetPersistable(this DataRepository dataRepository, bool persistable)
      {
         ExtendedProperty<bool> persitableProperty=null;
         if (dataRepository.ExtendedProperties.Contains(AppConstants.Persitable))
             persitableProperty = dataRepository.ExtendedProperties[AppConstants.Persitable] as ExtendedProperty<bool>;

         if (persitableProperty == null)
            persitableProperty = new ExtendedProperty<bool> {Name = AppConstants.Persitable};

         persitableProperty.Value = persistable;
         dataRepository.ExtendedProperties[AppConstants.Persitable] = persitableProperty;
      }
   }
}