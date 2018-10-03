using System.Collections.Generic;
using System.Linq;
using NHibernate;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace MoBi.Core.Serialization.ORM.MetaData
{
   public class ProjectMetaData : MetaDataWithContent<int>
   {
      public virtual string Name { get; set; }
      public virtual string Description { get; set; }
      public virtual int Version { get; set; }
      public virtual ICollection<EntityMetaData> Children { get; private set; }

      public ProjectMetaData()
      {
         Version = ProjectVersions.Current;
         Children = new HashSet<EntityMetaData>();
      }

      public virtual void AddChild(EntityMetaData childToAdd)
      {
         Children.Add(childToAdd);
      }

      public void UpdateFrom(ProjectMetaData projectMetaData, ISession session)
      {
         Version = projectMetaData.Version;
         UpdateContentFrom(projectMetaData);
         Children.UpdateFrom<string, EntityMetaData>(projectMetaData.Children, session);
      }

      public virtual IEnumerable<SimulationMetaData> Simulations => Children.OfType<SimulationMetaData>();
   }
}