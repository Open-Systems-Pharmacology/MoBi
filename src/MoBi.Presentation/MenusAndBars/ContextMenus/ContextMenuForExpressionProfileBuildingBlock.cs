using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForExpressionProfileBuildingBlock : ContextMenuForBuildingBlock<ExpressionProfileBuildingBlock>
   {
      public ContextMenuForExpressionProfileBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver) : base(context, objectTypeResolver)
      {
      }

      public override IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter)
      {
         base.InitializeWith(dto, presenter);
         
         return this;
      }
   }
}