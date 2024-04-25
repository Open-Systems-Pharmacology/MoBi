using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Presentation.UICommands;
using IContainer = OSPSuite.Utility.Container.IContainer;
using ParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
    public class MultipleParameterIdentificationContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableParameterIdentification>
    {
        private readonly IMoBiContext _context;
        private readonly IContainer _container;

        public MultipleParameterIdentificationContextMenuFactory(IMoBiContext context, IContainer container)
        {
            _context = context;
            _container = container;
        }

        public override bool IsSatisfiedBy(IReadOnlyList<ITreeNode> treeNodes, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
        {
            return treeNodes.All(node => node.IsAnImplementationOf<ParameterIdentificationNode>());
        }

        protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableParameterIdentification> parameterIdentifications, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
        {
            var paramIdentificationList = parameterIdentifications.Select(pi => pi.ParameterIdentification).ToList().AsReadOnly();
            return new MultipleParameterIdentificationContextMenu(paramIdentificationList, _context, _container);
        }
    }

    public class MultipleParameterIdentificationContextMenu : ContextMenu<IReadOnlyList<ParameterIdentification>, IMoBiContext>
    {
        public MultipleParameterIdentificationContextMenu(IReadOnlyList<ParameterIdentification> parameterIdentifications, IMoBiContext context, IContainer container)
            : base(parameterIdentifications, context, container)
        {
        }

        protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ParameterIdentification> parameterIdentifications, IMoBiContext context)
        {
            yield return ObjectBaseCommonContextMenuItems.AddToJournal(parameterIdentifications, _container);

            yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
                .WithCommandFor<RemoveMultipleParameterIdentificationsUICommand, IReadOnlyList<ParameterIdentification>>(parameterIdentifications, _container)
                .WithIcon(ApplicationIcons.Delete)
                .AsGroupStarter();
        }
    }
}
