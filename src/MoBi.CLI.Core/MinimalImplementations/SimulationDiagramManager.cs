using System;
using System.Drawing;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace MoBi.CLI.Core.MinimalImplementations
{
   public class SimulationDiagramManager : ISimulationDiagramManager
   {
      public IMoBiSimulation PkModel => throw new NotImplementedException();

      public bool IsInitialized => throw new NotImplementedException();

      public IDiagramOptions DiagramOptions => throw new NotImplementedException();

      public PointF CurrentInsertLocation
      {
         get => throw new NotImplementedException();
         set => throw new NotImplementedException();
      }

      public void AddObjectBase(IObjectBase objectBase)
      {
         //Nothing to do
      }

      public void Cleanup()
      {
         //Nothing to do
      }

      public IDiagramManager<IMoBiSimulation> Create()
      {
         return new SimulationDiagramManager();
      }

      public void InitializeWith(IMoBiSimulation pkModel, IDiagramOptions diagramOptions)
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