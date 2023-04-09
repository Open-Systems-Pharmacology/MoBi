using System.Collections.Generic;
using System.Linq;
using DevExpress.Data.Helpers;
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

/*
namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForBuildingBlocksGeneric<TModule, TBuildingBlock> //: IInteractionTasksForChildren<Module, T>  where T: class, IBuildingBlock
      where TBuildingBlock : class, IBuildingBlock
      where TModule : Module
   {
      void LoadBuildingBlocksToModule(Module module);
      
   }

   public class InteractionTasksForBuildingBlocksGeneric<TModule, TBuildingBlock> : InteractionTasksForChildren<TModule, TBuildingBlock> , IInteractionTasksForBuildingBlocksGeneric<TModule, TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock
      where TModule : Module
   {
   public InteractionTasksForBuildingBlocksGeneric(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask) : base(
      interactionTaskContext,
      editTask)
   {
   }

   public IMoBiCommand GetAddBuildingBlocksToModuleCommand(Module existingModule, IReadOnlyList<IBuildingBlock> listOfNewBuildingBlocks)
   {
      return new AddMultipleBuildingBlocksToModuleCommand(existingModule, listOfNewBuildingBlocks);
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

   public override IMoBiCommand GetRemoveCommand(TBuildingBlock objectToRemove, TModule parent, IBuildingBlock buildingBlock)
   {
      throw new System.NotImplementedException();
   }

   public override IMoBiCommand GetAddCommand(TBuildingBlock itemToAdd, TModule parent, IBuildingBlock buildingBlock)
   {
      throw new System.NotImplementedException();
   }
   }
}
*/