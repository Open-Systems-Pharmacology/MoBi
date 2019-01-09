using FakeItEasy;
using MoBi.Core.Serialization.ORM.MetaData;
using NHibernate;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace MoBi.Core
{
   public abstract class concern_for_ProjectMetaData : ContextSpecification<ProjectMetaData>
   {
      protected override void Context()
      {
         sut = new ProjectMetaData();
      }
   }

   public class When_updating_Properties : concern_for_ProjectMetaData
   {
      private ProjectMetaData _orgMetaData;
      private ISession _session;

      protected override void Context()
      {
         base.Context();
         _orgMetaData = new ProjectMetaData();
         _session = A.Fake<ISession>();
         _orgMetaData.Version = 1;
      }

      protected override void Because()
      {
         sut.UpdateFrom(_orgMetaData, _session);
      }

      [Observation]
      public void should_update_Version()
      {
         sut.Version.ShouldBeEqualTo(_orgMetaData.Version);
      }
   }
}