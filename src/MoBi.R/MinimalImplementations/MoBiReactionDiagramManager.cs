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
      public MoBiReactionBuildingBlock PkModel => throw new System.NotImplementedException();

      public bool IsInitialized => throw new System.NotImplementedException();

      public IDiagramOptions DiagramOptions => throw new System.NotImplementedException();

      public PointF CurrentInsertLocation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

      public void AddMolecule(ReactionBuilder reactionBuilder, string moleculeName)
      {
         throw new System.NotImplementedException();
      }

      public IMoleculeNode AddMoleculeNode(string moleculeName)
      {
         throw new System.NotImplementedException();
      }

      public void AddObjectBase(IObjectBase objectBase)
      {
         throw new System.NotImplementedException();
      }

      public void Cleanup()
      {
         throw new System.NotImplementedException();
      }

      public IDiagramManager<MoBiReactionBuildingBlock> Create()
      {
         throw new System.NotImplementedException();
      }

      public IEnumerable<IMoleculeNode> GetMoleculeNodes(string moleculeName)
      {
         throw new System.NotImplementedException();
      }

      public IEnumerable<IMoleculeNode> GetMoleculeNodes()
      {
         throw new System.NotImplementedException();
      }

      public void InitializeWith(MoBiReactionBuildingBlock pkModel, IDiagramOptions diagramOptions)
      {
         throw new System.NotImplementedException();
      }

      public bool InsertLocationHasChanged()
      {
         throw new System.NotImplementedException();
      }

      public bool MustHandleExisting(string id)
      {
         throw new System.NotImplementedException();
      }

      public ObjectPath PathForNodeWithoutEntity(IContainerNode containerNode)
      {
         throw new System.NotImplementedException();
      }

      public IReactionNode ReactionNodeFor(ReactionBuilder reactionBuilder)
      {
         throw new System.NotImplementedException();
      }

      public void RefreshDiagramFromModel()
      {
         throw new System.NotImplementedException();
      }

      public void RefreshFromDiagramOptions()
      {
         throw new System.NotImplementedException();
      }

      public void RefreshObjectBase(IObjectBase objectBase)
      {
         throw new System.NotImplementedException();
      }

      public void RemoveMolecule(ReactionBuilder reactionBuilder, string moleculeName)
      {
         throw new System.NotImplementedException();
      }

      public void RemoveMoleculeNode(IMoleculeNode moleculeNode)
      {
         throw new System.NotImplementedException();
      }

      public void RemoveObjectBase(IObjectBase objectBase)
      {
         throw new System.NotImplementedException();
      }

      public void RenameMolecule(ReactionBuilder reactionBuilder, string oldMoleculeName, string newMoleculeName)
      {
         throw new System.NotImplementedException();
      }

      public void UpdateInsertLocation()
      {
         throw new System.NotImplementedException();
      }

      public void UpdateReactionBuilder(IObjectBase reactionAsObjectBase, IBaseNode reactionNodeAsBaseNode)
      {
         throw new System.NotImplementedException();
      }

      public void UpdateReactionBuilder(ReactionBuilder reactionBuilder)
      {
         throw new System.NotImplementedException();
      }
   }
}