using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForIndividualBuildingBlock : ContextMenuForBuildingBlock<IndividualBuildingBlock>
   {
      public ContextMenuForIndividualBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver) : base(context, objectTypeResolver)
      {
      }
   }
}