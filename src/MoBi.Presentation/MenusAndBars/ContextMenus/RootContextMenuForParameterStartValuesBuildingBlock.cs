using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForParameterStartValuesBuildingBlock : RootContextMenuFor<IMoBiProject, IParameterStartValuesBuildingBlock>
   {
      public RootContextMenuForParameterStartValuesBuildingBlock(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      protected override void CreateAddItems(IMoBiProject parent)
      {
         base.CreateAddItems(parent);
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.ExpressionProfileBuildingBlock)).AsGroupStarter()
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(ExpressionProfileBuildingBlock)))
            .WithCommandFor<AddExpressionAsParameterStartValuesCommand, IMoBiProject>(parent));
      }
   }
}