using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Exceptions;
using MoBi.Core.Helper;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForMoleculeBuildingBlock : IInteractionTasksForBuildingBlock<MoleculeBuildingBlock>
   {
      void CreateNewFromSelection();
      void Edit(MoleculeBuildingBlock moleculeBuildingBlock, IMoleculeBuilder moleculeBuilder);
   }

   public class InteractionTasksForMoleculeBuildingBlock : InteractionTaskForCloneMergeBuildingBlock<MoleculeBuildingBlock, IMoleculeBuilder>, IInteractionTasksForMoleculeBuildingBlock
   {
      private readonly IEditTasksForBuildingBlock<MoleculeBuildingBlock> _editTaskForBuildingBlock;

      public InteractionTasksForMoleculeBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<MoleculeBuildingBlock> editTask,
         IInteractionTasksForBuilder<IMoleculeBuilder> builderTask,
         IMoleculeBuildingBlockCloneManager moleculeBuildingBlockCloneManager)
         : base(interactionTaskContext, editTask, builderTask, moleculeBuildingBlockCloneManager)
      {
         _editTaskForBuildingBlock = editTask;
      }

      public void CreateNewFromSelection()
      {
         var allMolecules = Context.CurrentProject.MoleculeBlockCollection;
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
         var cloneManagerForBuildingBlocks = new CloneManagerForBuildingBlock(Context.ObjectBaseFactory, new DataRepositoryTask()) {FormulaCache = moleculeBuildingBlock.FormulaCache};
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

      public override IMoBiCommand Remove(MoleculeBuildingBlock buildingBlockToRemove, IMoBiProject project, IBuildingBlock buildingBlock, bool silent)
      {
         var referringStartValuesBuildingBlocks = project.ReferringStartValuesBuildingBlocks(buildingBlockToRemove);
         if (referringStartValuesBuildingBlocks.Any())
         {
            throw new MoBiException(AppConstants.CannotRemoveBuildingBlockFromProject(buildingBlockToRemove.Name, referringStartValuesBuildingBlocks.Select(bb => bb.Name)));
         }

         return base.Remove(buildingBlockToRemove, project, buildingBlock, silent);
      }

      public void Edit(MoleculeBuildingBlock moleculeBuildingBlock, IMoleculeBuilder moleculeBuilder)
      {
         _editTaskForBuildingBlock.EditBuildingBlock(moleculeBuildingBlock);
         Context.PublishEvent(new EntitySelectedEvent(moleculeBuilder, this));
      }

      protected override IMoBiMacroCommand GenerateAddCommandAndUpdateFormulaReferences(IMoleculeBuilder builder, MoleculeBuildingBlock targetBuildingBlock, string originalBuilderName = null)
      {
         var defaultStartFormulaDecoder = new DefaultStartFormulaDecoder();
         var macroCommand = base.GenerateAddCommandAndUpdateFormulaReferences(builder, targetBuildingBlock);
         macroCommand.Add(_interactionTaskContext.MoBiFormulaTask.AddFormulaToCacheOrFixReferenceCommand(targetBuildingBlock, builder, defaultStartFormulaDecoder));

         return macroCommand;
      }
   }
}