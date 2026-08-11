using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleObservedDataNodeContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableObservedData>
   {
      private readonly IContainer _container;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public MultipleObservedDataNodeContextMenuFactory(IContainer container, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _container = container;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableObservedData> observedData, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleObservedDataNodeContextMenu(observedData.Select(x => x.Repository).ToList(), _activeSubjectRetriever.Active<IMoBiSimulation>(), _container);
      }
   }

   public class MultipleObservedDataNodeContextMenu : ContextMenu<IReadOnlyList<DataRepository>, IMoBiSimulation>
   {
      public MultipleObservedDataNodeContextMenu(IReadOnlyList<DataRepository> observedData, IMoBiSimulation activeSimulation, IContainer container) : base(observedData, activeSimulation, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<DataRepository> observedData, IMoBiSimulation activeSimulation)
      {
         if (activeSimulation == null)
            yield break;

         if (observedData.All(activeSimulation.UsesObservedData))
            yield break;

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddToSimulation(activeSimulation.Name))
            .WithCommand(_container.Resolve<AddObservedDataToSimulationUICommand>().For(observedData).For(activeSimulation))
            .WithIcon(ApplicationIcons.Simulation);
      }
   }
}
