using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForApplicationMoleculeBuilder : InteractionTasksForChildren<ApplicationBuilder, ApplicationMoleculeBuilder>
   {
      private readonly IIdGenerator _idGenerator;

      public InteractionTasksForApplicationMoleculeBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ApplicationMoleculeBuilder> editTask, IIdGenerator idGenerator)
         : base(interactionTaskContext, editTask)
      {
         _idGenerator = idGenerator;
      }

      public override ApplicationMoleculeBuilder CreateNewEntity(ApplicationBuilder applicationBuilder)
      {
         return base.CreateNewEntity(applicationBuilder).WithName(_idGenerator.NewId());
      }

      public override IMoBiCommand GetRemoveCommand(ApplicationMoleculeBuilder transportBuilderToRemove, ApplicationBuilder applicationBuilder, IBuildingBlock buildingBlock)
      {
         return new RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand(applicationBuilder, transportBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(ApplicationMoleculeBuilder applicationMoleculeBuilder, ApplicationBuilder applicationBuilder, IBuildingBlock buildingBlock)
      {
         return new AddApplicationMoleculBuilderToApplicationBuilderCommand(applicationBuilder, applicationMoleculeBuilder, buildingBlock);
      }
   }
}