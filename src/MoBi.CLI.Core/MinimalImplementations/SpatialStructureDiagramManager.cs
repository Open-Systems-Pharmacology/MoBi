using System;
using System.Drawing;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class SpatialStructureDiagramManager : ISpatialStructureDiagramManager
   {
      public MoBiSpatialStructure PkModel { get; set; }

      public bool IsInitialized { get; set; }

      public IDiagramOptions DiagramOptions { get; set; }

      public PointF CurrentInsertLocation { get; set; }

      public void AddObjectBase(IObjectBase objectBase)
      {
         //Nothing to do
      }

      public void Cleanup()
      {
         //Nothing to do
      }

      public IDiagramManager<MoBiSpatialStructure> Create()
      {
         return null;
      }

      public void InitializeWith(MoBiSpatialStructure pkModel, IDiagramOptions diagramOptions)
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

      public void RemoveObjectBase(IObjectBase objectBase)
      {
         //Nothing to do
      }

      public void UpdateInsertLocation()
      {
         //Nothing to do
      }
   }
}