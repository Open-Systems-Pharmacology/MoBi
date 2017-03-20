using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Nodes;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Nodes;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation
{
   public abstract class concern_for_MultipleBuildingBlockInfoNodeContextMenuFactory : ContextSpecificationWithLocalContainer<MultipleBuildingBlockInfoNodeContextMenuFactory>
   {
      private IMoBiContext _context;
      protected List<ITreeNode> _treeNodes;
      protected IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> _presenter;

      protected override void Context()
      {
         _context = A.Fake<IMoBiContext>();
         _treeNodes = new List<ITreeNode>();
         _presenter = A.Fake<IPresenterWithContextMenu<IReadOnlyList<ITreeNode>>>();
         sut = new MultipleBuildingBlockInfoNodeContextMenuFactory(_context);
      }
   }

   public class When_checking_if_two_nodes_containing_two_different_building_block_info_types_can_be_used_for_a_comparison : concern_for_MultipleBuildingBlockInfoNodeContextMenuFactory
   {
      private ITreeNode _msvInfoNode;
      private ITreeNode _psvInfoNode;

      protected override void Context()
      {
         base.Context();
         _msvInfoNode = new BuildingBlockInfoNode(new MoleculeStartValuesBuildingBlockInfo {BuildingBlock = new MoleculeStartValuesBuildingBlock()});
         _psvInfoNode = new BuildingBlockInfoNode(new ParameterStartValuesBuildingBlockInfo {BuildingBlock = new ParameterStartValuesBuildingBlock()});
         _treeNodes.Add(_msvInfoNode);
         _treeNodes.Add(_psvInfoNode);
      }

      [Observation]
      public void should_return_false()
      {
         sut.IsSatisfiedBy(_treeNodes, _presenter).ShouldBeFalse();
      }
   }
}