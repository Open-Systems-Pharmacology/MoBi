using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuSpecificationFactoryForActiveTransportBuilderContainer:IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return IoC.Resolve<IContextMenuForTransporterMoleculeContainer>().InitializeWith(viewItem as TransporterMoleculeContainerDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<TransporterMoleculeContainerDTO>();
      }
   }

   internal interface IContextMenuForTransporterMoleculeContainer : IContextMenuFor<TransporterMoleculeContainer>
   {
   }
   internal class ContextMenuForTransporterMoleculeContainer:ContextMenuBase, IContextMenuForTransporterMoleculeContainer
   {
      private IList<IMenuBarItem> _allMenuItems;
      private readonly IMoBiContext _context;

      public ContextMenuForTransporterMoleculeContainer(IMoBiContext context) 
      {
         _context = context;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var activeTransportBuilderContainer = _context.Get<TransporterMoleculeContainer>(dto.Id);
         _allMenuItems = new List<IMenuBarItem>
                            {
                               CreateEditItemFor(activeTransportBuilderContainer),
                               CreateRenameItemFor(activeTransportBuilderContainer),
                               CreateAddNewTransportFor(activeTransportBuilderContainer),
                               CreateAddExistingTransportFor(activeTransportBuilderContainer),
                               CreateAddExistingFromTemplateTransportFor(activeTransportBuilderContainer),
                               CreateRemoveItemFor(activeTransportBuilderContainer)
                            };
         return this;
      }

      protected IMenuBarItem CreateEditItemFor(TransporterMoleculeContainer objectToEdit)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Edit)
            .WithCommandFor<EditCommandFor<TransporterMoleculeContainer>, TransporterMoleculeContainer>(objectToEdit)
            .WithIcon(ApplicationIcons.Edit);
      }

      protected IMenuBarItem CreateRenameItemFor(TransporterMoleculeContainer objectToEdit)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Rename)
            .WithCommandFor<RenameObjectCommand<TransporterMoleculeContainer>, TransporterMoleculeContainer>(objectToEdit)
            .WithIcon(ApplicationIcons.Rename);
      }

      protected virtual IMenuBarItem CreateRemoveItemFor(TransporterMoleculeContainer objectToEdit)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithRemoveCommand(objectToEdit.ParentContainer.DowncastTo<IMoleculeBuilder>(),objectToEdit)
            .WithIcon(ApplicationIcons.Delete);
      }
      protected virtual IMenuBarItem CreateAddNewTransportFor(TransporterMoleculeContainer selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew("Transport"))
            .WithCommandFor<AddNewCommandFor<TransporterMoleculeContainer, ITransportBuilder>, TransporterMoleculeContainer>(selectedObject)
            .WithIcon(ApplicationIcons.Add);
      }

      protected virtual IMenuBarItem CreateAddExistingTransportFor(TransporterMoleculeContainer selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting("Transport"))
            .WithCommandFor<AddExistingCommandFor<TransporterMoleculeContainer, ITransportBuilder>, TransporterMoleculeContainer>(selectedObject)
            .WithIcon(ApplicationIcons.PKMLLoad);
      }

      protected virtual IMenuBarItem CreateAddExistingFromTemplateTransportFor(TransporterMoleculeContainer selectedObject)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExistingFromTemplate("Transport"))
            .WithCommandFor<AddExistingFromTemplateCommandFor<TransporterMoleculeContainer, ITransportBuilder>, TransporterMoleculeContainer>(selectedObject)
            .WithIcon(ApplicationIcons.LoadFromTemplate);
      }
   }
}