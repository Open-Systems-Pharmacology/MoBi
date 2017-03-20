using System;
using MoBi.Assets;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Domain.Services
{
   internal class DataNamingService : IDataNamingService
   {
      private readonly IMoBiContext _context;

      public DataNamingService(IMoBiContext context)
      {
         _context = context;
      }

      public string GetTimeName()
      {
         return AppConstants.TimeColumName;
      }

      public string GetNewRepositoryName()
      {
         return AppConstants.ResultName + DateTime.Now.ToIsoFormat(withSeconds: true);
      }

      public string GetEntityName(string id)
      {
         var objectBase = _context.Get<IObjectBase>(id);
         if (objectBase == null)
            return id;

         var entityName = objectBase.Name;
         if (!objectBase.IsAnImplementationOf<IObserver>())
            return entityName;

         var observer = objectBase.DowncastTo<IObserver>();
         var moleculeName = observer.ParentContainer.Name;
         return $"{moleculeName} {entityName}";
      }
   }
}