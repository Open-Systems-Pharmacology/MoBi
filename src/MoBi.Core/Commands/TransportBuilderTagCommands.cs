using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddSourceMatchTagConditionToTransportBuilderCommand : AddMatchTagConditionCommandBase<ITransportBuilder>
   {
      public AddSourceMatchTagConditionToTransportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.SourceCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveSourceMatchTagConditionFromTranportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class AddSourceNotMatchTagConditionToTransportBuilderCommand : AddNotMatchTagConditionCommandBase<ITransportBuilder>
   {
      public AddSourceNotMatchTagConditionToTransportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.SourceCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveSourceNotMatchTagConditionFromTranportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class AddTargetMatchTagConditionToTransportBuilderCommand : AddMatchTagConditionCommandBase<ITransportBuilder>
   {
      public AddTargetMatchTagConditionToTransportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.TargetCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTargetMatchTagConditionFromTranportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class AddTargetNotMatchTagConditionToTransportBuilderCommand : AddNotMatchTagConditionCommandBase<ITransportBuilder>
   {
      public AddTargetNotMatchTagConditionToTransportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.TargetCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTargetNotMatchTagConditionFromTransportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveSourceMatchTagConditionFromTranportBuilderCommand : RemoveMatchTagConditionCommandBase<ITransportBuilder>
   {
      public RemoveSourceMatchTagConditionFromTranportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.SourceCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddSourceMatchTagConditionToTransportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveSourceNotMatchTagConditionFromTranportBuilderCommand : RemoveNotMatchTagConditionCommandBase<ITransportBuilder>
   {
      public RemoveSourceNotMatchTagConditionFromTranportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.SourceCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddSourceNotMatchTagConditionToTransportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveTargetMatchTagConditionFromTranportBuilderCommand : RemoveMatchTagConditionCommandBase<ITransportBuilder>
   {
      public RemoveTargetMatchTagConditionFromTranportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.TargetCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddTargetMatchTagConditionToTransportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveTargetNotMatchTagConditionFromTransportBuilderCommand : RemoveNotMatchTagConditionCommandBase<ITransportBuilder>
   {
      public RemoveTargetNotMatchTagConditionFromTransportBuilderCommand(string tag, ITransportBuilder transportBuilder, IBuildingBlock buildingBlock)
         : base(tag, transportBuilder, buildingBlock, x => x.TargetCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddTargetNotMatchTagConditionToTransportBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }
}