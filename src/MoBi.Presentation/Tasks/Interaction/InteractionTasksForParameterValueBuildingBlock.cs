using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForParameterValueBuildingBlock : IInteractionTasksForProjectBuildingBlock<ParameterValuesBuildingBlock>,
      IInteractionTasksForProjectPathAndValueEntityBuildingBlocks<ParameterValuesBuildingBlock, ParameterValue>,
      IInteractionTasksForProjectBuildingBlock
   {
   }

   public class InteractionTasksForParameterValueBuildingBlock : InteractionTasksForProjectPathAndValueEntityBuildingBlocks<IndividualBuildingBlock, IndividualParameter>, IInteractionTasksForIndividualBuildingBlock
   {
      public InteractionTasksForParameterValueBuildingBlock(IInteractionTaskContext interactionTaskContext,
         IEditTasksForIndividualBuildingBlock editTask,
         IMoBiFormulaTask moBiFormulaTask,
         IParameterFactory parameterFactory,
         IDialogCreator dialogCreator) :
         base(interactionTaskContext, editTask, moBiFormulaTask, parameterFactory, dialogCreator)
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