using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using MoBi.Helpers;

namespace MoBi.Core.Commands
{
   public abstract class concern_for_SwapBuildingBlockCommand<T> : ContextSpecification<SwapBuildingBlockCommand<T>> where T : class, IBuildingBlock
   {
      protected T _oldBuildingBlock;
      protected T _newBuildingBlock;
      protected IMoBiContext _context;
      protected MoBiProject _project;

      protected override void Context()
      {
         _oldBuildingBlock = A.Fake<T>().WithId("OLD");
         _newBuildingBlock = A.Fake<T>().WithId("NEW");
         _context= A.Fake<IMoBiContext>();
         _project= DomainHelperForSpecs.NewProject();
         A.CallTo(() => _context.CurrentProject).Returns(_project);
         sut = new SwapBuildingBlockCommand<T>(_oldBuildingBlock,_newBuildingBlock);
      }
   }

   public class When_swapping_out_two_building_blocks : concern_for_SwapBuildingBlockCommand<IBuildingBlock>
   {
      protected override void Context()
      {
         base.Context();
         _project.AddBuildingBlock(_oldBuildingBlock);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_old_building_block_from_the_project()
      {
         _project.AllBuildingBlocks().Contains(_oldBuildingBlock).ShouldBeFalse();
      }

      [Observation]
      public void should_add_the_new_building_block_to_the_project()
      {
         _project.AllBuildingBlocks().Contains(_newBuildingBlock).ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_id_of_the_old_building_block_in_the_new_building_block()
      {
         _newBuildingBlock.Id.ShouldBeEqualTo("OLD");
      }
   }
}	