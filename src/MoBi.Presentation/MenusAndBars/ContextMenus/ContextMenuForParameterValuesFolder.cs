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
   internal class ContextMenuForParameterValuesFolder : ContextMenuForModuleBuildingBlockCollection
   {
      private readonly IContainer _container;

      public ContextMenuForParameterValuesFolder(IContainer container)
      {
         _container = container;
         _allMenuItems = new List<IMenuBarItem>();
      }

      public override IContextMenu InitializeWith(ObjectBaseDTO dto, IPresenter presenter)
      {
         base.InitializeWith(dto, presenter);
         _allMenuItems.Add(createAddExpressionAsParameterValueBuildingBlock(ModuleFor(dto)));
         return this;
      }

      protected override IMenuBarItem AddNewBuildingBlock(Module module)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.ParameterValuesBuildingBlock))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(ParameterValuesBuildingBlock)))
            .WithCommandFor<AddNewParameterValuesBuildingBlockUICommand, Module>(module, _container);
      }

      private IMenuBarItem createAddExpressionAsParameterValueBuildingBlock(Module module)
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.ExpressionProfileBuildingBlock))
            .WithIcon(ApplicationIcons.LoadIconFor(nameof(ExpressionProfileBuildingBlock)))
            .WithCommandFor<AddExpressionAsParameterValuesCommand, Module>(module, _container);
      }
   }

   public class ContextMenuSpecificationFactoryForModuleParameterValuesCollection : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;

      public ContextMenuSpecificationFactoryForModuleParameterValuesCollection(IContainer container)
      {
         _container = container;
      }

      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = new ContextMenuForParameterValuesFolder(_container);
         return contextMenu.InitializeWith(viewItem.DowncastTo<ModuleViewItem>(), presenter);
      }

      public bool IsSatisfiedBy(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         if (!(viewItem is ModuleViewItem moduleViewItem))
            return false;

         return moduleViewItem.TargetAsObject.IsAnImplementationOf<IEnumerable<ParameterValuesBuildingBlock>>();
      }
   }
}