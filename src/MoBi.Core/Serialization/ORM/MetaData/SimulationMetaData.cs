using System.Collections.Generic;
using MoBi.Core.Serialization.ORM.MetaData.Extensions;
using NHibernate;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public class SimulationMetaData : EntityMetaData
   {
      public virtual ICollection<DataRepositoryMetaData> HistoricalResults { get; set; }

      public SimulationMetaData()
      {
         HistoricalResults = new HashSet<DataRepositoryMetaData>();
      }

      public virtual void AddHistoricalResults(DataRepositoryMetaData dataRepositoryMetaData)
      {
         HistoricalResults.Add(dataRepositoryMetaData);
      }

      public override void UpdateFrom(EntityMetaData sourceMetaData, ISession session)
      {
         var sourceSimulation = sourceMetaData as SimulationMetaData;
         if (sourceSimulation == null) return;
         base.UpdateFrom(sourceSimulation, session);

         HistoricalResults.UpdateFrom<string, DataRepositoryMetaData>(sourceSimulation.HistoricalResults, session);
      }
   }
}