using MoBi.HelpersForTests;
using OSPSuite.Core.Snapshots.Mappers;
using OSPSuite.Utility.Container;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Snapshots.Services;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.IntegrationTests.Snapshots
{
   [Category("Snapshot Integration")]
   public abstract class ContextWithLoadedSnapshot : ContextForIntegration<ISnapshotMapper>
   {
      private ISnapshotTask _snapshotTask;
      protected MoBiProject _project;

      public void LoadSnapshot(string snapshotFileName, bool isFullPath = false)
      {
         var snapshotFile = isFullPath ? snapshotFileName : DomainHelperForSpecs.DataTestFileFullPath($"{snapshotFileName}.json");
         _snapshotTask = IoC.Resolve<ISnapshotTask>();
         _project = _snapshotTask.LoadProjectFromSnapshotFileAsync(snapshotFile).Result;
      }

      public TBuildingBlock FindByName<TBuildingBlock>(string name) where TBuildingBlock : class, IBuildingBlock
      {
         var bb = _project.All<TBuildingBlock>().FindByName(name);
         return bb;
      }

      public TBuildingBlock First<TBuildingBlock>() where TBuildingBlock : class, IBuildingBlock
      {
         return _project.All<TBuildingBlock>().FirstOrDefault();
      }

      public List<TBuildingBlock> All<TBuildingBlock>() where TBuildingBlock : class, IBuildingBlock
      {
         return _project.All<TBuildingBlock>().ToList();
      }
   }
}
