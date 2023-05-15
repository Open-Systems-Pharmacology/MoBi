﻿using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Helpers;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation
{
   public abstract class concern_for_InteractionTasksForMoleculeBuildingBlock : ContextSpecification<InteractionTasksForMoleculeBuildingBlock>
   {
      private IInteractionTaskContext _interactionTaskContext;
      private IEditTasksForBuildingBlock<MoleculeBuildingBlock> _editTasksForBuildingBlock;
      private IInteractionTasksForBuilder<MoleculeBuilder> _task;
      protected MoBiProject _project;

      protected override void Context()
      {
         _interactionTaskContext = A.Fake<IInteractionTaskContext>();
         _editTasksForBuildingBlock = A.Fake<IEditTasksForBuildingBlock<MoleculeBuildingBlock>>();
         _task = A.Fake<IInteractionTasksForBuilder<MoleculeBuilder>>();
         _project = DomainHelperForSpecs.NewProject();
         A.CallTo(() => _interactionTaskContext.Context.CurrentProject).Returns(_project);
         
         sut = new InteractionTasksForMoleculeBuildingBlock(_interactionTaskContext, _editTasksForBuildingBlock, _task);
      }
   }

   public class When_removing_molecule_building_block_that_is_referred_to_in_another_building_block : concern_for_InteractionTasksForMoleculeBuildingBlock
   {
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private Module _module;

      protected override void Context()
      {
         base.Context();
         _moleculeBuildingBlock = new MoleculeBuildingBlock {Id = "1"};
         _module = new Module
         {
            new InitialConditionsBuildingBlock { MoleculeBuildingBlockId = _moleculeBuildingBlock.Id, SpatialStructureId = "" },
            new InitialConditionsBuildingBlock { MoleculeBuildingBlockId = _moleculeBuildingBlock.Id }
         };
      }

      [Observation]
      public void should_throw_MoBi_exception()
      {
         The.Action(() => sut.Remove(_moleculeBuildingBlock, _module, null, false)).ShouldThrowAn<MoBiException>();
      }
   }
}