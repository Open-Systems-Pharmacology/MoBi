using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.SimModel;

namespace MoBi.Core.Commands
{
   public class RemoveIndividualBuildingBlockFromProjectCommand : RemoveBuildingBlockFromProjectCommand<IndividualBuildingBlock>
   {
      public RemoveIndividualBuildingBlockFromProjectCommand(IndividualBuildingBlock buildingBlock) : base(buildingBlock)
      {
      }

      protected override void RemoveFromProject(MoBiProject project)
      {
         project.RemoveIndividualBuildingBlock(_buildingBlock);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddIndividualBuildingBlockToProjectCommand(_buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveExpressionProfileBuildingBlockFromProjectCommand : RemoveBuildingBlockFromProjectCommand<ExpressionProfileBuildingBlock>
   {
      public RemoveExpressionProfileBuildingBlockFromProjectCommand(ExpressionProfileBuildingBlock buildingBlock) : base(buildingBlock)
      {
      }

      protected override void RemoveFromProject(MoBiProject project)
      {
         project.RemoveExpressionProfileBuildingBlock(_buildingBlock);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddExpressionProfileBuildingBlockToProjectCommand(_buildingBlock).AsInverseFor(this);
      }
   }

   public abstract class RemoveBuildingBlockFromProjectCommand<T> : MoBiReversibleCommand where T : class, IBuildingBlock
   {
      protected T _buildingBlock;
      private byte[] _serializationStream;

      protected RemoveBuildingBlockFromProjectCommand(T buildingBlock)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.DeleteCommand;
         Description = AppConstants.Commands.RemoveFromProjectDescription(ObjectType, buildingBlock.Name);
         _buildingBlock = buildingBlock;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         RemoveFromProject(project);
         context.Unregister(_buildingBlock);
         _serializationStream = context.Serialize(_buildingBlock);
         context.PublishEvent(new RemovedEvent(_buildingBlock,project));
      }

      protected abstract void RemoveFromProject(MoBiProject project);

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Deserialize<T>(_serializationStream);
      }
      
      protected override void ClearReferences()
      {
         _buildingBlock = null;
      }
   }
}