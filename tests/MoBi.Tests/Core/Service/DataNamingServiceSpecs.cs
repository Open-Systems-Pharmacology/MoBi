using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;


namespace MoBi.Core.Service
{
   public abstract class concern_for_DataNamingService : ContextSpecification<IDataNamingService>
   {
      protected IMoBiContext _context;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         sut = new DataNamingService(_context);
      }
   }

   public class When_asking_data_naming_service_for_entity_name : concern_for_DataNamingService
   {
      private string _id ="ID";
      private string _entityName="Entity";
      private string _returnedName;

      protected override void Context()
      {
         base.Context();
         var entity = A.Fake<IObjectBase>().WithId(_id).WithName(_entityName);
         A.CallTo(() => _context.Get<IObjectBase>(_id)).Returns(entity);
      }

      protected override void Because()
      {
         _returnedName = sut.GetEntityName(_id);
      }

      [Observation]
      public void should_reteive_entity_from_context()
      {
         A.CallTo(() => _context.Get<IObjectBase>(_id));
      }

      [Observation]
      public void should_return_entity_Name()
      {
         _returnedName.ShouldBeEqualTo(_entityName);
      }
   }
}	