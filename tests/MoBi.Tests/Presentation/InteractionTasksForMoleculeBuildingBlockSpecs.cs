using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForMoleculeBuildingBlock : ContextSpecification<InteractionTasksForMoleculeBuildingBlock>
   {
      private IInteractionTaskContext _interactionTaskContext;
      private IEditTasksForBuildingBlock<IMoleculeBuildingBlock> _editTasksForBuildingBlock;
      private IInteractionTasksForBuilder<IMoleculeBuilder> _task;
      private IMoleculeBuildingBlockCloneManager _moleculeBuildingBlockCloneManager;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTasksForBuildingBlock = A.Fake<IEditTasksForBuildingBlock<IMoleculeBuildingBlock>>();
         _task = A.Fake<IInteractionTasksForBuilder<IMoleculeBuilder>>();
         _moleculeBuildingBlockCloneManager = A.Fake<IMoleculeBuildingBlockCloneManager>();

         sut = new InteractionTasksForMoleculeBuildingBlock(_interactionTaskContext, _editTasksForBuildingBlock, _task, _moleculeBuildingBlockCloneManager);
      }
   }

   public class When_removing_molecule_building_block_that_is_referred_to_in_another_building_block : concern_for_InteractionTasksForMoleculeBuildingBlock
   {
      private IMoleculeBuildingBlock _moleculeBuildingBlock;
      private IMoBiProject _project;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock {Id = "1"};
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