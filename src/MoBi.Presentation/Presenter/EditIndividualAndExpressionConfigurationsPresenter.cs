using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Simulation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IEditIndividualAndExpressionConfigurationsPresenter : ISimulationConfigurationItemPresenter, ICanDragDropPresenter
   {
      void ProjectExpressionSelectionChanged(IReadOnlyList<ITreeNode> selectedNodes);
      void SimulationExpressionSelectionChanged(IReadOnlyList<ITreeNode> selectedNodes);
      void RemoveSelectedExpressions(IReadOnlyList<ITreeNode> selectedNode);
      void AddSelectedExpressions(IReadOnlyList<ITreeNode> selectedNodes);
      int CompareSelectedNodes(ITreeNode node1, ITreeNode node2);
      IndividualBuildingBlock SelectedIndividual { get; }
      IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles { get; }
   }

   public class EditIndividualAndExpressionConfigurationsPresenter : AbstractSubPresenter<IEditIndividualAndExpressionConfigurationsView, IEditIndividualAndExpressionConfigurationsPresenter>, IEditIndividualAndExpressionConfigurationsPresenter
   {
      private readonly ISelectedIndividualToIndividualSelectionDTOMapper _selectedIndividualDTOMapper;
      private readonly ITreeNodeFactory _treeNodeFactory;
      private IndividualSelectionDTO _individualSelectionDTO;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IDialogCreator _dialogCreator;
      private readonly List<ExpressionProfileBuildingBlock> _selectedExpressions;

      public EditIndividualAndExpressionConfigurationsPresenter(IEditIndividualAndExpressionConfigurationsView view, ISelectedIndividualToIndividualSelectionDTOMapper selectedIndividualDTOMapper,
         ITreeNodeFactory treeNodeFactory, IBuildingBlockRepository buildingBlockRepository, IDialogCreator dialogCreator) : base(view)
      {
         _selectedExpressions = new List<ExpressionProfileBuildingBlock>();
         _selectedIndividualDTOMapper = selectedIndividualDTOMapper;
         _treeNodeFactory = treeNodeFactory;
         _buildingBlockRepository = buildingBlockRepository;
         _dialogCreator = dialogCreator;
      }

      public void Edit(SimulationConfiguration simulationConfiguration)
      {
         _individualSelectionDTO = _selectedIndividualDTOMapper.MapFrom(simulationConfiguration.Individual);
         initializeSelectedExpressions(simulationConfiguration);
         addUnusedExpressionsToSelectionView();
         addUsedExpressionsToSelectedView();

         _view.BindTo(_individualSelectionDTO);
      }

      private void initializeSelectedExpressions(SimulationConfiguration simulationConfiguration)
      {
         _buildingBlockRepository.ExpressionProfileCollection.Where(x => x.NameIsOneOf(simulationConfiguration.ExpressionProfiles.AllNames())).Each(_selectedExpressions.Add);
      }

      private void addUsedExpressionsToSelectedView()
      {
         _selectedExpressions.Each(addUsedExpressionToSelectedView);
      }

      private void addUsedExpressionToSelectedView(ExpressionProfileBuildingBlock expression)
      {
         _view.AddUsedExpression(_treeNodeFactory.CreateFor(expression));
      }

      private void addUnusedExpressionsToSelectionView()
      {
         _buildingBlockRepository.ExpressionProfileCollection.Where(x => !_selectedExpressions.ExistsByName(x.Name)).Each(addUnusedExpressionToSelectionView);
      }

      private void addUnusedExpressionToSelectionView(ExpressionProfileBuildingBlock expression)
      {
         _view.AddUnusedExpression(_treeNodeFactory.CreateFor(expression));
      }

      public void ProjectExpressionSelectionChanged(IReadOnlyList<ITreeNode> selectedNodes)
      {
         _view.EnableAdd = selectedNodes.Any();
      }

      public void SimulationExpressionSelectionChanged(IReadOnlyList<ITreeNode> selectedNodes)
      {
         _view.EnableRemove = selectedNodes.Any();
      }

      public void RemoveSelectedExpressions(IReadOnlyList<ITreeNode> selectedNodes)
      {
         selectedNodes.Each(removeSelectedExpression);
      }

      private void removeSelectedExpression(ITreeNode selectedNode)
      {
         var expression = expressionProfileFromNode(selectedNode);
         if (expression == null)
            return;

         _selectedExpressions.Remove(expression);
         addUnusedExpressionToSelectionView(expression);
         _view.RemoveUsedExpression(selectedNode);
      }

      public void AddSelectedExpressions(IReadOnlyList<ITreeNode> selectedNodes)
      {
         // We need the ToList because all nodes must be evaluated, then we are testing if any
         // nodes failed to be added to the selection
         var nodesNotAdded = selectedNodes.Where(x => !addSelectedExpression(x)).Select(expressionProfileFromNode).ToList();
         if (nodesNotAdded.Any())
            _dialogCreator.MessageBoxInfo(AppConstants.Captions.CouldNotAddExpressionProfilesDuplicatingProtein(nodesNotAdded.Select(x => x.MoleculeName).Distinct().ToList()));
      }

      private static ExpressionProfileBuildingBlock expressionProfileFromNode(ITreeNode treeNode)
      {
         return treeNode.TagAsObject as ExpressionProfileBuildingBlock;
      }

      private bool addSelectedExpression(ITreeNode selectedNode)
      {
         var expression = expressionProfileFromNode(selectedNode);
         if (expression == null)
            return false;

         if (_selectedExpressions.Any(x => Equals(expression.MoleculeName, x.MoleculeName)))
            return false;

         _selectedExpressions.Add(expression);
         addUsedExpressionToSelectedView(expression);
         _view.RemoveUnusedExpression(selectedNode);

         return true;
      }

      public int CompareSelectedNodes(ITreeNode node1, ITreeNode node2)
      {
         var expression1 = node1.TagAsObject as ExpressionProfileBuildingBlock;
         var expression2 = node2.TagAsObject as ExpressionProfileBuildingBlock;

         return _selectedExpressions.IndexOf(expression1) - _selectedExpressions.IndexOf(expression2);
      }

      public IndividualBuildingBlock SelectedIndividual => _individualSelectionDTO.IsNull() ? null : _individualSelectionDTO.SelectedIndividualBuildingBlock;
      public IReadOnlyList<ExpressionProfileBuildingBlock> ExpressionProfiles => _selectedExpressions;

      public bool CanDrag(ITreeNode node)
      {
         return node.TagAsObject is ExpressionProfileBuildingBlock;
      }

      public bool CanDrop(ITreeNode dragNode, ITreeNode targetNode)
      {
         var expression = expressionProfileFromNode(targetNode);
         return expression != null;
      }

      public void DropNode(ITreeNode dragNode, ITreeNode targetNode, DragDropKeyState keyState = DragDropKeyState.None)
      {
         var movingExpression = dragNode.TagAsObject as ExpressionProfileBuildingBlock;
         var targetExpression = targetNode.TagAsObject as ExpressionProfileBuildingBlock;

         _selectedExpressions.Remove(movingExpression);
         _selectedExpressions.Insert(_selectedExpressions.IndexOf(targetExpression), movingExpression);
         _view.SortSelectedExpressions();
      }
   }

   public interface IEditIndividualAndExpressionConfigurationsView : IView<IEditIndividualAndExpressionConfigurationsPresenter>
   {
      void BindTo(IndividualSelectionDTO individualSelectionDTO);
      void AddUnusedExpression(ITreeNode treeNodeToAdd);
      void AddUsedExpression(ITreeNode treeNodeToAdd);
      bool EnableAdd { get; set; }
      bool EnableRemove { get; set; }
      void RemoveUsedExpression(ITreeNode selectedNode);
      void RemoveUnusedExpression(ITreeNode selectedNode);
      void SortSelectedExpressions();
   }
}