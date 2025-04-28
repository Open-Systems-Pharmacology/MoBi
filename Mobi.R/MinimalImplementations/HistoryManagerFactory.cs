using System;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Infrastructure.Serialization.ORM.History;

namespace MoBi.R.MinimalImplementations
{
   public class HistoryManagerFactory : IHistoryManagerFactory
   {
      public HistoryManagerFactory()
      {
      }

      public IHistoryManager Create()
      {
         throw new NotImplementedException();
      }
   }
}