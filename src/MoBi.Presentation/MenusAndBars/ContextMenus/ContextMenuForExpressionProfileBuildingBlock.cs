using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForExpressionProfileBuildingBlock : ContextMenuForProjectBuildingBlock<ExpressionProfileBuildingBlock>
   {
      public ContextMenuForExpressionProfileBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }
   }
}