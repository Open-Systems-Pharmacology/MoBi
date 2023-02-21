using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForParameterStartValuesBuildingBlock : ContextMenuForMergableBuildingBlock<IParameterStartValuesBuildingBlock>
   {
      public ContextMenuForParameterStartValuesBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver, IContainer container) : base(context, objectTypeResolver, container)
      {
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<IParameterStartValuesBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Import)
            .WithIcon(ApplicationIcons.ParameterStartValuesImport)
            .WithCommandFor<ImportParameterStartValuesUICommand, IBuildingBlock>(buildingBlock, _container));

         return this;
      }
   }
}