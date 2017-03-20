using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForApplicationMoleculeBuilder : InteractionTasksForChildren<IApplicationBuilder, IApplicationMoleculeBuilder>
   {
      private readonly IIdGenerator _idGenerator;

      public InteractionTasksForApplicationMoleculeBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IApplicationMoleculeBuilder> editTask, IIdGenerator idGenerator)
         : base(interactionTaskContext, editTask)
      {
         _idGenerator = idGenerator;
      }

      public override IApplicationMoleculeBuilder CreateNewEntity(IApplicationBuilder applicationBuilder)
      {
         return base.CreateNewEntity(applicationBuilder).WithName(_idGenerator.NewId());
      }

      public override IMoBiCommand GetRemoveCommand(IApplicationMoleculeBuilder transportBuilderToRemove, IApplicationBuilder applicationBuilder, IBuildingBlock buildingBlock)
      {
         return new RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand(applicationBuilder, transportBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IApplicationMoleculeBuilder applicationMoleculeBuilder, IApplicationBuilder applicationBuilder, IBuildingBlock buildingBlock)
      {
         return new AddApplicationMoleculBuilderToApplicationBuilderCommand(applicationBuilder, applicationMoleculeBuilder, buildingBlock);
      }
   }
}