using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using OSPSuite.Utility.Container;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForSpatialStructure : ContextMenuBase
   {
      private readonly IContainer _container;
      private readonly MoBiSpatialStructure _spatialStructure;

      public RootContextMenuForSpatialStructure(IContainer container, MoBiSpatialStructure spatialStructure)
      {
         _container = container;
         _spatialStructure = spatialStructure;
      }
      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew("Top Container"))
            .WithIcon(ApplicationIcons.ContainerAdd)
            .WithCommand<AddNewTopContainerCommand>(_container);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting("Top Container"))
            .WithIcon(ApplicationIcons.ContainerLoad)
            .WithCommand<AddExistingTopContainerCommand>(_container);

         if (_spatialStructure?.GlobalMoleculeDependentProperties == null)
            yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddMoleculeProperties)
               .WithIcon(ApplicationIcons.Add)
               .WithCommand<AddGlobalMoleculePropertiesUICommand>(_container);
      }
   }

   public class RootContextMenuForSpatialStructureFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IContainer _container;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public RootContextMenuForSpatialStructureFactory(IContainer container, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _container = container;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new RootContextMenuForSpatialStructure(_container, _activeSubjectRetriever.Active<MoBiSpatialStructure>());
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<SpatialStructureRootItem>() && presenter.IsAnImplementationOf<IHierarchicalSpatialStructurePresenter>();
      }
   }
}