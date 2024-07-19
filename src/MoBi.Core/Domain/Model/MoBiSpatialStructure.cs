using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Domain.Model
{
   public class MoBiSpatialStructure : SpatialStructure, IWithDiagramFor<MoBiSpatialStructure>
   {
      public IDiagramModel DiagramModel { get; set; }
      public IDiagramManager<MoBiSpatialStructure> DiagramManager { get; set; }

      /// <summary>
      ///    Returns all neighborhoods defined in the spatial structure having at least one of its neighbor connected to a
      ///    neighbor from <paramref name="neighbors" />
      /// </summary>
      public IReadOnlyList<NeighborhoodBuilder> GetConnectingNeighborhoods(IReadOnlyList<IContainer> neighbors, IObjectPathFactory objectPathFactory)
      {

         //Returns all possible physical containers that can be taken into consideration
         var allContainers = neighbors
            .SelectMany(cont => cont.GetAllContainersAndSelf<IContainer>(x => !x.IsAnImplementationOf<IParameter>()))
            .Where(x => x.Mode == ContainerMode.Physical)
            .ToList();

         //We use a hashset since we might return the same neighborhood multiple times for different neighbors
         var neighborhoods = new HashSet<NeighborhoodBuilder>();
         allContainers.Select(objectPathFactory.CreateAbsoluteObjectPath)
            .SelectMany(containerPath => Neighborhoods.Where(x => x.IsConnectedTo(containerPath)))
            .Each(n => neighborhoods.Add(n));

         return neighborhoods.ToList();
      }

      public override void UpdatePropertiesFrom(IUpdatable sourceObject, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(sourceObject, cloneManager);
         var sourceSpatialStructure = sourceObject as MoBiSpatialStructure;
         if (sourceSpatialStructure == null) return;

         this.UpdateDiagramFrom(sourceSpatialStructure);
      }
   }
}