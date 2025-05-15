using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using System.Drawing;

namespace MoBi.R.MinimalImplementations
{
   public class MoBiReactionDiagramManager : IMoBiReactionDiagramManager
   {
      public MoBiReactionBuildingBlock PkModel { get; set; }

      public bool IsInitialized { get; set; }

      public IDiagramOptions DiagramOptions { get; set; }

      public PointF CurrentInsertLocation { get; set; }

      public void AddMolecule(ReactionBuilder reactionBuilder, string moleculeName)
      {
         //Nothing to do
      }

      public IMoleculeNode AddMoleculeNode(string moleculeName)
      {
         return null;
      }

      public void AddObjectBase(IObjectBase objectBase)
      {
         //Nothing to do
      }

      public void Cleanup()
      {
         //Nothing to do
      }

      public IDiagramManager<MoBiReactionBuildingBlock> Create()
      {
         return new MoBiReactionDiagramManager();
      }

      public IEnumerable<IMoleculeNode> GetMoleculeNodes(string moleculeName)
      {
         return new List<IMoleculeNode>();
      }

      public IEnumerable<IMoleculeNode> GetMoleculeNodes()
      {
         return new List<IMoleculeNode>();
      }

      public void InitializeWith(MoBiReactionBuildingBlock pkModel, IDiagramOptions diagramOptions)
      {
         //Nothing to do
      }

      public bool InsertLocationHasChanged()
      {
         return false;
      }

      public bool MustHandleExisting(string id)
      {
         return false;
      }

      public ObjectPath PathForNodeWithoutEntity(IContainerNode containerNode)
      {
         return new ObjectPath();
      }

      public IReactionNode ReactionNodeFor(ReactionBuilder reactionBuilder)
      {
         return null;
      }

      public void RefreshDiagramFromModel()
      {
         //Nothing to do
      }

      public void RefreshFromDiagramOptions()
      {
         //Nothing to do
      }

      public void RefreshObjectBase(IObjectBase objectBase)
      {
         //Nothing to do
      }

      public void RemoveMolecule(ReactionBuilder reactionBuilder, string moleculeName)
      {
         //Nothing to do
      }

      public void RemoveMoleculeNode(IMoleculeNode moleculeNode)
      {
         //Nothing to do
      }

      public void RemoveObjectBase(IObjectBase objectBase)
      {
         //Nothing to do
      }

      public void RenameMolecule(ReactionBuilder reactionBuilder, string oldMoleculeName, string newMoleculeName)
      {
         //Nothing to do
      }

      public void UpdateInsertLocation()
      {
         //Nothing to do
      }

      public void UpdateReactionBuilder(IObjectBase reactionAsObjectBase, IBaseNode reactionNodeAsBaseNode)
      {
         //Nothing to do
      }

      public void UpdateReactionBuilder(ReactionBuilder reactionBuilder)
      {
         //Nothing to do
      }
   }
}