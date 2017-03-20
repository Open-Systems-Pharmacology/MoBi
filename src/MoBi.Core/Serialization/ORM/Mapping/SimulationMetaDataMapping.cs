using FluentNHibernate.Mapping;
using MoBi.Core.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.Mapping
{
   public class SimulationMetaDataMapping : SubclassMap<SimulationMetaData>
   {
      public SimulationMetaDataMapping()
      {
         Table("SIMULATIONS");
         KeyColumn("SimulationId");

         HasMany(x => x.HistoricalResults)
            .Not.LazyLoad()
            .Fetch.Join()
            .Cascade.AllDeleteOrphan()
            .KeyColumn("SimulationId")
            .AsSet();
      }
   }
}