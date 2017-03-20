using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace MoBi.Core.Domain.Services
{
   internal class MoBiDataFactory : DataFactory
   {
      public MoBiDataFactory(IMoBiContext context, IDataNamingService dataNamingService, IDisplayUnitRetriever displayUnitRetriever) : 
         base(context.DimensionFactory,dataNamingService,context.ObjectPathFactory, displayUnitRetriever)
      {
      }
   }

   
}