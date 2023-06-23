using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditModuleConfigurationsView : IView<IEditModuleConfigurationsPresenter>
   {
      void BindModuleConfiguration(ModuleConfigurationDTO moduleConfiguration);
      void UnbindModuleConfiguration();

      void AddModuleNode(ITreeNode moduleNode);
      void AddModuleConfigurationNode(ITreeNode nodeToAdd);

      /// <summary>
      /// Removes the node from the selection view. When the node is added to the simulation
      /// it is removed from the selection view
      /// </summary>
      /// <param name="selectedModule">The module to be removed</param>
      void RemoveNodeFromSelectionView(ITreeNode selectedModule);
      
      /// <summary>
      /// Removes a tree node from the selected view. When the node is removed from the simulation
      /// it is removed from the selected view
      /// </summary>
      /// <param name="selectedModuleToRemove"></param>
      void RemoveNodeFromSelectedView(ITreeNode selectedModuleToRemove);
      
      bool EnableRemove { get; set; }
      bool EnableAdd { get; set; }
      bool EnableUp { get; set; }
      bool EnableDown { get; set; }
      void SortSelectedModules();
      void AddNodeToSelectedModuleConfigurations(ITreeNode nodeToAdd);
   }
}
