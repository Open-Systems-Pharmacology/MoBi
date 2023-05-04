using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal class ContextMenuForParameterStartValuesFolder : ContextMenuBase, IContextMenuFor<ModuleViewItem>
   {
      private readonly IContainer _container;
      private readonly List<IMenuBarItem> _allMenuItems;

      public ContextMenuForParameterStartValuesFolder(IContainer container)
      {
         _container = container;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         return _allMenuItems;
      }

      public IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         var psvCollectionViewItem = dto.DowncastTo<ModuleViewItem>();
         var module = psvCollectionViewItem.Module;
         _allMenuItems.Add(createAddExpressionAsStartValue(module));
         return this;
      }

      private IMenuBarItem createAddExpressionAsStartValue(Module module)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.ExpressionProfileBuildingBlock)).AsGroupStarter()
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(ExpressionProfileBuildingBlock)))
            .WithCommandFor<AddExpressionAsParameterStartValuesCommand, Module>(module, _container);
      }
   }

   public class ContextMenuSpecificationFactoryForModuleParameterStartValuesCollection : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForModuleParameterStartValuesCollection(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForParameterStartValuesFolder(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<ModuleViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         if (!(viewItem is ModuleViewItem moduleViewItem))
            return false;

         return moduleViewItem.TargetAsObject.IsAnImplementationOf<IEnumerable<ParameterStartValuesBuildingBlock>>();
      }
   }
}