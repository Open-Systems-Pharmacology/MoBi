using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleSimulationNodeContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableSimulation>
   {
      private readonly IMoBiContext _context;
      private readonly IContainer _container;

      public MultipleSimulationNodeContextMenuFactory(IMoBiContext context, IContainer container)
      {
         _context = context;
         _container = container;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableSimulation> classifiableSimulations, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleSimulationNodeContextMenu(classifiableSimulations.Select(x => x.Subject).ToList(), _context, _container);
      }
   }

   public class MultipleSimulationNodeContextMenu : ContextMenu<IReadOnlyList<IMoBiSimulation>, IMoBiContext>
   {
      public MultipleSimulationNodeContextMenu(IReadOnlyList<IMoBiSimulation> simulations, IMoBiContext context, IContainer container)
         : base(simulations, context, container)
      {
      }

      private IMenuBarItem createStartParameterIdentificationMenuBarItem(IReadOnlyList<IMoBiSimulation> simulations)
      {
         return ParameterIdentificationContextMenuItems.CreateParameterIdentificationFor(simulations, _container);
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<IMoBiSimulation> simulations, IMoBiContext context)
      {
         if (simulations.Count == 2)
            yield return ComparisonCommonContextMenuItems.CompareObjectsMenu(simulations, context, _container);

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(simulations, _container);

         yield return createStartParameterIdentificationMenuBarItem(simulations);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveMultipleSimulationsUICommand, IReadOnlyList<IMoBiSimulation>>(simulations, _container)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }
   }
}