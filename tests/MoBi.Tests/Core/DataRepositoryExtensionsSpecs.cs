using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Core
{
   public class When_setting_the_persitable_state_for_a_data_repository : StaticContextSpecification
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         _dataRepository = new DataRepository();
      }

      [Observation]
      public void the_default_value_should_be_false()
      {
         _dataRepository.IsPersistable().ShouldBeFalse();
      }

      [Observation]
      public void should_be_able_to_set_the_value_if_it_was_not_there_before()
      {
         _dataRepository.SetPersistable(true);
         _dataRepository.IsPersistable().ShouldBeTrue();
      }

      [Observation]
      public void should_be_able_to_reset_the_value()
      {
         _dataRepository.SetPersistable(true);
         _dataRepository.SetPersistable(false);
         _dataRepository.IsPersistable().ShouldBeFalse();
      }
   }
}