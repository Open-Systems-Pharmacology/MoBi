using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForExpressionProfileBuildingBlock : ContextMenuForBuildingBlock<ExpressionProfileBuildingBlock>
   {
      public ContextMenuForExpressionProfileBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver) : base(context, objectTypeResolver)
      {
      }
   }
}