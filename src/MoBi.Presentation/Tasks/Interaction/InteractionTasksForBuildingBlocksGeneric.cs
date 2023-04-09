using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Edit;
using NPOI.POIFS.Properties;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForBuildingBlocks : IInteractionTasksForChildren<Module, ObserverBuildingBlock>
   {
      void LoadBuildingBlocksToModule(Module module);
      
   }

   public class InteractionTasksForBuildingBlocks : InteractionTasksForChildren<Module, ObserverBuildingBlock>, IInteractionTasksForBuildingBlocks
   {
   public InteractionTasksForBuildingBlocks(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<ObserverBuildingBlock> editTask) : base(
      interactionTaskContext,
      editTask)
   {
   }

   public IMoBiCommand GetAddBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks)
   {
      return new AddMultipleBuildingBlocksToModuleCommand(existingModule, listOfNewBuildingBlocks);
   }



   public void AddBuildingBlocksToModule(Module module)
   {
      using (var presenter = _interactionTaskContext.ApplicationController.Start<IAddBuildingBlocksToModulePresenter>())
      {
         var listOfNewBuildingBlocks = presenter.AddBuildingBlocksToModule(module);

         if (!listOfNewBuildingBlocks.Any())
            return;

         _interactionTaskContext.Context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, listOfNewBuildingBlocks)
            .Run(_interactionTaskContext.Context));
      }
   }

   public void LoadBuildingBlocksToModule(Module module)
   {
      var filename = AskForPKMLFileToOpen();

      if (filename.IsNullOrEmpty())
         return; // new MoBiEmptyCommand();

      var items = LoadItems(filename);

      //doing now just for reaction as a test
      _interactionTaskContext.Context.AddToHistory(GetAddBuildingBlocksToModuleCommand(module, LoadItems(filename).ToList())
         .Run(_interactionTaskContext.Context));

   }

   //not sure what to do with those here underneath

   public override IMoBiCommand GetRemoveCommand(ObserverBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
   {
      throw new System.NotImplementedException();
   }

   public override IMoBiCommand GetAddCommand(ObserverBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
   {
      throw new System.NotImplementedException();
   }
   }
}