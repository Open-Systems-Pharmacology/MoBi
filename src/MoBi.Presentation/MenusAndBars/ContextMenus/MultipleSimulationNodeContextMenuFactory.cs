using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class MultipleSimulationNodeContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableSimulation>
   {
      private readonly IMoBiContext _context;

      public MultipleSimulationNodeContextMenuFactory(IMoBiContext context)
      {
         _context = context;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableSimulation> classifiableSimulations, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleSimulationNodeContextMenu(classifiableSimulations.Select(x => x.Subject).ToList(), _context);
      }
   }

   public class MultipleSimulationNodeContextMenu : ContextMenu<IReadOnlyList<IMoBiSimulation>, IMoBiContext>
   {
      public MultipleSimulationNodeContextMenu(IReadOnlyList<IMoBiSimulation> simulations, IMoBiContext context)
         : base(simulations, context)
      {
      }

      private IMenuBarItem createStartParameterIdentificationMenuBarItem(IReadOnlyList<IMoBiSimulation> simulations)
      {
         return ParameterIdentificationContextMenuItems.CreateParameterIdentificationFor(simulations);
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<IMoBiSimulation> simulations, IMoBiContext context)
      {
         if (simulations.Count == 2)
            yield return ComparisonCommonContextMenuItems.CompareObjectsMenu(simulations, context);

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(simulations);

         yield return createStartParameterIdentificationMenuBarItem(simulations);

         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
            .WithCommandFor<RemoveMultipleSimulationsUICommand, IReadOnlyList<IMoBiSimulation>>(simulations)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }
   }
}