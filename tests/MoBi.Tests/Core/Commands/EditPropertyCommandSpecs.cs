using FakeItEasy;

using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;


namespace MoBi.Core.Commands
{
   public  class When_creating_a_new_Edit_Property_command : ContextSpecification<EditObjectBasePropertyInBuildingBlockCommand>
   {
      [Observation]
      public void should_be_able_to_use_null_for_buildingblock()
      {
         IParameter parameter= new Parameter();
         sut = new EditObjectBasePropertyInBuildingBlockCommand(parameter.PropertyName(x => x.IsFixedValue),true,false,parameter, null);
      }
   }

   public abstract class concern_for_EditPropertyCommandSpecs:ContextSpecification<EditObjectBasePropertyInBuildingBlockCommand>
   {
      protected IParameter _parameter;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter();
         sut = new EditObjectBasePropertyInBuildingBlockCommand(_parameter.PropertyName(x => x.IsFixedValue),true,false,_parameter, A.Fake<IBuildingBlock>().WithId(null));
      }
   }

   public class When_restore_executon_Data_for_an_EditPropertyCommand_with_no_building_block_id_is_called : concern_for_EditPropertyCommandSpecs
   {
      private IMoBiContext _context;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IMoBiContext>();
         sut.Execute(_context);
         A.CallTo(() => _context.Get<IObjectBase>(A<string>._)).Returns(_parameter);
      }

      protected override void Because()
      {
         sut.RestoreExecutionData(_context);
      }

      [Observation]
      public void should_not_try_to_get_building_block_from_context()
      {
         A.CallTo(() => _context.Get<IBuildingBlock>(null)).MustNotHaveHappened();
      }
   }
}	