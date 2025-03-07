using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IModulesInExplorerPresenter : IClassificationInExplorerPresenter
   {
      void AddModulesToTree(MoBiProject project);
   }

   public class ModulesInExplorerPresenter : ClassificationInExplorerPresenter<ClassifiableModule>, IModulesInExplorerPresenter
   {
      private readonly ITreeNodeFactory _moBiTreeNodeFactory;

      public ModulesInExplorerPresenter(ITreeNodeFactory treeNodeFactory) : base(treeNodeFactory)
      {
         _moBiTreeNodeFactory = treeNodeFactory;
      }

      public bool CanDrag(ITreeNode node)
      {
         if (node.IsAnImplementationOf<ModuleNode>())
            return true;

         var rootNode = node.TagAsObject as RootNodeType;
         if (rootNode == null)
            return false;

         return rootNode == _rootNodeType;
      }

      public void AddModulesToTree(MoBiProject project)
      {
         project.Modules.Each(x => project.GetOrCreateClassifiableFor<ClassifiableModule, Module>(x));
         AddAllClassificationsToTree(project);
      }

      protected override ITreeNode CreateNodeFor(ClassifiableModule classifiable) => _moBiTreeNodeFactory.CreateFor(classifiable);

      protected override ClassificationType ClassificationType => ClassificationType.Module;
   }
}
