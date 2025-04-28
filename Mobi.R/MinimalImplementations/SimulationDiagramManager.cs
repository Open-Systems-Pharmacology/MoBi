using System;
using System.Drawing;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;

namespace MoBi.R.MinimalImplementations
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
         throw new NotImplementedException();
      }

      public void Cleanup()
      {
         throw new NotImplementedException();
      }

      public IDiagramManager<IMoBiSimulation> Create()
      {
         throw new NotImplementedException();
      }

      public void InitializeWith(IMoBiSimulation pkModel, IDiagramOptions diagramOptions)
      {
         throw new NotImplementedException();
      }

      public bool InsertLocationHasChanged()
      {
         throw new NotImplementedException();
      }

      public bool MustHandleExisting(string id)
      {
         throw new NotImplementedException();
      }

      public ObjectPath PathForNodeWithoutEntity(IContainerNode containerNode)
      {
         throw new NotImplementedException();
      }

      public void RefreshDiagramFromModel()
      {
         throw new NotImplementedException();
      }

      public void RefreshFromDiagramOptions()
      {
         throw new NotImplementedException();
      }

      public void RefreshObjectBase(IObjectBase objectBase)
      {
         throw new NotImplementedException();
      }

      public void RemoveObjectBase(IObjectBase objectBase)
      {
         throw new NotImplementedException();
      }

      public void UpdateInsertLocation()
      {
         throw new NotImplementedException();
      }
   }
}