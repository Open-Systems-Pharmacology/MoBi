using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForIndividualBuildingBlock : IInteractionTasksForProjectBuildingBlock<IndividualBuildingBlock>,
      IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<IndividualBuildingBlock, IndividualParameter>,
      IInteractionTasksForProjectBuildingBlock
   {
   }

   public class InteractionTasksForIndividualBuildingBlock : InteractionTasksForProjectPathAndValueEntityBuildingBlocks<IndividualBuildingBlock, IndividualParameter>, IInteractionTasksForIndividualBuildingBlock
   {
      public InteractionTasksForIndividualBuildingBlock(IInteractionTaskContext interactionTaskContext, 
         IEditTasksForIndividualBuildingBlock editTask, 
         IMoBiFormulaTask moBiFormulaTask, 
         IParameterFactory parameterFactory) : 
         base(interactionTaskContext, editTask, moBiFormulaTask, parameterFactory)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IndividualBuildingBlock individualBuildingBlockToRemove, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new RemoveIndividualBuildingBlockFromProjectCommand(individualBuildingBlockToRemove);
      }

      public override IMoBiCommand GetAddCommand(IndividualBuildingBlock individualBuildingBlockToAdd, MoBiProject parent, IBuildingBlock buildingBlock)
      {
         return new AddIndividualBuildingBlockToProjectCommand(individualBuildingBlockToAdd);
      }
   }
}