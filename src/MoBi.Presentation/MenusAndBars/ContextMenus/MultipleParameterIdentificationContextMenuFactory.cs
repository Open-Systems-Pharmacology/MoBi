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
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;
using static OSPSuite.Assets.Captions;
using ParameterIdentification = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentification;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

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

        //protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableSimulation> classifiableSimulations, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
        //{
        //    var moBiSimulations = classifiableSimulations.Select(x => x.Subject).ToList();
        //    return new MultipleParameterIdentificationContextMenu(moBiSimulations, _context, _container);
        //}

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

        private IMenuBarItem createStartParameterIdentificationMenuBarItem(IReadOnlyList<ParameterIdentification> parameterIdentifications)
        {
            return ParameterIdentificationContextMenuItems.CreateParameterIdentificationFor(parameterIdentifications[0].AllSimulations, _container);
        }

        //protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<IMoBiSimulation> simulations, IMoBiContext context)
        protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ParameterIdentification> parameterIdentifications, IMoBiContext context)
        {
            if (parameterIdentifications.Count != 0)
            {

                //if (parameterIdentifications.Count == 2)
                //    yield return ComparisonCommonContextMenuItems.CompareObjectsMenu(parameterIdentifications, context, _container);

                yield return ObjectBaseCommonContextMenuItems.AddToJournal(parameterIdentifications, _container);

                //yield return createStartParameterIdentificationMenuBarItem(parameterIdentifications);

                yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.Delete)
                    .WithCommandFor<DeleteParameterIdentificationUICommand, ParameterIdentification>(parameterIdentifications[0], _container)
                    .WithIcon(ApplicationIcons.Delete)
                    .AsGroupStarter();
            }
        }
    }
}
