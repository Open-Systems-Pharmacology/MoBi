using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Helpers;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForMoleculeBuildingBlock : ContextSpecification<InteractionTasksForMoleculeBuildingBlock>
   {
      protected override void Context()
      {
         sut = new InteractionTasksForMoleculeBuildingBlock(
            A.Fake<IInteractionTaskContext>(), 
            A.Fake<IEditTasksForBuildingBlock<IMoleculeBuildingBlock>>(), 
            A.Fake<IInteractionTasksForBuilder<IMoleculeBuilder>>(), 
            A.Fake<IMoleculeBuildingBlockCloneManager>(),
            A.Fake<IMoBiFormulaTask>());
      }
   }

   public class when_removing_molecule_building_block_that_is_referred_to_in_another_building_block : concern_for_InteractionTasksForMoleculeBuildingBlock
   {
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock {Id="1"};
         _project = new MoBiProject();

         _project.AddBuildingBlock(new MoleculeStartValuesBuildingBlock {MoleculeBuildingBlockId = _moleculeBuildingBlock.Id, SpatialStructureId = ""});
      }

      [Observation]
      public void should_throw_mobi_exception()
      {
         The.Action(() => sut.Remove(_moleculeBuildingBlock, _project, null, false)).ShouldThrowAn<MoBiException>();
      }
   }
}
