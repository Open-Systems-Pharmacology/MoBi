using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddSourceMatchTagConditionToTransportBuilderCommand : AddMatchTagConditionCommand<ITransportBuilder>
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

   public class AddSourceNotMatchTagConditionToTransportBuilderCommand : AddNotMatchTagConditionCommand<ITransportBuilder>
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

   public class AddTargetMatchTagConditionToTransportBuilderCommand : AddMatchTagConditionCommand<ITransportBuilder>
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

   public class AddTargetNotMatchTagConditionToTransportBuilderCommand : AddNotMatchTagConditionCommand<ITransportBuilder>
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

   public class RemoveSourceMatchTagConditionFromTranportBuilderCommand : RemoveMatchTagConditionCommand<ITransportBuilder>
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

   public class RemoveSourceNotMatchTagConditionFromTranportBuilderCommand : RemoveNotMatchTagConditionCommand<ITransportBuilder>
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

   public class RemoveTargetMatchTagConditionFromTranportBuilderCommand : RemoveMatchTagConditionCommand<ITransportBuilder>
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

   public class RemoveTargetNotMatchTagConditionFromTransportBuilderCommand : RemoveNotMatchTagConditionCommand<ITransportBuilder>
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