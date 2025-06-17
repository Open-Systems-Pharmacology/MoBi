using System;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Snapshots.Mappers;

namespace MoBi.Core.Snapshots.Mappers
{
   public class DataRepositoryMapper : DataRepositoryMapper<MoBiProject, SnapshotContext>
   {
      public DataRepositoryMapper(ExtendedPropertyMapper extendedPropertyMapper, DataColumnMapper dataColumnMapper) : base(extendedPropertyMapper, dataColumnMapper)
      {
      }

      protected override SnapshotContextWithDataRepository<MoBiProject> ContextFor(SnapshotContext snapshotContext, DataRepository dataRepository)
      {
         return new SnapshotContextWithDataRepository(dataRepository, snapshotContext);
      }
   }
}
