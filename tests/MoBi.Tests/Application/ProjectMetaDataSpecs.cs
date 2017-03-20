using System.Linq;
using MoBi.Core.Serialization.ORM.MetaData;
using NHibernate;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;


namespace MoBi.Application
{
   public abstract class concern_for_ProjectMetaDataSpecs : ContextSpecification<ProjectMetaData>
   {
      protected override void Context()
      {
         sut = new ProjectMetaData();
      }
   }

   class When_updateing_Properties : concern_for_ProjectMetaDataSpecs
   {
      private ProjectMetaData _orgMetaData;
      private ISession _session;

      protected override void Context()
      {
         base.Context();
         _orgMetaData = new ProjectMetaData();
         _session = A.Fake<ISession>();
         _orgMetaData.Version = 1;
         _orgMetaData.Content = A.Fake<MetaDataContent>();

      }

      protected override void Because()
      {
         sut.UpdateFrom(_orgMetaData,_session);
      }

      [Observation]
      public void should_update_Version()
      {
         sut.Version.ShouldBeEqualTo(_orgMetaData.Version);
      }
   }

   
}	