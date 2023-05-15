using System;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Repository;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForMoleculeBuildingBlock : IInteractionTasksForBuildingBlock<Module, MoleculeBuildingBlock>
   {
      void CreateNewFromSelection();
      void Edit(MoleculeBuildingBlock moleculeBuildingBlock, MoleculeBuilder moleculeBuilder);
   }

   public class InteractionTasksForMoleculeBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<Module, MoleculeBuildingBlock, MoleculeBuilder>, IInteractionTasksForMoleculeBuildingBlock
   {
      private readonly IEditTasksForBuildingBlock<MoleculeBuildingBlock> _editTaskForBuildingBlock;

      public InteractionTasksForMoleculeBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<MoleculeBuildingBlock> editTask,
         IInteractionTasksForBuilder<MoleculeBuilder> builderTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
         _editTaskForBuildingBlock = editTask;
      }

      public void CreateNewFromSelection()
      {
         var allMolecules = BuildingBlockRepository.MoleculeBlockCollection;
         NewMoleculeBuildingBlockDescription newMoleculeBuildingBlockDescription = null;
         using (var selectMoleculesPresenter = ApplicationController.Start<ISelectMoleculesForBuildingBlockPresenter>())
         {
            selectMoleculesPresenter.MoleculeBuildinBlocks = allMolecules;
            if (selectMoleculesPresenter.AskForCreation())
            {
               newMoleculeBuildingBlockDescription = selectMoleculesPresenter.Selected;
            }
         }

         if (newMoleculeBuildingBlockDescription == null)
            return;

         var moleculeBuildingBlock = Context.Create<MoleculeBuildingBlock>().WithName(newMoleculeBuildingBlockDescription.Name);
         var cloneManagerForBuildingBlocks = new CloneManagerForBuildingBlock(Context.ObjectBaseFactory, new DataRepositoryTask()) { FormulaCache = moleculeBuildingBlock.FormulaCache };
         newMoleculeBuildingBlockDescription.Molecules.Each(x => moleculeBuildingBlock.Add(cloneManagerForBuildingBlocks.Clone(x)));
         var command = new MoBiMacroCommand
         {
            CommandType = AppConstants.Commands.AddCommand,
            ObjectType = ObjectName,
            Description = AppConstants.Commands.CreateFromSelectionDescription(moleculeBuildingBlock.Name, moleculeBuildingBlock.Select(x => x.Name).ToList())
         };
         command.AddCommand(GetAddCommand(moleculeBuildingBlock, null, null).Run(Context));
         AddCommand(command);
      }

      public override IMoBiCommand Remove(MoleculeBuildingBlock buildingBlockToRemove, Module module, IBuildingBlock buildingBlock, bool silent)
      {
         var referringStartValuesBuildingBlocks = module.ReferringStartValueBuildingBlocks(buildingBlockToRemove);
         if (referringStartValuesBuildingBlocks.Any())
         {
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromModule(buildingBlockToRemove.Name, referringStartValuesBuildingBlocks.Select(bb => bb.Name)));
         }
         
         return base.Remove(buildingBlockToRemove, module, buildingBlock, silent);
      }

      public override IMoBiCommand GetRemoveCommand(MoleculeBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<MoleculeBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(MoleculeBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<MoleculeBuildingBlock>(itemToAdd, parent);
      }

      public void Edit(MoleculeBuildingBlock moleculeBuildingBlock, MoleculeBuilder moleculeBuilder)
      {
         _editTaskForBuildingBlock.EditBuildingBlock(moleculeBuildingBlock);
         Context.PublishEvent(new EntitySelectedEvent(moleculeBuilder, this));
      }
   }
}