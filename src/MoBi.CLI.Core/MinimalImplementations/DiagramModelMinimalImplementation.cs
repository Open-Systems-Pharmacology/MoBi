using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class DiagramModelMinimalImplementation : IDiagramModel
   {
      public PointF Location { get; set; }
      public PointF Center { get; set; }
      public SizeF Size { get; set; }
      public RectangleF Bounds { get; set; }

      public IEnumerable<T> GetDirectChildren<T>() where T : class
      {
         return Enumerable.Empty<T>();
      }

      public IEnumerable<T> GetAllChildren<T>() where T : class
      {
         return Enumerable.Empty<T>();
      }

      public void AddChildNode(IBaseNode node)
      {
         //nothing to do
      }

      public void RemoveChildNode(IBaseNode node)
      {
         //nothing to do
      }

      public bool ContainsChildNode(IBaseNode node, bool recursive)
      {
         return false;
      }

      public RectangleF CalculateBounds()
      {
         return new RectangleF(0, 0, 10, 10);
      }

      public void SetHiddenRecursive(bool hidden)
      {
         //nothing to do
      }

      public void PostLayoutStep()
      {
         //nothing to do
      }

      public void Collapse(int level)
      {
         //nothing to do
      }

      public void Expand(int level)
      {
         //nothing to do
      }

      public IBaseNode GetNode(string id)
      {
         return null;
      }

      public T GetNode<T>(string id) where T : class, IBaseNode
      {
         return null;
      }

      public T CreateNode<T>(string id, PointF location, IContainerBase parentContainerBase) where T : class, IBaseNode, new()
      {
         return new T();
      }

      public void RemoveNode(string id)
      {
         //nothing to do
      }

      public void RenameNode(string id, string name)
      {
         //nothing to do
      }

      public IDiagramModel CreateCopy(string containerId = null)
      {
         return new DiagramModelMinimalImplementation();
      }

      public void ReplaceNodeIds(IDictionary<string, string> changedIds)
      {
         //nothing to do
      }

      public bool IsEmpty()
      {
         return true;
      }

      public void Clear()
      {
         //nothing to do
      }

      public void SetDefaultExpansion()
      {
         //nothing to do
      }

      public void RefreshSize()
      {
         //nothing to do
      }

      public void Undo()
      {
         //nothing to do
      }

      public void ClearUndoStack()
      {
         //nothing to do
      }

      public void BeginUpdate()
      {
         //nothing to do
      }

      public void EndUpdate()
      {
         //nothing to do
      }

      public bool StartTransaction()
      {
         return true;
      }

      public bool FinishTransaction(string layoutrecursivedone)
      {
         return true;
      }

      public void ShowDefaultExpansion()
      {
         //nothing to do
      }

      public IBaseNode FindByName(string name)
      {
         return null;
      }

      public void AddNodeId(IBaseNode baseNode)
      {
         //nothing to do
      }

      public bool IsLayouted { get; set; }
      public IDiagramOptions DiagramOptions { get; set; }

      public IDiagramModel Create()
      {
         return new DiagramModelMinimalImplementation();
      }
   }
}