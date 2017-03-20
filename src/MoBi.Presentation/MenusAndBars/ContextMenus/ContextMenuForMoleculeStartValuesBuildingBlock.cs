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

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForMoleculeStartValuesBuildingBlock : ContextMenuForMergableBuildingBlock<IMoleculeStartValuesBuildingBlock>
   {
      public ContextMenuForMoleculeStartValuesBuildingBlock(IMoBiContext context, IObjectTypeResolver objectTypeResolver) : base(context, objectTypeResolver)
      {
      }

      public override IContextMenu InitializeWith(IObjectBaseDTO dto, IPresenter presenter)
      {
         var buildingBlock = _context.Get<IMoleculeStartValuesBuildingBlock>(dto.Id);
         base.InitializeWith(dto, presenter);

         _allMenuItems.Add(CreateMenuButton.WithCaption(AppConstants.MenuNames.Import)
            .WithIcon(ApplicationIcons.MoleculeStartValuesImport)
            .WithCommandFor<ImportMoleculeStartValuesUICommand, IBuildingBlock>(buildingBlock));

         return this;
      }
   }
}