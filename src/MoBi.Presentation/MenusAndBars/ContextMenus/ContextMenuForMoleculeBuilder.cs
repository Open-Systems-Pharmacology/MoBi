using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuSpecificationFactoryForMoleculeBuilder : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return IoC.Resolve<IContextMenuForMoleculeBuilder>().InitializeWith(viewItem as MoleculeBuilderDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return (objectRequestingContextMenu == null && presenter.IsAnImplementationOf<IMoleculeListPresenter>()) ||
                objectRequestingContextMenu.IsAnImplementationOf<MoleculeBuilderDTO>();
      }
   }

   public interface IContextMenuForMoleculeBuilder : IContextMenuFor<IMoleculeBuilder>
   {
   }

   public class ContextMenuForMoleculeBuilder : ContextMenuBase, IContextMenuForMoleculeBuilder
   {
      private IList<IMenuBarItem> _allMolecules;
      private readonly IMoBiContext _context;

      public ContextMenuForMoleculeBuilder(IMoBiContext context)
      {
         _context = context;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMolecules;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var listPresenter = presenter.DowncastTo<IMoleculeListPresenter>();
         var moleculeBuildingBlock = listPresenter.MoleculeBuildingBlock;
         if (dto == null)
         {
            _allMolecules = new List<IMenuBarItem>
            {
               createAddNewMoleculeBuilder(moleculeBuildingBlock),
               createAddExistingMoleculeBuilder(moleculeBuildingBlock),
               createAddExistingMoleculeBuilderFromTemplate(moleculeBuildingBlock),
               createAddPKSimMoleculeFromTemplate(moleculeBuildingBlock),
            };
            return this;
         }

         var moleculeBuilder = _context.Get<IMoleculeBuilder>(dto.Id);
         _allMolecules = new List<IMenuBarItem>
         {
            createEditItemFor(moleculeBuilder),
            createRenameItemFor(moleculeBuilder),
            createAddNewTransporterFor(moleculeBuilder),
            createAddExistingTransporterFor(moleculeBuilder),
            createAddExistingFromTemplateTransporterFor(moleculeBuilder),
            createAddNewInteractionContainerFor(moleculeBuilder),
            createAddExistingInteractionContainerFor(moleculeBuilder),
            createAddExistingFromTemplateInteractionContainerFor(moleculeBuilder),
            createSaveItemFor(moleculeBuilder),
            createRemoveItemFor(moleculeBuildingBlock, moleculeBuilder)
         };

         return this;
      }

      private IMenuBarItem createAddNewInteractionContainerFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.InteractionContainer))
            .WithCommandFor<AddNewCommandFor<IMoleculeBuilder, InteractionContainer>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.Add);
      }
      
      private IMenuBarItem createAddExistingInteractionContainerFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(
            AppConstants.MenuNames.AddExisting(ObjectTypes.InteractionContainer))
            .WithCommandFor<AddExistingCommandFor<IMoleculeBuilder, InteractionContainer>, IMoleculeBuilder>(
               moleculeBuilder)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      private IMenuBarItem createAddExistingFromTemplateInteractionContainerFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.InteractionContainer))
            .WithCommandFor<AddExistingFromTemplateCommandFor<IMoleculeBuilder, InteractionContainer>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }
      private IMenuBarItem createEditItemFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditCommandFor<IMoleculeBuilder>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.Edit);
      }

      private IMenuBarItem createRenameItemFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameObjectCommand<IMoleculeBuilder>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.Rename);
      }

      private IMenuBarItem createRemoveItemFor(IMoleculeBuildingBlock moleculeBuildingBlock, IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithRemoveCommand(moleculeBuildingBlock, moleculeBuilder)
            .WithIcon(ApplicationIcons.Delete);
      }

      private IMenuBarItem createAddNewTransporterFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.TransporterMoleculeContainer))
            .WithCommandFor<AddNewCommandFor<IMoleculeBuilder, TransporterMoleculeContainer>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.Add);
      }

      private IMenuBarItem createAddExistingTransporterFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.TransporterMoleculeContainer))
            .WithCommandFor<AddExistingCommandFor<IMoleculeBuilder, TransporterMoleculeContainer>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      private IMenuBarItem createAddExistingFromTemplateTransporterFor(IMoleculeBuilder moleculeBuilder)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.TransporterMoleculeContainer))
            .WithCommandFor<AddExistingFromTemplateCommandFor<IMoleculeBuilder, TransporterMoleculeContainer>, IMoleculeBuilder>(moleculeBuilder)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }
      
      private IMenuBarItem createAddNewMoleculeBuilder(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Molecule))
            .WithCommandFor<AddNewCommandFor<IMoleculeBuildingBlock, IMoleculeBuilder>, IMoleculeBuildingBlock>(moleculeBuildingBlock)
            .WithIcon(ApplicationIcons.MoleculeAdd);
      }

      private IMenuBarItem createAddPKSimMoleculeFromTemplate(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddPKSimMolecule)
            .WithCommandFor<AddPKSimMoleculeCommand, IMoleculeBuildingBlock>(moleculeBuildingBlock)
            .WithIcon(ApplicationIcons.PKSimMoleculeAdd);
      }

      private IMenuBarItem createAddExistingMoleculeBuilder(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Molecule))
            .WithCommandFor<AddExistingCommandFor<IMoleculeBuildingBlock, IMoleculeBuilder>, IMoleculeBuildingBlock>(moleculeBuildingBlock)
            .WithIcon(ApplicationIcons.MoleculeLoad);
      }

      private IMenuBarItem createAddExistingMoleculeBuilderFromTemplate(IMoleculeBuildingBlock moleculeBuildingBlock)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate(ObjectTypes.Molecule))
            .WithCommandFor<AddExistingFromTemplateCommandFor<IMoleculeBuildingBlock, IMoleculeBuilder>, IMoleculeBuildingBlock>(moleculeBuildingBlock)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }

      private IMenuBarItem createSaveItemFor(IMoleculeBuilder selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.SaveAsPKML)
            .WithCommandFor<SaveUICommandFor<IMoleculeBuilder>, IMoleculeBuilder>(selectedObject)
            .WithIcon(ApplicationIcons.SaveMolecule);
      }
   }
}